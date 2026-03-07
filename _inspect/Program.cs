using System;
using System.Reflection;
using System.Linq;

// Inspect Foundry Local
var dllPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    @".nuget\packages\microsoft.ai.foundry.local\0.8.0.1\lib\net8.0\Microsoft.AI.Foundry.Local.dll");

// Inspect Agents AI OpenAI
var agentsDll = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    @".nuget\packages\microsoft.agents.ai.openai\1.0.0-rc3\lib\net8.0\Microsoft.Agents.AI.OpenAI.dll");

foreach (var dll in new[] { agentsDll })
{
    Console.WriteLine($"\n### {Path.GetFileName(dll)} ###");
    var asm = Assembly.LoadFrom(dll);
    foreach (var t in asm.GetExportedTypes())
    {
        Console.WriteLine($"\n=== {t.FullName} ===");
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            try {
                var parms = string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  {(m.IsStatic ? "static " : "")}{m.ReturnType.Name} {m.Name}({parms})");
            } catch { Console.WriteLine($"  {m.Name} (params-error)"); }
        }
    }
}
