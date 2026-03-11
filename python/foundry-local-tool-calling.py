import json
import openai
from foundry_local import FoundryLocalManager

# Tool calling requires a model that supports it.
# Qwen 2.5 models and Phi-4-mini support tool calling in Foundry Local.
alias = "qwen2.5-0.5b"

# Step 1: Start Foundry Local and bootstrap the model
print("Starting Foundry Local service...")
manager = FoundryLocalManager(alias)

client = openai.OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

model_id = manager.get_model_info(alias).id

# Step 2: Define tools the model can call
tools = [
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
                        "description": "The city name, e.g. London",
                    }
                },
                "required": ["city"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "get_population",
            "description": "Get the population of a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {
                        "type": "string",
                        "description": "The city name, e.g. London",
                    }
                },
                "required": ["city"],
            },
        },
    },
]


# Step 3: Simulate tool execution (in a real app these would call APIs)
def execute_tool(name, arguments):
    if name == "get_weather":
        city = arguments.get("city", "Unknown")
        return json.dumps({"city": city, "temperature": "18°C", "condition": "Partly cloudy"})
    elif name == "get_population":
        city = arguments.get("city", "Unknown")
        populations = {"london": "8.8 million", "paris": "2.1 million", "tokyo": "14 million"}
        pop = populations.get(city.lower(), "Unknown")
        return json.dumps({"city": city, "population": pop})
    return json.dumps({"error": "Unknown tool"})


# Step 4: Send a message that should trigger tool calls
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather like in London?"},
]

print("\nUser: What is the weather like in London?\n")

response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

assistant_message = response.choices[0].message

# Step 5: Handle tool calls if the model requests them
if assistant_message.tool_calls:
    print(f"Model requested {len(assistant_message.tool_calls)} tool call(s):")
    messages.append(assistant_message)

    for tool_call in assistant_message.tool_calls:
        fn_name = tool_call.function.name
        fn_args = json.loads(tool_call.function.arguments)
        print(f"  → {fn_name}({fn_args})")

        result = execute_tool(fn_name, fn_args)
        messages.append({
            "role": "tool",
            "tool_call_id": tool_call.id,
            "content": result,
        })

    # Step 6: Send the tool results back for the final answer
    print("\nFinal response:")
    final = client.chat.completions.create(
        model=model_id,
        messages=messages,
        tools=tools,
    )
    print(final.choices[0].message.content)
else:
    # Model answered directly without calling tools
    print("Response:", assistant_message.content)
