# Problemas Conhecidos — Foundry Local Workshop

Problemas encontrados ao construir e testar este workshop em um dispositivo **Snapdragon X Elite (ARM64)** rodando Windows, com Foundry Local SDK v0.9.0, CLI v0.8.117 e .NET SDK 10.0.

> **Última validação:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Não Reconhecida pelo ONNX Runtime

**Status:** Aberto  
**Gravidade:** Aviso (não bloqueante)  
**Componente:** ONNX Runtime / cpuinfo  
**Reprodução:** Cada início do serviço Foundry Local no hardware Snapdragon X Elite

Toda vez que o serviço Foundry Local inicia, dois avisos são exibidos:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impacto:** Os avisos são apenas estéticos — a inferência funciona corretamente. Contudo, eles aparecem em toda execução e podem confundir os participantes do workshop. A biblioteca cpuinfo do ONNX Runtime precisa ser atualizada para reconhecer os núcleos Qualcomm Oryon CPU.

**Esperado:** Snapdragon X Elite deveria ser reconhecido como um CPU ARM64 suportado sem emitir mensagens de erro.

---

## 2. NullReferenceException no SingleAgent na Primeira Execução

**Status:** Aberto (intermitente)  
**Gravidade:** Crítico (crash)  
**Componente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodução:** Executar `dotnet run agent` — crasha imediatamente após o carregamento do modelo

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Contexto:** A linha 37 chama `model.IsCachedAsync(default)`. O crash ocorreu na primeira execução do agente após um `foundry service stop` recente. Executions subsequentes com o mesmo código sucederam.

**Impacto:** Intermitente — sugere uma condição de corrida na inicialização do serviço do SDK ou na consulta do catálogo. A chamada `GetModelAsync()` pode retornar antes do serviço estar totalmente pronto.

**Esperado:** `GetModelAsync()` deveria bloquear até o serviço estar pronto ou retornar uma mensagem clara de erro se o serviço não estiver inicializado.

---

## 3. SDK C# Requer RuntimeIdentifier Explícito

**Status:** Aberto — monitorado em [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Gravidade:** Lacuna na documentação  
**Componente:** pacote NuGet `Microsoft.AI.Foundry.Local`  
**Reprodução:** Criar um projeto .NET 8+ sem `<RuntimeIdentifier>` no `.csproj`

A compilação falha com:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Causa:** A exigência do RID é esperada — o SDK inclui binários nativos (P/Invoke em `Microsoft.AI.Foundry.Local.Core` e ONNX Runtime), logo o .NET precisa saber qual biblioteca específica da plataforma resolver.

Isto está documentado no MS Learn ([Como usar chat completions nativas](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), onde as instruções de execução mostram:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Entretanto, os usuários precisam lembrar da flag `-r` toda vez, o que é fácil esquecer.

**Solução provisória:** Adicione um fallback de autodetecção ao seu `.csproj` para que `dotnet run` funcione sem flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` é uma propriedade MSBuild embutida que resolve automaticamente o RID da máquina host. Os próprios projetos de teste do SDK já usam esse padrão. Flags explíticas `-r` ainda são respeitadas quando fornecidas.

> **Nota:** O `.csproj` do workshop inclui este fallback para que `dotnet run` funcione imediatamente em qualquer plataforma.

**Esperado:** O template `.csproj` na documentação MS Learn deve incluir esse padrão de autodetecção para que os usuários não precisem se lembrar da flag `-r`.

---

## 4. Whisper JavaScript — Transcrição de Áudio Retorna Saída Vazia/Binária

**Status:** Aberto (regressão — piorou desde o relatório inicial)  
**Gravidade:** Grave  
**Componente:** Implementação Whisper JavaScript (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodução:** Executar `node foundry-local-whisper.mjs` — todos os arquivos de áudio retornam saída vazia ou binária ao invés de transcrição de texto

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Originalmente somente o 5º arquivo de áudio retornava vazio; a partir da v0.9.x, todos os 5 arquivos retornam um único byte (`\ufffd`) ao invés do texto transcrito. A implementação Python do Whisper usando o OpenAI SDK transcreve os mesmos arquivos corretamente.

**Esperado:** `createAudioClient()` deveria retornar a transcrição de texto correspondente às implementações Python/C#.

---

## 5. SDK C# Distribui Apenas net8.0 — Sem Alvo Oficial para .NET 9 ou .NET 10

**Status:** Aberto  
**Gravidade:** Lacuna na documentação  
**Componente:** pacote NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Comando de instalação:** `dotnet add package Microsoft.AI.Foundry.Local`

O pacote NuGet entrega apenas uma framework alvo:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Nenhum TFM `net9.0` ou `net10.0` está incluído. Por contraste, o pacote acompanhante `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) entrega `net8.0`, `net9.0`, `net10.0`, `net472` e `netstandard2.0`.

### Testes de Compatibilidade

| Framework Alvo | Compila | Executa | Notas |
|----------------|---------|---------|-------|
| net8.0 | ✅ | ✅ | Suportado oficialmente |
| net9.0 | ✅ | ✅ | Compila via compatibilidade futura — usado nos exemplos do workshop |
| net10.0 | ✅ | ✅ | Compila e executa via compatibilidade futura com runtime .NET 10.0.3 |

O assembly net8.0 carrega em runtimes mais novos pelo mecanismo de forward-compat do .NET, portanto a compilação funciona. Porém isso não é documentado nem testado pela equipe do SDK.

### Por Que os Exemplos Usam net9.0

1. **.NET 9 é a versão estável mais recente** — a maioria dos participantes terá instalada  
2. **A compatibilidade futura funciona** — o assembly net8.0 no pacote NuGet roda no runtime .NET 9 sem problemas  
3. **.NET 10 (preview/RC)** é muito novo para usar num workshop que deve funcionar para todos

**Esperado:** Versões futuras do SDK devem considerar adicionar TFMs `net9.0` e `net10.0` paralelamente ao `net8.0` para seguir o padrão usado pelo `Microsoft.Agents.AI.OpenAI` e fornecer suporte validado para runtimes mais recentes.

---

## 6. Streaming do ChatClient JavaScript Usa Callbacks, Não Iteradores Async

**Status:** Aberto  
**Gravidade:** Lacuna na documentação  
**Componente:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

O `ChatClient` retornado por `model.createChatClient()` oferece um método `completeStreamingChat()`, mas usa um **padrão de callback** ao invés de retornar um iterável async:

```javascript
// ❌ Isso NÃO funciona — lança "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Padrão correto — passe uma função de callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impacto:** Desenvolvedores familiarizados com o padrão async iterator do OpenAI SDK (`for await`) terão erros confusos. O callback precisa ser uma função válida ou o SDK lança "Callback must be a valid function."

**Esperado:** Documentar o padrão callback na referência do SDK. Alternativamente, suportar o padrão iterável async para consistência com o OpenAI SDK.

---

## Detalhes do Ambiente

| Componente | Versão |
|------------|---------|
| SO | Windows 11 ARM64 |
| Hardware | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor, esteja ciente de que traduções automatizadas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autoritativa. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->