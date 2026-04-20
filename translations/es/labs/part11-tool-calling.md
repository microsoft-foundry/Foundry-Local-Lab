![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 11: Llamadas a Herramientas con Modelos Locales

> **Objetivo:** Permitir que tu modelo local llame a funciones externas (herramientas) para que pueda obtener datos en tiempo real, realizar cálculos o interactuar con APIs — todo ejecutándose de forma privada en tu dispositivo.

## ¿Qué es la llamada a herramientas?

La llamada a herramientas (también conocida como **llamada a funciones**) permite que un modelo de lenguaje solicite la ejecución de funciones que defines. En lugar de adivinar una respuesta, el modelo reconoce cuándo una herramienta sería útil y devuelve una solicitud estructurada para que tu código la ejecute. Tu aplicación ejecuta la función, envía el resultado de vuelta, y el modelo incorpora esa información en su respuesta final.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Este patrón es esencial para construir agentes que puedan:

- **Consultar datos en vivo** (clima, precios de acciones, consultas a bases de datos)
- **Realizar cálculos precisos** (matemáticas, conversiones de unidades)
- **Tomar acciones** (enviar correos, crear tickets, actualizar registros)
- **Acceder a sistemas privados** (APIs internas, sistemas de archivos)

---

## Cómo funciona la llamada a herramientas

El flujo de llamada a herramientas tiene cuatro etapas:

| Etapa | Qué sucede |
|-------|-------------|
| **1. Definir herramientas** | Describes las funciones disponibles usando JSON Schema — nombre, descripción y parámetros |
| **2. Modelo decide** | El modelo recibe tu mensaje más las definiciones de las herramientas. Si una herramienta ayuda, devuelve una respuesta `tool_calls` en lugar de una respuesta de texto |
| **3. Ejecutar localmente** | Tu código analiza la llamada a la herramienta, ejecuta la función y recopila el resultado |
| **4. Respuesta final** | Envías el resultado de la herramienta de vuelta al modelo, que produce su respuesta final |

> **Punto clave:** El modelo nunca ejecuta código. Solo *solicita* que se llame a una herramienta. Tu aplicación decide si acepta esa solicitud — esto te mantiene en control total.

---

## ¿Qué modelos soportan llamada a herramientas?

No todos los modelos soportan llamada a herramientas. En el catálogo actual de Foundry Local, los siguientes modelos tienen capacidad de llamada a herramientas:

| Modelo | Tamaño | Llamada a herramienta |
|-------|------|:---:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Consejo:** Para este laboratorio usamos **qwen2.5-0.5b** — es pequeño (822 MB de descarga), rápido y tiene soporte confiable para llamada a herramientas.

---

## Objetivos de aprendizaje

Al final de este laboratorio podrás:

- Explicar el patrón de llamada a herramientas y por qué es importante para agentes IA
- Definir esquemas de herramientas usando el formato de llamada a funciones de OpenAI
- Manejar el flujo de conversación de llamada a herramientas en múltiples turnos
- Ejecutar llamadas a herramientas localmente y devolver resultados al modelo
- Elegir el modelo adecuado para escenarios de llamada a herramientas

---

## Requisitos previos

| Requisito | Detalles |
|-------------|---------|
| **Foundry Local CLI** | Instalado y en tu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK de Python, JavaScript o C# instalado ([Parte 2](part2-foundry-local-sdk.md)) |
| **Un modelo que soporte llamada a herramientas** | qwen2.5-0.5b (se descargará automáticamente) |

---

## Ejercicios

### Ejercicio 1 — Entender el flujo de llamada a herramientas

Antes de escribir código, estudia este diagrama de secuencia:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Observaciones clave:**

1. Defines las herramientas desde el principio como objetos JSON Schema
2. La respuesta del modelo contiene `tool_calls` en lugar de contenido normal
3. Cada llamada a herramienta tiene un `id` único que debes referenciar al devolver resultados
4. El modelo ve todos los mensajes previos *más* los resultados de las herramientas al generar la respuesta final
5. Pueden ocurrir múltiples llamadas a herramientas en una sola respuesta

> **Discusión:** ¿Por qué el modelo devuelve llamadas a herramientas en lugar de ejecutar funciones directamente? ¿Qué ventajas de seguridad proporciona esto?

---

### Ejercicio 2 — Definir esquemas de herramientas

Las herramientas se definen usando el formato estándar de llamada a funciones de OpenAI. Cada herramienta necesita:

- **`type`**: Siempre `"function"`
- **`function.name`**: Un nombre descriptivo para la función (ej. `get_weather`)
- **`function.description`**: Una descripción clara — el modelo usa esto para decidir cuándo llamar a la herramienta
- **`function.parameters`**: Un objeto JSON Schema que describe los argumentos esperados

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Buenas prácticas para descripciones de herramientas:**
> - Sé específico: "Obtener el clima actual para una ciudad dada" es mejor que "Obtener clima"
> - Describe claramente los parámetros: el modelo lee estas descripciones para completar los valores correctos
> - Marca parámetros requeridos vs opcionales — esto ayuda al modelo a decidir qué solicitar

---

### Ejercicio 3 — Ejecutar los ejemplos de llamada a herramientas

Cada muestra de código define dos herramientas (`get_weather` y `get_population`), envía una pregunta que activa el uso de herramientas, ejecuta la herramienta localmente y envía el resultado para obtener una respuesta final.

<details>
<summary><strong>🐍 Python</strong></summary>

**Requisitos previos:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Ejecutar:**
```bash
python foundry-local-tool-calling.py
```

**Salida esperada:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicación del código** (`python/foundry-local-tool-calling.py`):

```python
# Definir herramientas como una lista de esquemas de funciones
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Enviar con herramientas — el modelo puede devolver llamadas a herramientas en lugar de contenido
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Comprobar si el modelo quiere llamar a una herramienta
if response.choices[0].message.tool_calls:
    # Ejecutar la herramienta y enviar el resultado de vuelta
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Requisitos previos:**
```bash
cd javascript
npm install
```

**Ejecutar:**
```bash
node foundry-local-tool-calling.mjs
```

**Salida esperada:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicación del código** (`javascript/foundry-local-tool-calling.mjs`):

Este ejemplo usa el `ChatClient` nativo del SDK de Foundry Local en lugar del SDK de OpenAI, demostrando la conveniencia del método `createChatClient()`:

```javascript
// Obtén un ChatClient directamente del objeto modelo
const chatClient = model.createChatClient();

// Envía con herramientas — ChatClient maneja el formato compatible con OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Verificar llamadas a herramientas
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Ejecutar herramientas y enviar los resultados de vuelta
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Requisitos previos:**
```bash
cd csharp
dotnet restore
```

**Ejecutar:**
```bash
dotnet run toolcall
```

**Salida esperada:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicación del código** (`csharp/ToolCalling.cs`):

C# usa el helper `ChatTool.CreateFunctionTool` para definir herramientas:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Ejercicio 4 — El flujo de conversación de llamada a herramientas

Entender la estructura de los mensajes es crítico. Aquí está el flujo completo, mostrando el arreglo `messages` en cada etapa:

**Etapa 1 — Solicitud inicial:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Etapa 2 — El modelo responde con tool_calls (no contenido):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Etapa 3 — Añades el mensaje del asistente Y el resultado de la herramienta:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Etapa 4 — El modelo produce la respuesta final usando el resultado de la herramienta.**

> **Importante:** El `tool_call_id` en el mensaje de la herramienta debe coincidir con el `id` de la llamada a herramienta. Así el modelo asocia resultados con solicitudes.

---

### Ejercicio 5 — Múltiples llamadas a herramientas

Un modelo puede solicitar varias llamadas a herramientas en una sola respuesta. Prueba cambiar el mensaje del usuario para activar múltiples llamadas:

```python
# En Python — cambia el mensaje del usuario:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// En JavaScript — cambia el mensaje del usuario:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

El modelo debería devolver dos `tool_calls`: uno para `get_weather` y otro para `get_population`. Tu código ya maneja esto porque itera sobre todas las llamadas a herramientas.

> **Pruébalo:** Modifica el mensaje del usuario y ejecuta el ejemplo otra vez. ¿El modelo llama a ambas herramientas?

---

### Ejercicio 6 — Añade tu propia herramienta

Extiende una de las muestras con una nueva herramienta. Por ejemplo, añade una herramienta `get_time`:

1. Define el esquema de la herramienta:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Añade la lógica de ejecución:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # En una aplicación real, usa una biblioteca de zona horaria
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... herramientas existentes ...
```

3. Añade la herramienta al arreglo `tools` y prueba con: "¿Qué hora es en Tokio?"

> **Desafío:** Añade una herramienta que realice un cálculo, como `convert_temperature` que convierta entre Celsius y Fahrenheit. Pruébala con: "Convierte 100°F a Celsius."

---

### Ejercicio 7 — Llamada a herramientas con ChatClient del SDK (JavaScript)

La muestra en JavaScript ya usa el `ChatClient` nativo del SDK en lugar del SDK de OpenAI. Esto es una característica de conveniencia que elimina la necesidad de construir un cliente OpenAI tú mismo:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient se crea directamente desde el objeto modelo
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat acepta herramientas como segundo parámetro
const response = await chatClient.completeChat(messages, tools);
```

Compáralo con el enfoque de Python que usa explícitamente el SDK de OpenAI:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Ambos patrones son válidos. `ChatClient` es más conveniente; el SDK de OpenAI te da acceso al rango completo de parámetros de OpenAI.

> **Pruébalo:** Modifica la muestra de JavaScript para usar el SDK de OpenAI en lugar de `ChatClient`. Necesitarás `import OpenAI from "openai"` y construir el cliente usando el endpoint de `manager.urls[0]`.

---

### Ejercicio 8 — Entendiendo tool_choice

El parámetro `tool_choice` controla si el modelo *debe* usar una herramienta o puede elegir libremente:

| Valor | Comportamiento |
|-------|-----------|
| `"auto"` | El modelo decide si llamar a una herramienta (por defecto) |
| `"none"` | El modelo no llamará a ninguna herramienta, aunque estén disponibles |
| `"required"` | El modelo debe llamar al menos a una herramienta |
| `{"type": "function", "function": {"name": "get_weather"}}` | El modelo debe llamar a la herramienta especificada |

Prueba cada opción en la muestra de Python:

```python
# Forzar al modelo a llamar a get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Nota:** No todas las opciones de `tool_choice` pueden ser soportadas por todos los modelos. Si un modelo no soporta `"required"`, puede ignorar la configuración y comportarse como `"auto"`.

---

## Errores comunes

| Problema | Solución |
|---------|----------|
| El modelo nunca llama a herramientas | Asegúrate de usar un modelo con llamada a herramientas (ej. qwen2.5-0.5b). Revisa la tabla arriba. |
| `tool_call_id` no coincide | Siempre usa el `id` de la respuesta de llamada a herramienta, no un valor codificado |
| El modelo devuelve JSON mal formado en `arguments` | Modelos más pequeños a veces producen JSON inválido. Usa un try/catch para `JSON.parse()` |
| El modelo llama a una herramienta que no existe | Añade un manejador por defecto en tu función `execute_tool` |
| Bucle infinito de llamadas a herramientas | Establece un número máximo de rondas (ej. 5) para evitar bucles interminables |

---

## Puntos clave

1. **La llamada a herramientas** permite a los modelos solicitar ejecución de funciones en lugar de adivinar respuestas
2. El modelo **nunca ejecuta código**; tu aplicación decide qué ejecutar
3. Las herramientas se definen como objetos **JSON Schema** siguiendo el formato de llamada a funciones de OpenAI
4. La conversación usa un patrón **multiturno**: usuario, luego asistente (tool_calls), luego herramienta (resultados), luego asistente (respuesta final)
5. Siempre usa un **modelo que soporte llamada a herramientas** (Qwen 2.5, Phi-4-mini)
6. El método del SDK `createChatClient()` ofrece una forma conveniente de hacer llamadas a herramientas sin construir un cliente OpenAI

---

Continúa a [Parte 12: Construir una interfaz web para el escritor creativo Zava](part12-zava-ui.md) para añadir un front-end basado en navegador al pipeline multiagente con streaming en tiempo real.

---

[← Parte 10: Modelos Personalizados](part10-custom-models.md) | [Parte 12: Interfaz de Zava →](part12-zava-ui.md)