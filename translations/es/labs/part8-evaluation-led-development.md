![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 8: Desarrollo Guiado por Evaluación con Foundry Local

> **Objetivo:** Construir un marco de evaluación que pruebe y califique sistemáticamente tus agentes de IA, usando el mismo modelo local tanto como agente bajo prueba y juez, para que puedas iterar en las instrucciones con confianza antes de lanzar.

## ¿Por qué Desarrollo Guiado por Evaluación?

Al construir agentes de IA, "parece correcto" no es suficiente. El **desarrollo guiado por evaluación** trata las salidas del agente como código: escribes pruebas primero, mides la calidad y solo lanzas cuando las puntuaciones alcanzan un umbral.

En el Escritor Creativo de Zava (Parte 7), el **agente Editor** ya actúa como un evaluador ligero; decide ACEPTAR o REVISAR. Este laboratorio formaliza ese patrón en un marco de evaluación repetible que puedes aplicar a cualquier agente o flujo.

| Problema | Solución |
|---------|----------|
| Cambios en el prompt rompen la calidad silenciosamente | El **conjunto de datos dorado** detecta regresiones |
| Sesgo de "funciona en un ejemplo" | **Casos de prueba múltiples** revelan casos límite |
| Evaluación subjetiva de calidad | **Puntuación basada en reglas + LLM como juez** da números |
| Sin forma de comparar variantes de prompt | **Evaluaciones lado a lado** con puntuaciones agregadas |

---

## Conceptos Clave

### 1. Conjuntos de Datos Dorados

Un **conjunto de datos dorado** es un conjunto curado de casos de prueba con salidas esperadas conocidas. Cada caso de prueba incluye:

- **Entrada**: El prompt o pregunta para enviar al agente
- **Salida esperada**: Qué debería contener una respuesta correcta o de alta calidad (palabras clave, estructura, hechos)
- **Categoría**: Agrupación para reportes (ej. "precisión factual", "tono", "integralidad")

### 2. Comprobaciones Basadas en Reglas

Comprobaciones rápidas y deterministas que no requieren un LLM:

| Comprobación | Qué Prueba |
|--------------|------------|
| **Límites de longitud** | La respuesta no es demasiado corta (perezosa) ni demasiado larga (divaga) |
| **Palabras clave requeridas** | La respuesta menciona términos o entidades esperadas |
| **Validación de formato** | El JSON es analizables, presentes los encabezados Markdown |
| **Contenido prohibido** | No hay nombres de marcas inventados, ni menciones a competidores |

### 3. LLM como Juez

Usa el **mismo modelo local** para calificar sus propias salidas (o salidas de una variante diferente). El juez recibe:

- La pregunta original
- La respuesta del agente
- Criterios de calificación

Y devuelve una puntuación estructurada. Esto replica el patrón del Editor de la Parte 7 pero aplicado sistemáticamente a toda la suite de pruebas.

### 4. Bucle de Iteración Guiado por Evaluación

![Bucle de iteración guiado por evaluación](../../../images/eval-loop-flowchart.svg)

---

## Prerrequisitos

| Requisito | Detalles |
|-----------|----------|
| **Foundry Local CLI** | Instalado con un modelo descargado |
| **Entorno de ejecución** | **Python 3.9+** y/o **Node.js 18+** y/o **.NET 9+ SDK** |
| **Completado** | [Parte 5: Agentes Individuales](part5-single-agents.md) y [Parte 6: Flujos de trabajo Multi-Agente](part6-multi-agent-workflows.md) |

---

## Ejercicios del Laboratorio

### Ejercicio 1 - Ejecutar el Marco de Evaluación

El taller incluye una muestra completa de evaluación que prueba un agente Foundry Local contra un conjunto dorado de preguntas relacionadas con Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Preparación:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Ejecución:**
```bash
python foundry-local-eval.py
```

**Qué sucede:**
1. Conecta a Foundry Local y carga el modelo
2. Define un conjunto dorado de 5 casos de prueba (preguntas sobre productos Zava)
3. Ejecuta dos variantes de prompt contra cada caso de prueba
4. Puntúa cada respuesta con **comprobaciones basadas en reglas** (longitud, palabras clave, formato)
5. Puntúa cada respuesta con **LLM como juez** (el mismo modelo califica calidad 1-5)
6. Imprime un reporte comparativo de ambas variantes de prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Preparación:**
```bash
cd javascript
npm install
```

**Ejecución:**
```bash
node foundry-local-eval.mjs
```

**Misma pipeline de evaluación:** conjunto dorado, ejecuciones duales de prompt, puntuación basada en reglas + LLM, reporte comparativo.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Preparación:**
```bash
cd csharp
dotnet restore
```

**Ejecución:**
```bash
dotnet run eval
```

**Misma pipeline de evaluación:** conjunto dorado, ejecuciones duales de prompt, puntuación basada en reglas + LLM, reporte comparativo.

</details>

---

### Ejercicio 2 - Entender el Conjunto Dorado

Examina los casos de prueba definidos en la muestra de evaluación. Cada caso incluye:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Preguntas para considerar:**
1. ¿Por qué los valores esperados son **palabras clave** en lugar de oraciones completas?
2. ¿Cuántos casos de prueba necesitas para una evaluación confiable?
3. ¿Qué categorías agregarías para tu propia aplicación?

---

### Ejercicio 3 - Entender Puntuación Basada en Reglas vs LLM como Juez

El marco de evaluación usa **dos capas de puntuación**:

#### Comprobaciones Basadas en Reglas (rápidas, deterministas)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM como Juez (matizado, cualitativo)

El mismo modelo local actúa como juez con un rubrico estructurado:

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

**Preguntas para considerar:**
1. ¿Cuándo confiarías más en las comprobaciones basadas en reglas que en el LLM como juez?
2. ¿Puede un modelo juzgar confiablemente su propia salida? ¿Cuáles son las limitaciones?
3. ¿Cómo se compara esto con el patrón del agente Editor de la Parte 7?

---

### Ejercicio 4 - Comparar Variantes de Prompt

La muestra ejecuta **dos variantes de prompt** contra los mismos casos:

| Variante | Estilo del Prompt del Sistema |
|----------|-------------------------------|
| **Base** | Genérico: "Eres un asistente útil" |
| **Especializado** | Detallado: "Eres un experto en Zava DIY que recomienda productos específicos y da guía paso a paso" |

Después de ejecutar, verás un reporte como:

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

**Ejercicios:**
1. Ejecuta la evaluación y anota las puntuaciones de cada variante
2. Modifica el prompt especializado para que sea aún más específico. ¿Mejora la puntuación?
3. Añade una tercera variante de prompt y compara las tres.
4. Prueba cambiar el alias del modelo (ej. `phi-4-mini` vs `phi-3.5-mini`) y compara resultados.

---

### Ejercicio 5 - Aplica Evaluación a Tu Propio Agente

Usa el marco de evaluación como plantilla para tus propios agentes:

1. **Define tu conjunto dorado**: escribe 5 a 10 casos con palabras clave esperadas.
2. **Escribe tu prompt del sistema**: las instrucciones para el agente que quieres probar.
3. **Ejecuta la evaluación**: obtén las puntuaciones base.
4. **Itera**: ajusta el prompt, ejecuta de nuevo y compara.
5. **Define un umbral**: ej. "no lanzar por debajo de 0.75 en puntuación combinada".

---

### Ejercicio 6 - Conexión con el Patrón Editor de Zava

Revisa el agente Editor del Escritor Creativo de Zava (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# El editor es un LLM-como-juez en producción:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Este es el **mismo concepto** que el LLM como juez de la Parte 8, pero integrado en el pipeline de producción en lugar de en una suite de pruebas offline. Ambos patrones:

- Usan salida JSON estructurada del modelo.
- Aplican criterios de calidad definidos en el prompt del sistema.
- Tomar una decisión de aprobado/rechazado.

**La diferencia:** El editor se ejecuta en producción (en cada solicitud). El marco de evaluación se ejecuta en desarrollo (antes de lanzar).

---

## Conclusiones Clave

| Concepto | Resumen |
|----------|---------|
| **Conjuntos dorados** | Curar casos de prueba temprano; son tus pruebas de regresión para IA |
| **Comprobaciones basadas en reglas** | Rápidas, deterministas, detectan fallos obvios (longitud, palabras clave, formato) |
| **LLM como juez** | Puntuación de calidad matizada usando el mismo modelo local |
| **Comparación de prompts** | Ejecuta variantes múltiples en la misma suite para elegir la mejor |
| **Ventaja on-device** | Toda la evaluación ocurre localmente: sin costos de API, sin límites, sin que los datos salgan de tu máquina |
| **Evalúa antes de lanzar** | Establece umbrales de calidad y controla lanzamientos mediante puntuaciones |

---

## Próximos Pasos

- **Escalar**: Añade más casos y categorías a tu conjunto dorado.
- **Automatizar**: Integra la evaluación en tu pipeline CI/CD.
- **Jueces avanzados**: Usa un modelo más grande como juez mientras pruebas salidas de un modelo más pequeño.
- **Monitorear con el tiempo**: Guarda resultados de evaluación para comparar versiones de prompts y modelos.

---

## Próximo Laboratorio

Continúa con [Parte 9: Transcripción de Voz con Whisper](part9-whisper-voice-transcription.md) para explorar reconocimiento de voz local usando el Foundry Local SDK.