![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 8: Desenvolvimento Orientado por Avaliação com Foundry Local

> **Objetivo:** Construir uma estrutura de avaliação que teste e pontue sistematicamente os seus agentes de IA, usando o mesmo modelo local tanto como agente em teste como juiz, para que possa iterar nos prompts com confiança antes de lançar.

## Porquê o Desenvolvimento Orientado por Avaliação?

Ao construir agentes de IA, "parece estar bem" não é suficiente. O **desenvolvimento orientado por avaliação** trata as saídas dos agentes como código: escreve testes primeiro, mede a qualidade, e só lança quando as pontuações atingem um limiar.

No Zava Creative Writer (Parte 7), o **agente Editor** já atua como um avaliador leve; decide ACEITAR ou REVISAR. Este laboratório formaliza esse padrão numa estrutura de avaliação repetível que pode aplicar a qualquer agente ou pipeline.

| Problema | Solução |
|---------|----------|
| Alterações nos prompts quebram silenciosamente a qualidade | **Conjunto de dados golden** detecta regressões |
| Viés de “funciona num exemplo” | **Múltiplos casos de teste** revelam casos extremos |
| Avaliação de qualidade subjetiva | **Pontuações baseadas em regras + LLM como juiz** fornecem números |
| Nenhuma forma de comparar variantes de prompt | **Execuções de avaliação lado a lado** com pontuações agregadas |

---

## Conceitos-Chave

### 1. Conjuntos de Dados Golden

Um **conjunto de dados golden** é um conjunto curado de casos de teste com saídas esperadas conhecidas. Cada caso de teste inclui:

- **Entrada**: O prompt ou pergunta a enviar ao agente
- **Saída esperada**: O que uma resposta correta ou de alta qualidade deve conter (palavras-chave, estrutura, factos)
- **Categoria**: Agrupamento para relatórios (ex. "precisão factual", "tom", "completude")

### 2. Verificações Baseadas em Regras

Verificações rápidas e determinísticas que não requerem um LLM:

| Verificação | O que testa |
|-------------|-------------|
| **Limites de comprimento** | Resposta não muito curta (preguiçosa) ou demasiado longa (divaga) |
| **Palavras-chave obrigatórias** | Resposta menciona termos ou entidades esperados |
| **Validação de formato** | JSON é analisável, cabeçalhos Markdown estão presentes |
| **Conteúdo proibido** | Sem nomes de marcas alucinados, sem menções a concorrentes |

### 3. LLM como Juiz

Utilize o **mesmo modelo local** para avaliar as suas próprias saídas (ou saídas de uma variante de prompt diferente). O juiz recebe:

- A questão original
- A resposta do agente
- Os critérios de avaliação

E devolve uma pontuação estruturada. Isto espelha o padrão do Editor da Parte 7 mas aplicado de forma sistemática numa suíte de testes.

### 4. Ciclo de Iteração Orientado por Avaliação

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Pré-requisitos

| Requisito | Detalhes |
|-----------|----------|
| **CLI Foundry Local** | Instalada com um modelo descarregado |
| **Runtime de linguagem** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9+ SDK** |
| **Concluído** | [Parte 5: Agentes Individuais](part5-single-agents.md) e [Parte 6: Fluxos de Trabalho Multi-Agente](part6-multi-agent-workflows.md) |

---

## Exercícios do Laboratório

### Exercício 1 - Execute a Estrutura de Avaliação

O workshop inclui um exemplo completo de avaliação que testa um agente Foundry Local contra um conjunto de dados golden de perguntas relacionadas com Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Configuração:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Executar:**
```bash
python foundry-local-eval.py
```

**O que acontece:**
1. Liga-se ao Foundry Local e carrega o modelo
2. Define um conjunto de dados golden com 5 casos de teste (perguntas sobre produtos Zava)
3. Executa duas variantes de prompt contra cada caso de teste
4. Avalia cada resposta com **verificações baseadas em regras** (comprimento, palavras-chave, formato)
5. Avalia cada resposta com **LLM como juiz** (o mesmo modelo pontua qualidade 1-5)
6. Imprime uma tabela comparativa das variantes de prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Configuração:**
```bash
cd javascript
npm install
```

**Executar:**
```bash
node foundry-local-eval.mjs
```

**Mesma pipeline de avaliação:** conjunto de dados golden, execuções duplas de prompt, pontuação baseada em regras + LLM, tabela de pontuações.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Configuração:**
```bash
cd csharp
dotnet restore
```

**Executar:**
```bash
dotnet run eval
```

**Mesma pipeline de avaliação:** conjunto de dados golden, execuções duplas de prompt, pontuação baseada em regras + LLM, tabela de pontuações.

</details>

---

### Exercício 2 - Compreender o Conjunto de Dados Golden

Examine os casos de teste definidos no exemplo de avaliação. Cada caso de teste tem:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Perguntas a considerar:**
1. Porque é que os valores esperados são **palavras-chave** em vez de frases completas?
2. Quantos casos de teste são necessários para uma avaliação fiável?
3. Que categorias adicionaria para a sua própria aplicação?

---

### Exercício 3 - Compreender a Pontuação Baseada em Regras vs LLM como Juiz

A estrutura de avaliação usa **duas camadas de pontuação**:

#### Verificações Baseadas em Regras (rápidas, determinísticas)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM como Juiz (nuanciada, qualitativa)

O mesmo modelo local atua como juiz com um rubrica estruturada:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**Perguntas a considerar:**
1. Quando confiaria mais nas verificações baseadas em regras do que no LLM como juiz?
2. Pode um modelo avaliar fiavelmente a sua própria saída? Quais são as limitações?
3. Como se compara isto ao padrão do agente Editor da Parte 7?

---

### Exercício 4 - Comparar Variantes de Prompt

O exemplo executa **duas variantes de prompt** contra os mesmos casos de teste:

| Variante | Estilo de Prompt do Sistema |
|----------|-----------------------------|
| **Base** | Genérico: "Você é um assistente prestável" |
| **Especializado** | Detalhado: "Você é um especialista Zava DIY que recomenda produtos específicos e fornece orientação passo a passo" |

Após a execução, verá uma tabela pontuadora como:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**Exercícios:**
1. Execute a avaliação e anote as pontuações para cada variante
2. Modifique o prompt especializado para ser ainda mais específico. A pontuação melhora?
3. Adicione uma terceira variante de prompt e compare as três.
4. Experimente mudar o alias do modelo (ex. `phi-4-mini` vs `phi-3.5-mini`) e compare resultados.

---

### Exercício 5 - Aplique a Avaliação ao Seu Próprio Agente

Use a estrutura de avaliação como modelo para os seus próprios agentes:

1. **Defina o seu conjunto de dados golden**: escreva 5 a 10 casos de teste com palavras-chave esperadas.
2. **Escreva o seu prompt do sistema**: as instruções do agente que pretende testar.
3. **Execute a avaliação**: obtenha pontuações base.
4. **Itere**: ajuste o prompt, execute novamente e compare.
5. **Defina um limiar**: ex. "não lançar abaixo de 0.75 de pontuação combinada".

---

### Exercício 6 - Ligação ao Padrão Editor do Zava

Reveja o agente Editor do Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# O editor é um LLM-como-juiz em produção:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Este é o **mesmo conceito** do LLM como juiz da Parte 8, mas embutido na pipeline de produção em vez de numa suíte de testes offline. Ambos os padrões:

- Usam output JSON estruturado do modelo.
- Aplicam critérios de qualidade definidos no prompt do sistema.
- Tomam uma decisão de passar/falhar.

**A diferença:** O editor corre em produção (a cada pedido). A estrutura de avaliação corre em desenvolvimento (antes de lançar).

---

## Conclusões-Chave

| Conceito | Conclusão |
|----------|------------|
| **Conjuntos de dados golden** | Curar casos de teste cedo; são os seus testes de regressão para IA |
| **Verificações baseadas em regras** | Rápidas, determinísticas, capturam falhas óbvias (comprimento, palavras-chave, formato) |
| **LLM como juiz** | Pontuação qualitativa nuanciada usando o mesmo modelo local |
| **Comparação de prompts** | Execute múltiplas variantes no mesmo conjunto de testes para escolher a melhor |
| **Vantagem local** | Toda a avaliação corre localmente: sem custos de API, sem limites de taxa, sem dados a sair da sua máquina |
| **Avaliação antes do lançamento** | Defina limiares de qualidade e bloqueie lançamentos com base nas pontuações de avaliação |

---

## Próximos Passos

- **Escalar**: Adicione mais casos de teste e categorias ao seu conjunto de dados golden.
- **Automatizar**: Integre a avaliação no seu pipeline CI/CD.
- **Juízes avançados**: Use um modelo maior como juiz enquanto testa a saída de um modelo menor.
- **Monitorizar ao longo do tempo**: Guarde os resultados da avaliação para comparar versões de prompt e modelo.

---

## Próximo Laboratório

Continue para [Parte 9: Transcrição de Voz com Whisper](part9-whisper-voice-transcription.md) para explorar fala para texto local usando o Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor tenha em consideração que as traduções automáticas podem conter erros ou imprecisões. O documento original no seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, é recomendada a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas resultantes da utilização desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->