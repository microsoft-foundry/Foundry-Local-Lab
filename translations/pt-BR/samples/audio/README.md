# Arquivos de Áudio de Exemplo para a Parte 7 - Transcrição de Voz Whisper

Esses arquivos WAV foram gerados usando `pyttsx3` (texto para fala SAPI5 do Windows) e têm como tema os **produtos Zava DIY** do demo Creative Writer.

## Gerar os exemplos

```bash
# A partir da raiz do repositório - requer o .venv com pyttsx3 instalado
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Arquivos de exemplo

| Arquivo | Cenário | Duração |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Cliente perguntando sobre a **Furadeira Sem Fio Zava ProGrip** - torque, duração da bateria, estojo de transporte | ~15 seg |
| `zava-product-review.wav` | Cliente avaliando a **Tinta Interior Zava UltraSmooth** - cobertura, tempo de secagem, baixo VOC | ~22 seg |
| `zava-support-call.wav` | Chamada de suporte sobre o **Baú de Ferramentas Zava TitanLock** - chaves de reposição, forros extras para gavetas | ~20 seg |
| `zava-project-planning.wav` | Amador fazendo planos para um deck no quintal com **Deck Composto Zava EcoBoard** e luzes BrightBeam | ~25 seg |
| `zava-workshop-setup.wav` | Apresentação de uma oficina completa usando **todos os cinco produtos Zava** | ~32 seg |
| `zava-full-project-walkthrough.wav` | Apresentação estendida da reforma da garagem usando **todos os produtos Zava** (para teste com áudio longo, veja [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notas

- Os arquivos WAV estão **comprometidos** no repositório (listados em `. Para criar novos arquivos .wav, execute o script acima para regenerar novos scripts ou modifique para criar novos scripts.
- O script usa a voz **Microsoft David** (inglês americano) em 160 PPM para resultados claros de transcrição.
- Todos os cenários fazem referência a produtos de [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido usando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, é recomendada a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->