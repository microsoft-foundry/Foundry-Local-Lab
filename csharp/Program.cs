// Foundry Local Workshop — C# Examples
// Usage: dotnet run [part]
//   dotnet run           → Part 2: Basic chat completion
//   dotnet run chat      → Part 2: Basic chat completion
//   dotnet run rag       → Part 3: Retrieval-Augmented Generation
//   dotnet run agent     → Part 4: Single agent
//   dotnet run multi     → Part 5: Multi-agent workflow
//   dotnet run eval      → Part 8: Agent evaluation framework
//   dotnet run whisper   → Part 9: Whisper voice transcription

var part = args.Length > 0 ? args[0].ToLowerInvariant() : "chat";

switch (part)
{
    case "chat":
        await Examples.BasicChat.RunAsync();
        break;
    case "rag":
        await Examples.RagPipeline.RunAsync();
        break;
    case "agent":
        await Examples.SingleAgent.RunAsync();
        break;
    case "multi":
        await Examples.MultiAgent.RunAsync();
        break;
    case "eval":
        await Examples.AgentEvaluation.RunAsync();
        break;
    case "whisper":
        await Examples.WhisperTranscription.RunAsync(args);
        break;
    default:
        Console.WriteLine("Usage: dotnet run [chat|rag|agent|multi|eval|whisper]");
        Console.WriteLine("  chat    — Part 2: Basic streaming chat completion");
        Console.WriteLine("  rag     — Part 3: RAG pipeline with local knowledge base");
        Console.WriteLine("  agent   — Part 4: Single AI agent with system instructions");
        Console.WriteLine("  multi   — Part 5: Multi-agent Researcher → Writer → Editor");
        Console.WriteLine("  eval    — Part 8: Agent evaluation framework");
        Console.WriteLine("  whisper — Part 9: Whisper voice transcription");
        break;
}