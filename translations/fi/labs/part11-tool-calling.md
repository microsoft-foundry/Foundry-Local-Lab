![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 11: Työkalukutsut paikallisten mallien kanssa

> **Tavoite:** Mahdollistaa paikallisen mallisi kutsua ulkoisia funktioita (työkaluja) niin, että se voi hakea reaaliaikaista dataa, suorittaa laskentaa tai kommunikoida rajapintojen kanssa — kaikki tämä ajettuna yksityisesti laitteellasi.

## Mitä on työkalukutsu?

Työkalukutsu (tunnetaan myös nimellä **funktiokutsu**) antaa kielimallille mahdollisuuden pyytää määrittelemiesi funktioiden suorittamista. Malli ei arvaa vastausta, vaan tunnistaa, milloin työkalu auttaisi, ja palauttaa rakenteellisen pyynnön koodillesi suoritettavaksi. Sovelluksesi suorittaa funktion, lähettää tuloksen takaisin ja malli sisällyttää tiedon lopulliseen vastaukseensa.

![Työkalukutsun virtaus](../../../images/tool-calling-flow.svg)

Tämä malli on olennainen agenttien rakentamisessa, jotka voivat:

- **Hakea reaaliaikaista dataa** (sää, osakekurssit, tietokantakyselyt)
- **Tehdä tarkkoja laskelmia** (matematiikka, yksikkömuunnokset)
- **Suorittaa toimintoja** (lähettää sähköposteja, luoda tikettejä, päivittää tietueita)
- **Käyttää yksityisiä järjestelmiä** (sisäiset rajapinnat, tiedostojärjestelmät)

---

## Miten työkalukutsu toimii

Työkalukutsun virtaus sisältää neljä vaihetta:

| Vaihe | Mitä tapahtuu |
|-------|---------------|
| **1. Määritä työkalut** | Kuvaat käytettävissä olevat funktiot JSON-skeemalla — nimi, kuvaus ja parametrit |
| **2. Malli päättää** | Malli saa viestisi ja työkalumääritelmät. Jos työkalu auttaisi, se palauttaa `tool_calls` -vastauksen tekstivastauksen sijaan |
| **3. Suorita paikallisesti** | Koodisi jäsentää työkalukutsun, suorittaa funktion ja kerää tuloksen |
| **4. Lopullinen vastaus** | Lähetät työkalun tuloksen takaisin mallille, joka muodostaa lopullisen vastauksen |

> **Tärkeää:** Malli ei koskaan suorita koodia. Se ainoastaan *pyytää* työkalun kutsua. Sovelluksesi päättää, suorittaako pyynnön — näin pysyt kokonaan hallinnassa.

---

## Mitkä mallit tukevat työkalukutsuja?

Kaikki mallit eivät tue työkalukutsuja. Nykyisessä Foundry Local -katalogissa seuraavat mallit tukevat työkalukutsuja:

| Malli | Koko | Työkalukutsu |
|-------|------|:------------:|
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

> **Vinkki:** Tätä harjoitusta varten käytämme **qwen2.5-0.5b** — se on pieni (822 Mt lataus), nopea ja sen työkalukutsujen tuki on luotettava.

---

## Oppimistavoitteet

Harjoituksen lopuksi osaat:

- Selittää työkalukutsun mallin ja sen merkityksen tekoälyagenttien kontekstissa
- Määritellä työkalujen skeemat OpenAI:n funktiokutsumuodon avulla
- Hallita moniaskelista työkalukutsukeskustelua
- Suorittaa työkalukutsuja paikallisesti ja palauttaa tulokset mallille
- Valita oikean mallin työkalukutsutilanteisiin

---

## Esivaatimukset

| Vaade | Tiedot |
|-------------|---------|
| **Foundry Local CLI** | Asennettuna ja polulla (`PATH`) ([Osa 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python-, JavaScript- tai C#-SDK asennettuna ([Osa 2](part2-foundry-local-sdk.md)) |
| **Työkalukutsumalli** | qwen2.5-0.5b (ladataan automaattisesti) |

---

## Harjoitukset

### Harjoitus 1 — Ymmärrä työkalukutsun virtaus

Ennen koodin kirjoittamista tutki tätä sekvenssikaaviota:

![Työkalukutsun sekvenssikaavio](../../../images/tool-calling-sequence.svg)

**Tärkeimmät huomiot:**

1. Määrität työkalut etukäteen JSON-skeematoteutuksina
2. Mallin vastaus sisältää `tool_calls`, ei tavallista sisältöä
3. Jokaisella työkalukutsulla on ainutlaatuinen `id`, johon on viitattava tuloksia palautettaessa
4. Malli näkee kaikki aiemmat viestit *sekä* työkalun tulokset muodostaessaan loppuvastausta
5. Vastauksessa voi olla useita työkalukutsuja

> **Keskustelu:** Miksi malli palauttaa työkalukutsuja sen sijaan, että suorittaisi funktioita suoraan? Mitä turvallisuusetuja tällä on?

---

### Harjoitus 2 — Työkaluskeemojen määrittely

Työkalut määritellään avoimen OpenAI:n funktiokutsumuodon mukaisesti. Jokaisella työkalulla on:

- **`type`**: Aina `"function"`
- **`function.name`**: Kuvaava funktioiden nimi (esim. `get_weather`)
- **`function.description`**: Selkeä kuvaus — malli käyttää tätä päättäessään työkalun kutsumisesta
- **`function.parameters`**: JSON-skeema, joka kuvaa odotetut argumentit

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

> **Parhaat käytännöt työkalukuvauksissa:**
> - Ole tarkka: "Hae nykyinen sää tietylle kaupungille" on parempi kuin "Hae sää"
> - Kuvaa parametrit selkeästi: malli lukee kuvauksia täyttääkseen oikeat arvot
> - Merkitse pakolliset ja valinnaiset parametrit — tämä auttaa mallia päättämään, mitä kysyä

---

### Harjoitus 3 — Suorita työkalukutsuesimerkit

Jokaisessa kieliesimerkissä määritellään kaksi työkalua (`get_weather` ja `get_population`), lähetetään kysymys, joka laukaisee työkalun käytön, suoritetaan työkalu paikallisesti ja lähetetään tulos lopullista vastausta varten.

<details>
<summary><strong>🐍 Python</strong></summary>

**Esivaatimukset:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Suorita:**
```bash
python foundry-local-tool-calling.py
```

**Odotettu tulos:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodin selitys** (`python/foundry-local-tool-calling.py`):

```python
# Määrittele työkalut funktioskeemojen listana
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

# Lähetä työkalujen kanssa — malli saattaa palauttaa tool_callseja sisällön sijaan
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Tarkista, haluaako malli kutsua työkalua
if response.choices[0].message.tool_calls:
    # Suorita työkalu ja lähetä tulos takaisin
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Esivaatimukset:**
```bash
cd javascript
npm install
```

**Suorita:**
```bash
node foundry-local-tool-calling.mjs
```

**Odotettu tulos:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodin selitys** (`javascript/foundry-local-tool-calling.mjs`):

Tämä esimerkki käyttää Foundry Local SDK:n natiivia `ChatClient`-luokkaa OpenAI SDK:n sijaan, demonstroiden kätevää `createChatClient()`-metodia:

```javascript
// Hanki ChatClient suoraan mallin objektista
const chatClient = model.createChatClient();

// Lähetä työkaluilla — ChatClient käsittelee OpenAI-yhteensopivan muodon
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Tarkista työkalupyyntöjä
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Suorita työkalut ja lähetä tulokset takaisin
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Esivaatimukset:**
```bash
cd csharp
dotnet restore
```

**Suorita:**
```bash
dotnet run toolcall
```

**Odotettu tulos:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodin selitys** (`csharp/ToolCalling.cs`):

C# käyttää `ChatTool.CreateFunctionTool` -apumetodia työkalujen määrittelyssä:

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

### Harjoitus 4 — Työkalukutsukeskustelun virtaus

Viestirakenteen ymmärtäminen on ratkaisevan tärkeää. Tässä koko virtaus `messages`-taulukon muodossa kussakin vaiheessa:

**Vaihe 1 — Alkuperäinen pyyntö:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Vaihe 2 — Malli vastaa `tool_calls`:lla (ei sisältöä):**
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

**Vaihe 3 — Lisäät avustajan viestin JA työkalun tuloksen:**
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

**Vaihe 4 — Malli antaa lopullisen vastauksen käyttäen työkalun tulosta.**

> **Tärkeää:** `tool_call_id` työkaluvastauksessa tulee vastata työkalukutsun `id`:tä. Näin malli yhdistää tulokset pyyntöihin.

---

### Harjoitus 5 — Useat työkalukutsut

Malli voi pyytää useita työkalukutsuja yhdessä vastauksessa. Kokeile muuttaa käyttäjäviestiä laukaistaksesi monta kutsua:

```python
# Pythonissa — muuta käyttäjän viesti:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScriptissä — muuta käyttäjän viesti:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Mallin pitäisi palauttaa kaksi `tool_calls`-kutsua — yksi `get_weather` ja yksi `get_population`. Koodisi käsittelee tämän jo, koska se käy kaikki kutsut läpi silmukassa.

> **Kokeile:** Muuta käyttäjäviestiä ja suorita esimerkki uudelleen. Kutsutaanko molempia työkaluja?

---

### Harjoitus 6 — Lisää oma työkalu

Laajenna jotakin esimerkeistä uudella työkalulla. Esimerkiksi lisää `get_time`-työkalu:

1. Määrittele työkalun skeema:
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

2. Lisää suorituslogiikka:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Todellisessa sovelluksessa käytä aikavyöhyke-kirjastoa
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... olemassa olevat työkalut ...
```

3. Lisää työkalu `tools`-taulukkoon ja testaa: "Paljonko kello on Tokiossa?"

> **Haaste:** Lisää työkalu, joka suorittaa laskentaa, esim. `convert_temperature`, joka muuntaa Celsius- ja Fahrenheit-asteiden välillä. Testaa: "Muunna 100°F Celsiukseksi."

---

### Harjoitus 7 — Työkalukutsut SDK:n ChatClientilla (JavaScript)

JavaScript-esimerkki käyttää jo SDK:n natiivin `ChatClient`-luokan sijaan OpenAI SDK:ta. Tämä on kätevä ominaisuus, joka poistaa tarpeen rakentaa OpenAI-asiakasta itse:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient luodaan suoraan mallin objektista
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat vastaanottaa työkalut toisena parametrina
const response = await chatClient.completeChat(messages, tools);
```

Vertaa tätä Pythonin tapaan, joka käyttää eksplisiittisesti OpenAI SDK:ta:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Molemmat mallit ovat päteviä. `ChatClient` on kätevämpi; OpenAI SDK antaa täydet parametrioptioasetukset.

> **Kokeile:** Muokkaa JavaScript-esimerkkiasi käyttämään OpenAI SDK:ta `ChatClient`:n sijaan. Tarvitset `import OpenAI from "openai"` ja asiakaskohtaisen konstruktion `manager.urls[0]` -päätepisteellä.

---

### Harjoitus 8 — Tool_choice-parametrin ymmärtäminen

`tool_choice`-parametri määrää, onko mallin *pakko* käyttää työkalua vai voiko se valita vapaasti:

| Arvo | Käyttäytyminen |
|-------|----------------|
| `"auto"` | Malli päättää, kutsutaanko työkalua (oletus) |
| `"none"` | Malli ei kutsu yhtään työkalua, vaikka ne olisivat saatavilla |
| `"required"` | Mallin on pakko kutsua vähintään yksi työkalu |
| `{"type": "function", "function": {"name": "get_weather"}}` | Mallin on pakko kutsua määriteltyä työkalua |

Kokeile jokaista vaihtoehtoa Python-esimerkissä:

```python
# Pakota malli kutsumaan get_weather-funktiota
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Huom:** Kaikki `tool_choice`-vaihtoehdot eivät välttämättä ole tuettuja kaikissa malleissa. Jos malli ei tue `"required"`-asetusta, se voi ohittaa sen ja käyttäytyä kuin `"auto"`.

---

## Yleisiä sudenkuoppia

| Ongelma | Ratkaisu |
|---------|----------|
| Malli ei koskaan kutsu työkaluja | Varmista, että käytät työkalukutsua tukevaa mallia (esim. qwen2.5-0.5b). Tarkista yllä oleva taulukko. |
| `tool_call_id` ei täsmää | Käytä aina työkalukutsuvastauksesta saatua `id`:tä, älä kovakoodattua arvoa |
| Malli palauttaa virheellistä JSON:ia `arguments`-kentässä | Pienemmät mallit saattavat tuottaa virheellistä JSON:ia. Kiedo `JSON.parse()` try/catch -lohkoon |
| Malli kutsuu työkalua, jota ei ole olemassa | Lisää oletuskäsittelijä `execute_tool`-funktioon |
| Ääretön työkalukutsusilmukka | Aseta maksimikierrosmäärä (esim. 5) estämään hallitsematon silmukka |

---

## Tärkeitä oppeja

1. **Työkalukutsu** antaa malleille mahdollisuuden pyytää funktioiden suoritusta arvaamisen sijaan
2. Malli **ei koskaan suorita koodia**; sovelluksesi päättää, mitä ajetaan
3. Työkalut määritellään **JSON-skeemoina** OpenAI:n funktiokutsumuodon mukaisesti
4. Keskustelu tapahtuu **moniaskelisena mallina**: käyttäjä, sitten avustaja (tool_calls), sitten työkalu (tulokset), sitten avustaja (lopullinen vastaus)
5. Käytä aina **mallia, joka tukee työkalukutsuja** (Qwen 2.5, Phi-4-mini)
6. SDK:n `createChatClient()` tarjoaa kätevän tavan tehdä työkalukutsuja rakentamatta OpenAI-asiakasta itse

---

Jatka [Osa 12: Web-käyttöliittymän rakentaminen Zava Creative Writerille](part12-zava-ui.md) lisätäksesi selainpohjaisen käyttöliittymän moniagenttijonoon reaaliaikaisella suoratoistolla.

---

[← Osa 10: Mukautetut mallit](part10-custom-models.md) | [Osa 12: Zava Writer UI →](part12-zava-ui.md)