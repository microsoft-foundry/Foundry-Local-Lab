# Ficheiros de Áudio de Exemplo para a Parte 7 - Transcrição de Voz Whisper

Estes ficheiros WAV são gerados usando `pyttsx3` (texto para fala Windows SAPI5) e têm como tema os **produtos Zava DIY** do demo Creative Writer.

## Gerar os exemplos

```bash
# A partir da raiz do repositório - requer o .venv com o pyttsx3 instalado
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Ficheiros de exemplo

| Ficheiro | Cenário | Duração |
|----------|---------|---------|
| `zava-customer-inquiry.wav` | Cliente a perguntar sobre a **Zava ProGrip Cordless Drill** - binário, duração da bateria, estojo de transporte | ~15 seg |
| `zava-product-review.wav` | Cliente a avaliar a **Zava UltraSmooth Interior Paint** - cobertura, tempo de secagem, baixo VOC | ~22 seg |
| `zava-support-call.wav` | Chamada de suporte sobre a **Zava TitanLock Tool Chest** - chaves de substituição, forros extra para gavetas | ~20 seg |
| `zava-project-planning.wav` | DIYer a planear um deck de jardim com **Zava EcoBoard Composite Decking** & luzes BrightBeam | ~25 seg |
| `zava-workshop-setup.wav` | Apresentação de uma oficina completa usando **os cinco produtos Zava** | ~32 seg |
| `zava-full-project-walkthrough.wav` | Apresentação extensa da renovação da garagem com **todos os produtos Zava** (para teste com áudio longo, ver [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notas

- Os ficheiros WAV estão **comprometidos** no repositório (listados em `. Para criar novos ficheiros .wav, execute o script acima para regenerar novos scripts ou modifique para criar novos scripts.
- O script usa a voz **Microsoft David** (inglês dos EUA) a 160 palavras por minuto para resultados claros na transcrição.
- Todos os cenários referenciam produtos de [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso**:  
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte oficial. Para informações críticas, recomenda-se tradução profissional feita por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->