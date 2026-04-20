# 파트 7 샘플 오디오 파일 - Whisper 음성 전사

이 WAV 파일들은 `pyttsx3`(Windows SAPI5 음성 합성)를 사용하여 생성되었으며 Creative Writer 데모의 <strong>Zava DIY 제품</strong>을 주제로 합니다.

## 샘플 생성

```bash
# 저장소 루트에서 - pyttsx3가 설치된 .venv 필요
.venv\Scripts\Activate.ps1          # 윈도우즈
python samples/audio/generate_samples.py
```

## 샘플 파일

| 파일 | 시나리오 | 길이 |
|------|----------|----------|
| `zava-customer-inquiry.wav` | 고객이 <strong>Zava ProGrip 무선 드릴</strong>에 대해 문의함 - 토크, 배터리 수명, 휴대용 케이스 | 약 15초 |
| `zava-product-review.wav` | 고객이 <strong>Zava UltraSmooth 실내 페인트</strong>를 리뷰함 - 커버력, 건조 시간, 저휘발성 유기화합물 | 약 22초 |
| `zava-support-call.wav` | **Zava TitanLock 공구함** 지원 콜 - 교체 키, 추가 서랍 라이너 | 약 20초 |
| `zava-project-planning.wav` | DIY 사용자가 <strong>Zava EcoBoard 복합 데크재</strong>와 BrightBeam 조명으로 뒷마당 데크 계획 | 약 25초 |
| `zava-workshop-setup.wav` | <strong>5가지 Zava 제품 모두</strong>를 활용한 완전한 작업장 구성 안내 | 약 32초 |
| `zava-full-project-walkthrough.wav` | 모든 Zava 제품을 사용한 확장된 차고 리노베이션 안내 (장시간 오디오 테스트용, [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517) 참고) | 약 4분 |

## 참고사항

- WAV 파일들은 레포에 <strong>커밋</strong>되어 있습니다(목록은 `. 에서 확인). 새 .wav 파일 생성 시 위 스크립트를 실행하여 새 스크립트를 다시 생성하거나 수정하세요.
- 스크립트는 명료한 전사 결과를 위해 160 WPM 속도의 **Microsoft David**(미국 영어) 음성을 사용합니다.
- 모든 시나리오는 [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) 에서 참조한 제품을 기반으로 합니다.