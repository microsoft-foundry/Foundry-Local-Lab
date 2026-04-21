![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 8: Desenvolvimento Guiado por Avaliação com Foundry Local

> **Objetivo:** Construir uma estrutura de avaliação que testa e pontua sistematicamente seus agentes de IA, utilizando o mesmo modelo local tanto como agente em teste quanto como juiz, para que você possa iterar nos prompts com confiança antes de lançar.

## Por que Desenvolvimento Guiado por Avaliação?

Ao construir agentes de IA, "parece mais ou menos certo" não é suficiente. **Desenvolvimento guiado por avaliação** trata as saídas do agente como código: você escreve testes primeiro, mede a qualidade e só lança quando as pontuações alcançam um limite.

No Zava Creative Writer (Parte 7), o **agente Editor** já atua como um avaliador leve; ele decide ACEITAR ou REVISAR. Este laboratório formaliza esse padrão em uma estrutura de avaliação repetível que você pode aplicar a qualquer agente ou pipeline.

| Problema | Solução |
|---------|----------|
| Mudanças no prompt quebram silenciosamente a qualidade | **Conjunto de dados golden** detecta regressões |
| Viés de "funciona em um exemplo" | **Múltiplos casos de teste** revelam casos extremos |
| Avaliação de qualidade subjetiva | **Pontuação baseada em regras + LLM como juiz** fornece números |
| Sem forma de comparar variantes de prompt | **Execuções de avaliação lado a lado** com pontuações agregadas |

---

## Conceitos Chave

### 1. Conjuntos de Dados Golden

Um **conjunto de dados golden** é um conjunto selecionado de casos de teste com saídas esperadas conhecidas. Cada caso de teste inclui:

- **Entrada**: O prompt ou pergunta enviada ao agente
- **Saída esperada**: O que uma resposta correta ou de alta qualidade deve conter (palavras-chave, estrutura, fatos)
- **Categoria**: Agrupamento para relatório (ex.: "precisão factual", "tom", "completude")

### 2. Verificações Baseadas em Regras

Verificações rápidas e determinísticas que não requerem um LLM:

| Verificação | O que Testa |
|-------------|-------------|
| **Limites de comprimento** | Resposta não é muito curta (preguiçosa) nem muito longa (verborrágica) |
| **Palavras-chave obrigatórias** | Resposta menciona termos ou entidades esperadas |
| **Validação de formato** | JSON é analisável, cabeçalhos Markdown estão presentes |
| **Conteúdo proibido** | Sem nomes de marcas alucinados, sem menções a concorrentes |

### 3. LLM como Juiz

Use o **mesmo modelo local** para avaliar suas próprias saídas (ou saídas de uma variante diferente do prompt). O juiz recebe:

- A pergunta original
- A resposta do agente
- Critérios de avaliação

E retorna uma pontuação estruturada. Isso espelha o padrão Editor da Parte 7, mas aplicado sistematicamente numa suíte de testes.

### 4. Ciclo de Iteração Guiado por Avaliação

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Instalado com um modelo baixado |
| **Runtime de linguagem** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9+ SDK** |
| **Concluído** | [Parte 5: Agentes Simples](part5-single-agents.md) e [Parte 6: Workflows Multi-Agentes](part6-multi-agent-workflows.md) |

---

## Exercícios de Laboratório

### Exercício 1 - Execute a Estrutura de Avaliação

O workshop inclui um exemplo completo de avaliação que testa um agente Foundry Local contra um conjunto de dados golden com perguntas relacionadas ao Zava DIY.

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

**Execução:**
```bash
python foundry-local-eval.py
```

**O que acontece:**
1. Conecta ao Foundry Local e carrega o modelo
2. Define um conjunto de dados golden com 5 casos de teste (perguntas sobre produtos Zava)
3. Executa duas variantes de prompt para cada caso de teste
4. Pontua cada resposta com **verificações baseadas em regras** (comprimento, palavras-chave, formato)
5. Pontua cada resposta com **LLM como juiz** (o mesmo modelo avalia qualidade de 1-5)
6. Imprime uma tabela comparando ambas as variantes de prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Configuração:**
```bash
cd javascript
npm install
```

**Execução:**
```bash
node foundry-local-eval.mjs
```

**Mesmo pipeline de avaliação:** conjunto de dados golden, execuções duplas de prompt, pontuação baseada em regras + LLM, tabela de pontuação.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Configuração:**
```bash
cd csharp
dotnet restore
```

**Execução:**
```bash
dotnet run eval
```

**Mesmo pipeline de avaliação:** conjunto de dados golden, execuções duplas de prompt, pontuação baseada em regras + LLM, tabela de pontuação.

</details>

---

### Exercício 2 - Entenda o Conjunto de Dados Golden

Examine os casos de teste definidos no exemplo de avaliação. Cada caso de teste tem:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Perguntas para considerar:**
1. Por que os valores esperados são **palavras-chave** em vez de sentenças completas?
2. Quantos casos de teste são necessários para uma avaliação confiável?
3. Quais categorias você adicionaria para sua própria aplicação?

---

### Exercício 3 - Entenda a Pontuação Baseada em Regras vs LLM como Juiz

A estrutura de avaliação usa **duas camadas de pontuação**:

#### Verificações Baseadas em Regras (rápidas, determinísticas)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM como Juiz (nuançado, qualitativo)

O mesmo modelo local atua como juiz com uma rubrica estruturada:

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

**Perguntas para considerar:**
1. Quando você confiaria mais nas verificações baseadas em regras do que no LLM como juiz?
2. Um modelo pode julgar confiavelmente sua própria saída? Quais são as limitações?
3. Como isso se compara ao padrão agente Editor da Parte 7?

---

### Exercício 4 - Compare Variantes de Prompt

O exemplo executa **duas variantes de prompt** contra os mesmos casos de teste:

| Variante | Estilo do Prompt de Sistema |
|---------|-------------------|
| **Baseline** | Genérico: "Você é um assistente útil" |
| **Especializado** | Detalhado: "Você é um especialista Zava DIY que recomenda produtos específicos e fornece orientações passo a passo" |

Após a execução, você verá uma tabela como:

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
4. Experimente mudar o alias do modelo (ex.: `phi-4-mini` vs `phi-3.5-mini`) e compare resultados.

---

### Exercício 5 - Aplique a Avaliação ao Seu Próprio Agente

Use a estrutura de avaliação como um modelo para seus próprios agentes:

1. **Defina seu conjunto de dados golden**: escreva de 5 a 10 casos de teste com palavras-chave esperadas.
2. **Escreva seu prompt de sistema**: as instruções ao agente que você quer testar.
3. **Execute a avaliação**: obtenha pontuações base.
4. **Itere**: ajuste o prompt, execute novamente e compare.
5. **Defina um limite**: por exemplo, "não lançar abaixo de 0,75 de pontuação combinada".

---

### Exercício 6 - Conexão com o Padrão Editor do Zava

Reveja o agente Editor do Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# O editor é um LLM como juiz em produção:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Este é o **mesmo conceito** do LLM como juiz da Parte 8, mas incorporado ao pipeline de produção em vez de uma suíte de testes offline. Ambos os padrões:

- Usam saída JSON estruturada do modelo.
- Aplicam critérios de qualidade definidos no prompt do sistema.
- Tomam decisão de aprovado/reprovado.

**A diferença:** O editor roda em produção (em toda requisição). A estrutura de avaliação roda em desenvolvimento (antes do lançamento).

---

## Principais Conclusões

| Conceito | Conclusão |
|---------|----------|
| **Conjuntos de dados golden** | Selecione casos de teste cedo; são seus testes de regressão para IA |
| **Verificações baseadas em regras** | Rápidas, determinísticas, capturam falhas óbvias (comprimento, palavras-chave, formato) |
| **LLM como juiz** | Pontuação qualitativa e nuançada usando o mesmo modelo local |
| **Comparação de prompts** | Execute várias variantes na mesma suíte para escolher a melhor |
| **Vantagem on-device** | Toda avaliação roda localmente: sem custos de API, sem limites de taxa, sem dados saindo da sua máquina |
| **Avalie antes de lançar** | Defina limiares de qualidade e restrinja lançamentos às pontuações da avaliação |

---

## Próximos Passos

- **Escalar**: Adicione mais casos de teste e categorias ao seu conjunto golden.
- **Automatizar**: Integre a avaliação ao seu pipeline de CI/CD.
- **Juízes avançados**: Use um modelo maior como juiz enquanto testa a saída de um modelo menor.
- **Acompanhar ao longo do tempo**: Salve resultados de avaliação para comparar versões de prompt e modelo.

---

## Próximo Laboratório

Continue para [Parte 9: Transcrição de Voz com Whisper](part9-whisper-voice-transcription.md) para explorar conversão de fala para texto on-device usando o Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automatizadas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autêntica. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações equivocadas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->