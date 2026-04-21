![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء 13: انتهاء الورشة

> **تهانينا!** لقد أكملت ورشة Foundry Local كاملة.

لقد انتقلت من تثبيت CLI إلى بناء تطبيقات الدردشة، وسير عمل RAG، وأنظمة الوكلاء المتعددين، وتحويل الكلام إلى نص، وتجميع نماذجك المخصصة، وتمكين استدعاء الأدوات، وربط واجهة مستخدم على الويب مباشرة لتطبيقك متعدد الوكلاء. كل ذلك يعمل بالكامل على جهازك.

---

## ما الذي بنيته

| الجزء | ما الذي بنيته |
|------|--------------|
| 1 | تثبيت Foundry Local، استكشاف النماذج عبر CLI |
| 2 | إتقان واجهة SDK الخاصة بـ Foundry Local لإدارة الخدمة، الكتالوج، الذاكرة المؤقتة، والنماذج |
| 3 | الاتصال من Python وJavaScript وC# باستخدام SDK مع OpenAI |
| 4 | بناء سير عمل RAG مع استرجاع المعرفة محليًا |
| 5 | إنشاء وكلاء ذكاء اصطناعي بشخصيات ومخرجات منظّمة |
| 6 | تنسيق سير عمل متعدد الوكلاء مع حلقات تغذية راجعة |
| 7 | استكشاف تطبيق مختتم للإنتاج، كاتب زافا الإبداعي |
| 8 | بناء سير عمل تطوير موجه بالتقييم للوكلاء |
| 9 | نسخ الصوت إلى نص باستخدام Whisper، تحويل الكلام إلى نص على الجهاز |
| 10 | تجميع وتشغيل نموذج Hugging Face مخصص باستخدام ONNX Runtime GenAI |
| 11 | تمكين النماذج المحلية من استدعاء دوال خارجية مع استدعاء الأدوات |
| 12 | بناء واجهة مستخدم متصفح لكاتب زافا الإبداعي مع بث مباشر في الوقت الحقيقي |

---

## أفكار إضافية

- دمج استدعاء الأدوات مع الوكلاء لبناء سير عمل مستقل
- الاستعلام من قاعدة بيانات محلية أو استدعاء REST APIs داخلية من أدواتك
- تجربة نماذج مختلفة (`phi-4-mini`, `deepseek-r1-7b`) للمقارنة بين الجودة والسرعة
- إضافة مصادقة أو تحديد معدل إلى خادم الويب الخاص بكاتب زافا
- إنشاء تطبيق متعدد الوكلاء خاص بك لمجال تهتم به
- النشر على السحابة من خلال استبدال Foundry Local بـ Azure AI Foundry، نفس الكود، نقطة نهاية مختلفة

---

## الموارد

| المورد | الرابط |
|--------|--------|
| موقع Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| كتالوج النماذج | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local على GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| دليل البدء | [Microsoft Learn: Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| مرجع SDK Foundry Local | [Microsoft Learn: SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| إطار عمل Microsoft Agent | [Microsoft Learn: Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

ارجع إلى [نظرة عامة على الورشة](../README.md) لمراجعة ما قمت بتغطيته واستكشاف موارد القراءة الإضافية.

---

[← الجزء 12: بناء واجهة مستخدم على الويب لكاتب زافا الإبداعي](part12-zava-ui.md) | [العودة إلى الصفحة الرئيسية للورشة](../README.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء مسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر الموثوق. للحصول على معلومات هامة، يُنصح بترجمة بشرية محترفة. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->