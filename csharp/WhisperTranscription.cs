using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

namespace Examples;

/// <summary>
/// Part 9: Whisper voice transcription with Foundry Local.
/// Transcribes WAV audio files using the OpenAI Whisper model running locally.
/// Uses the Foundry Local SDK's built-in audio client.
/// </summary>
public static class WhisperTranscription
{
    public static async Task RunAsync(string[] args)
    {
        var alias = "whisper-medium";
        var samplesDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "samples", "audio");

        // Determine which files to transcribe
        string[] audioFiles;
        if (args.Length > 1)
        {
            // A specific file was passed after the "whisper" command
            var filePath = args[1];
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Audio file not found: {filePath}");
                Console.WriteLine("Usage: dotnet run whisper <path-to-wav-file>");
                return;
            }
            audioFiles = [filePath];
        }
        else
        {
            // Default: transcribe all Zava sample WAV files
            if (!Directory.Exists(samplesDir))
            {
                Console.WriteLine($"Samples directory not found: {samplesDir}");
                Console.WriteLine("Run 'python samples/audio/generate_samples.py' first.");
                return;
            }
            audioFiles = Directory.GetFiles(samplesDir, "zava-*.wav")
                .OrderBy(f => f).ToArray();

            if (audioFiles.Length == 0)
            {
                Console.WriteLine($"No WAV files found in {samplesDir}");
                Console.WriteLine("Run 'python samples/audio/generate_samples.py' first.");
                return;
            }
        }

        // Step 1: Start the Foundry Local service
        Console.WriteLine("Starting Foundry Local service...");
        await FoundryLocalManager.CreateAsync(
            new Configuration
            {
                AppName = "FoundryLocalSamples",
                Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
            }, NullLogger.Instance, default);
        var manager = FoundryLocalManager.Instance;
        await manager.StartWebServiceAsync(default);

        // Step 2: Get the model from the catalog
        var catalog = await manager.GetCatalogAsync(default);
        var model = await catalog.GetModelAsync(alias, default);

        // Step 3: Check if the model is already downloaded
        var isCached = await model.IsCachedAsync(default);

        if (isCached)
        {
            Console.WriteLine($"Model already downloaded: {alias}");
        }
        else
        {
            Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
            await model.DownloadAsync(null, default);
            Console.WriteLine($"Download complete: {alias}");
        }

        // Step 4: Load the model into memory
        Console.WriteLine($"Loading model: {alias}...");
        await model.LoadAsync(default);
        Console.WriteLine($"Loaded model: {model.Id}\n");

        // Step 5: Get the audio client from the Foundry Local SDK
        var audioClient = await model.GetAudioClientAsync();

        // Step 6: Transcribe each audio file
        foreach (var audioPath in audioFiles)
        {
            var filename = Path.GetFileName(audioPath);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"File: {filename}");
            Console.WriteLine(new string('=', 60));

            var result = await audioClient.TranscribeAudioAsync(audioPath);
            Console.WriteLine(result.Text);
            Console.WriteLine();
        }

        Console.WriteLine($"Done — transcribed {audioFiles.Length} file(s).");
    }
}
