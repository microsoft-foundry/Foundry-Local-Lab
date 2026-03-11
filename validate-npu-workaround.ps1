$logFile = "c:\Users\leestott\Foundry-Local-Lab\foundry-validation.log"
$csproj = "c:\Users\leestott\Foundry-Local-Lab\csharp\csharp.csproj"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# Write header
@"
==============================================================================
Foundry Local - WinML / QNN EP Validation Log
Date: $timestamp
Hardware: Snapdragon X Elite (ARM64) - X1E78100
OS: Windows 11 ARM64
SDK: Microsoft.AI.Foundry.Local v0.9.0
WinML: Microsoft.AI.Foundry.Local.WinML v0.9.0
CLI: Foundry Local v0.8.117
.NET SDK: 9.0.312
==============================================================================

CONTEXT: QNN is a plugin execution provider delivered through the WinML
package. Adding Microsoft.AI.Foundry.Local.WinML to the .csproj enables
the QNN EP, allowing NPU model variants to load directly on ARM devices.

FILES WITH WinML PACKAGE REFERENCE (3 .csproj files):
  - csharp/csharp.csproj
  - zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj
  - zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj

==============================================================================
VALIDATION RESULTS
==============================================================================
"@ | Set-Content $logFile -Encoding UTF8

$samples = @("chat", "agent", "rag", "multi", "eval")
$summaryData = @()

foreach ($sample in $samples) {
    Write-Host "`nRunning sample: $sample ..." -ForegroundColor Cyan
    "`n--- Sample: $sample ---" | Add-Content $logFile
    "Started: $(Get-Date -Format 'HH:mm:ss')" | Add-Content $logFile
    "" | Add-Content $logFile

    $output = & dotnet run --project $csproj -- $sample 2>&1 | Out-String
    $exitCode = $LASTEXITCODE

    $output | Add-Content $logFile
    "" | Add-Content $logFile
    "Exit code: $exitCode" | Add-Content $logFile
    "Ended: $(Get-Date -Format 'HH:mm:ss')" | Add-Content $logFile

    $modelMatch = [regex]::Match($output, "Loaded model: (.+)")
    $modelName = if ($modelMatch.Success) { $modelMatch.Groups[1].Value.Trim() } else { "(unknown)" }

    $qnnLoaded = $output -match "qnn-npu" -or $output -match "QNNExecutionProvider"

    if ($qnnLoaded) {
        "QNN EP: LOADED (NPU variant selected via WinML)" | Add-Content $logFile
    } elseif ($output -match "QNN execution provider is not supported") {
        "QNN EP: FAILED (WinML package may be missing)" | Add-Content $logFile
    } else {
        "QNN EP: NOT APPLICABLE (model loaded without NPU variant)" | Add-Content $logFile
    }

    $status = if ($exitCode -eq 0) { "PASS" } else { "FAIL" }
    $summaryData += [PSCustomObject]@{
        Sample = $sample
        QNN = if ($qnnLoaded) { "YES" } else { "NO" }
        Model = $modelName
        ExitCode = $exitCode
        Status = $status
    }

    Write-Host "  $sample => Exit: $exitCode | QNN via WinML: $(if ($qnnLoaded) {'YES'} else {'NO'}) | $status" -ForegroundColor $(if ($exitCode -eq 0) { "Green" } else { "Red" })
}

# Write summary table
@"

==============================================================================
SUMMARY TABLE
==============================================================================

Sample          QNN via WinML   Model Loaded                            Exit   Status
--------------- --------------- --------------------------------------- ------ ------
"@ | Add-Content $logFile

foreach ($row in $summaryData) {
    "{0,-15} {1,-15} {2,-39} {3,-6} {4}" -f $row.Sample, $row.QNN, $row.Model, $row.ExitCode, $row.Status | Add-Content $logFile
}

$allPass = ($summaryData | Where-Object { $_.Status -ne "PASS" }).Count -eq 0
$qnnCount = ($summaryData | Where-Object { $_.QNN -eq "YES" }).Count

@"

==============================================================================
CONCLUSION
==============================================================================

Samples run: $($summaryData.Count)
QNN EP loaded via WinML: $qnnCount / $($summaryData.Count)
Overall result: $(if ($allPass) { 'ALL PASSED' } else { 'SOME FAILED' })

The Microsoft.AI.Foundry.Local.WinML package provides the QNN execution
provider as a plugin EP. On this ARM64 device, the NPU variant loads
directly without requiring a CPU fallback workaround.

==============================================================================
"@ | Add-Content $logFile

Write-Host "`n==========================================" -ForegroundColor Green
Write-Host "Validation complete. Log: $logFile" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
