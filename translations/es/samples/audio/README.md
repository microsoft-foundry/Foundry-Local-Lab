# Archivos de Audio de Muestra para la Parte 7 - Transcripción de Voz Whisper

Estos archivos WAV se generan usando `pyttsx3` (síntesis de voz SAPI5 de Windows) y están temáticos alrededor de los **productos Zava DIY** del demo Creative Writer.

## Generar las muestras

```bash
# Desde la raíz del repositorio - requiere el .venv con pyttsx3 instalado
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Archivos de muestra

| Archivo | Escenario | Duración |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Cliente preguntando sobre el **Taladro inalámbrico Zava ProGrip** - torque, duración de batería, estuche de transporte | ~15 seg |
| `zava-product-review.wav` | Cliente reseñando la **Pintura interior Zava UltraSmooth** - cobertura, tiempo de secado, bajo COV | ~22 seg |
| `zava-support-call.wav` | Llamada de soporte sobre el **Baúl de herramientas Zava TitanLock** - llaves de reemplazo, forros de cajón extras | ~20 seg |
| `zava-project-planning.wav` | Aficionado al bricolaje planificando una terraza trasera con **tarima compuesta Zava EcoBoard** y luces BrightBeam | ~25 seg |
| `zava-workshop-setup.wav` | Recorrido por un taller completo usando **los cinco productos Zava** | ~32 seg |
| `zava-full-project-walkthrough.wav` | Recorrido extendido de renovación de garaje usando **todos los productos Zava** (para prueba de audio largo, ver [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notas

- Los archivos WAV están **comprometidos** en el repositorio (listados en `. Para crear nuevos archivos .wav ejecute el script arriba para regenerar nuevos guiones o modifique para crear nuevos guiones.
- El script usa la voz de **Microsoft David** (inglés estadounidense) a 160 PPM para resultados de transcripción claros.
- Todos los escenarios hacen referencia a productos del archivo [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).