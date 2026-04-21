# Problemas Conhecidos — Foundry Local Workshop

Problemas encontrados durante a construção e teste deste workshop num dispositivo **Snapdragon X Elite (ARM64)** a executar Windows, com Foundry Local SDK v0.9.0, CLI v0.8.117 e .NET SDK 10.0.

> **Última validação:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Não Reconhecida pelo ONNX Runtime

**Estado:** Aberto  
**Gravidade:** Aviso (não bloqueante)  
**Componente:** ONNX Runtime / cpuinfo  
**Reprodução:** Cada arranque do serviço Foundry Local em hardware Snapdragon X Elite

Sempre que o serviço Foundry Local é iniciado, são emitidos dois avisos:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impacto:** Os avisos são cosméticos — a inferência funciona corretamente. No entanto, aparecem em cada execução e podem confundir os participantes do workshop. A biblioteca cpuinfo do ONNX Runtime precisa ser atualizada para reconhecer os núcleos da CPU Qualcomm Oryon.

**Esperado:** O Snapdragon X Elite deve ser reconhecido como uma CPU ARM64 suportada sem emitir mensagens de erro de nível crítico.

---

## 2. SingleAgent NullReferenceException na Primeira Execução

**Estado:** Aberto (intermitente)  
**Gravidade:** Crítico (crash)  
**Componente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodução:** Executar `dotnet run agent` — crasha imediatamente após carregamento do modelo

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Contexto:** A linha 37 chama `model.IsCachedAsync(default)`. O crash ocorreu na primeira execução do agente após um `foundry service stop` recém-executado. Excecões subsequentes com o mesmo código tiveram sucesso.

**Impacto:** Intermitente — sugere uma condição de corrida na inicialização do serviço do SDK ou na consulta do catálogo. A chamada `GetModelAsync()` pode retornar antes do serviço estar completamente pronto.

**Esperado:** `GetModelAsync()` deve bloquear até que o serviço esteja pronto ou retornar uma mensagem de erro clara se o serviço ainda não terminou a inicialização.

---

## 3. C# SDK Requer RuntimeIdentifier Explícito

**Estado:** Aberto — acompanhado em [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Gravidade:** Falta de documentação  
**Componente:** pacote NuGet `Microsoft.AI.Foundry.Local`  
**Reprodução:** Criar projeto .NET 8+ sem `<RuntimeIdentifier>` no `.csproj`

A compilação falha com:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Causa raiz:** O requisito do RID é esperado — o SDK inclui binários nativos (P/Invoke em `Microsoft.AI.Foundry.Local.Core` e ONNX Runtime), portanto o .NET precisa saber qual biblioteca específica da plataforma resolver.

Isto está documentado na MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), onde as instruções de execução mostram:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
No entanto, os utilizadores têm de se lembrar da flag `-r` em cada execução, o que é fácil esquecer.

**Solução alternativa:** Adicione uma deteção automática fallback ao seu `.csproj` para que `dotnet run` funcione sem flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` é uma propriedade MSBuild incorporada que resolve automaticamente para o RID da máquina hospedeira. Os próprios projetos de teste do SDK já usam este padrão. Flags explícitas `-r` ainda são respeitadas quando fornecidas.

> **Nota:** O `.csproj` do workshop inclui esta fallback para que `dotnet run` funcione imediatamente em qualquer plataforma.

**Esperado:** O template `.csproj` nos documentos MS Learn deve incluir este padrão de deteção automática para que os utilizadores não tenham de se lembrar da flag `-r`.

---

## 4. JavaScript Whisper — Transcrição Áudio Retorna Saída Vazia/Binária

**Estado:** Aberto (regressão — piorou desde o relatório inicial)  
**Gravidade:** Maior  
**Componente:** Implementação JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodução:** Executar `node foundry-local-whisper.mjs` — todos os ficheiros áudio retornam saída vazia ou binária em vez de texto transcrito

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Originalmente apenas o 5º ficheiro de áudio retornava vazio; desde a v0.9.x, todos os 5 ficheiros retornam um único byte (`\ufffd`) em vez do texto transcrito. A implementação Python Whisper usando o SDK OpenAI transcreve corretamente os mesmos ficheiros.

**Esperado:** `createAudioClient()` deve retornar a transcrição de texto correspondente às implementações Python/C#.

---

## 5. C# SDK Apenas Fornece net8.0 — Sem Alvo Oficial para .NET 9 ou .NET 10

**Estado:** Aberto  
**Gravidade:** Falta de documentação  
**Componente:** pacote NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Comando de instalação:** `dotnet add package Microsoft.AI.Foundry.Local`

O pacote NuGet apenas inclui um framework alvo:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Nenhum TFM `net9.0` ou `net10.0` está incluído. Em contraste, o pacote acompanhante `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) inclui `net8.0`, `net9.0`, `net10.0`, `net472` e `netstandard2.0`.

### Testes de Compatibilidade

| Framework Alvo    | Compila | Executa | Notas                       |
|-------------------|---------|---------|-----------------------------|
| net8.0            | ✅      | ✅      | Oficialmente suportado       |
| net9.0            | ✅      | ✅      | Compila via compatibilidade avançada — usado nos exemplos do workshop |
| net10.0           | ✅      | ✅      | Compila e executa via compatibilidade avançada com runtime .NET 10.0.3 |

O assembly net8.0 carrega em runtimes mais recentes através do mecanismo de forward-compatibility do .NET, portanto a compilação tem sucesso. Contudo, isto não está documentado nem testado pela equipa do SDK.

### Porquê os Exemplos Usarem net9.0

1. **.NET 9 é a versão estável mais recente** — a maioria dos participantes do workshop terá esta versão instalada  
2. **A compatibilidade avançada funciona** — o assembly net8.0 no pacote NuGet corre no runtime .NET 9 sem problemas  
3. **.NET 10 (preview/RC)** é demasiado recente para ser alvo num workshop que deve funcionar para todos

**Esperado:** Releases futuras do SDK devem considerar adicionar TFMs `net9.0` e `net10.0` em paralelo com `net8.0` para seguir o padrão usado por `Microsoft.Agents.AI.OpenAI` e providenciar suporte validado para runtimes mais recentes.

---

## 6. Streaming em JavaScript ChatClient Usa Callbacks, Não Iteradores Async

**Estado:** Aberto  
**Gravidade:** Falta de documentação  
**Componente:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

O `ChatClient` retornado por `model.createChatClient()` fornece um método `completeStreamingChat()`, mas usa um **padrão callback** em vez de retornar um iterador async:

```javascript
// ❌ Isto NÃO funciona — lança "stream não é iterável assíncrono"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Padrão correto — passar uma função de callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impacto:** Desenvolvedores familiarizados com o padrão de iteração async do SDK OpenAI (`for await`) encontrarão erros confusos. O callback tem de ser uma função válida ou o SDK lança "Callback must be a valid function."

**Esperado:** Documentar o padrão callback na referência do SDK. Alternativamente, suportar o padrão iterador async para consistência com o SDK OpenAI.

---

## Detalhes do Ambiente

| Componente                 | Versão       |
|----------------------------|--------------|
| SO                         | Windows 11 ARM64 |
| Hardware                   | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI          | 0.8.117      |
| Foundry Local SDK (C#)     | 0.9.0        |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3    |
| OpenAI C# SDK              | 2.9.0        |
| .NET SDK                   | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x        |
| foundry-local-sdk (JS)     | 0.9.x        |
| Node.js                    | 18+          |
| ONNX Runtime               | 1.18+        |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor tenha em conta que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional realizada por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações erradas decorrentes da utilização desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->