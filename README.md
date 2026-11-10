# NeuroSonic Art — тестовое задание (Unity VR Technical Artist)

VR-сцена на Unity 6 + URP + OpenXR/XRI: комната 10×10 м в масштабе 1:1, пять интерактивных предметов с физикой и **пространственным звуком** (Steam Audio), громкость удара зависит от силы столкновения. Окружение собрано из бесплатных публичных ассетов (Poly Haven, ambientCG — CC0).

**Видео:** [NeuroSonic_demo.mp4 — релиз v1.0](https://github.com/h1deotw/neurosonic-vr-spatial-audio/releases/download/v1.0/NeuroSonic_demo.mp4) (1920×1080, 30 fps, 39 с, ≈15 МБ)

**Релиз со всеми ссылками:** https://github.com/h1deotw/neurosonic-vr-spatial-audio/releases/tag/v1.0

---

## Стек

| Что | Версия |
|---|---|
| Unity | **6000.3.28f1** (Unity 6 LTS) |
| Рендер | URP (Universal 3D template) |
| XR | OpenXR + XR Interaction Toolkit |
| Spatializer | Steam Audio **4.8.1** |
| Запись видео | Unity Recorder 5.1.7 |

Пакеты (см. `Packages/manifest.json`): `com.unity.xr.openxr`, `com.unity.xr.interaction.toolkit`, `com.unity.inputsystem`, `com.unity.render-pipelines.universal`, `com.unity.recorder`, `com.unity.textmeshpro`.

---

## Ассеты и лицензии

| Ассет | Источник | Лицензия |
|---|---|---|
| Poly Haven — 14 моделей: `ClassicConsole_01`, `CoffeeTable_01`, `Sofa_01`, `throw_pillows_01`, `wooden_bookshelf_worn`, `potted_plant_01`, `fancy_picture_frame_01`, `classic_laptop`, `baseball_01`, `wooden_bowl_01`, `ceramic_vase_03`, `cleaner_tin_01`, `metal_jug`, `rubber_duck_toy` | https://polyhaven.com/models (в проекте — `Assets/PolyHaven/`) | CC0, https://polyhaven.com/license |
| ambientCG — `WoodFloor051`, `PaintedPlaster017` | https://ambientcg.com (в проекте — `Assets/Materials/RoomTextures/`) | CC0, https://ambientcg.com/license |
| Steam Audio 4.8.1 | https://github.com/ValveSoftware/steam-audio/releases (в проекте — `Assets/Plugins/SteamAudio/`) | Apache 2.0, https://github.com/ValveSoftware/steam-audio/blob/master/LICENSE.md |
| Kenney Impact Sounds | https://kenney.nl/assets/impact-sounds | CC0, см. `Assets/Audio/License_Kenney_ImpactSounds.txt` |
| XR Interaction Toolkit Samples | пакет Unity (Starter Assets) | Unity Companion License |

Материалы сцены (`M_Wood`, `M_Metal`, `M_Plastic`, `M_Ceramic`, `M_Floor`, `M_Wall`) и PBR-материалы моделей собраны в проекте на основе этих текстур.

**Единственная правка стороннего кода:** две строки null-guard в `SteamAudioManager.ShutDown()` (`Assets/Plugins/SteamAudio/`) — иначе плагин кидает исключение при остановке Play Mode. Всё остальное в плагине как в оригинале.

---

## Что сделано / что нет

**Сделано**

- [x] Unity 6 LTS, URP, OpenXR + XRI, VR-риг из официального префаба.
- [x] Комната 10×10 м 1:1, телепорт по игровой зоне.
- [x] Обстановка из бесплатных публичных ассетов (Poly Haven + ambientCG, CC0), 14 моделей и 2 набора PBR-текстур.
- [x] 5 интерактивных предметов, 4 материала, Rigidbody + коллайдеры + массы.
- [x] Baked/mixed освещение, чистая консоль.
- [x] Steam Audio spatializer, HRTF, 3D-источники.
- [x] Свой скрипт `ImpactSound.cs`: громкость от силы удара.
- [x] Разные звуки на материал, 3 варианта на материал.
- [x] Видео 1080p, 39 с, выраженный звук сцены, титры и подписи, чистый кадр.
- [x] README, `.gitignore`, репозиторий без `Library/` и `Temp/`.

**Не сделано (сознательно)**

- [ ] Окклюзия, отражения и запечённые аудио-пробы Steam Audio. Для 10×10 м комнаты и задачи «показать пространственность» хватает HRTF + затухания; полный конвейер (occlusion/reflections/probe bake) — лишняя сложность и время, и я не хочу выдавать недоделанное за готовое. Включить можно за вечер: `Steam Audio Manager → Bake` (probes) + `occlusion/reflections = on` у источников.
- [ ] 60 fps в итоговом файле: финальная сборка шла с 30-кадрового монтажа. Исходные дубли записаны Unity Recorder в 1080p60 и при необходимости пересобираются в 60 fps без потерь.
- [ ] Музыка/эмбиент — в задании «опционально, не должно перебивать звук».

---

## Реальные часы

| Этап | Часы |
|---|---|
| Установка, пакеты, XR-настройка, комната, свет | ~4 |
| Steam Audio, скрипт, звуки, подбор параметров | ~2.5 |
| Обстановка Poly Haven/ambientCG, 17 PBR-материалов, коллайдеры мебели | ~3.5 |
| Запись (3 дубля с Recorder) + монтаж + перепроверки | ~5 |
| README / документация / сдача | ~1.5 |
| **Итого** | **~16.5** |

---

## Трудности

1. **Захват чистого кадра.** OBS давал в кадр обвязку Game View, а WGC-захват свёрнутого окна — чёрный кадр. Решение: Unity Recorder, который пишет ровно картинку Game View.
2. **Громкость vs клиппинг.** `alimiter` в ffmpeg по умолчанию (`asc=1`) нормализует сигнал *вверх* до 0 dBFS — формально «ровно», фактически клиппинг. Нужно `asc=0:level=0`; итог — пик −6.4 dBFS, ноль сэмплов в полной шкале.
3. **Материалы Poly Haven в URP.** Модели отдают `nor_gl` (OpenGL-нормали) и раздельные карты roughness/metallic, а URP-лит ожидает упакованную `metallic/smoothness`. Пришлось собирать `*_mask.png` (metallic в B, smoothness в A) и отдельно разбираться с листвой: RGBA-альбедо, alpha clip 0.5, очередь рендера 2450 — иначе листья либо непрозрачные, либо с чёрной каймой.
4. **Коллайдеры мебели.** Импортированные модели приходят с коллайдерами не по габаритам: у стола, дивана, стеллажа и растения пришлось вручную выставлять размер и центр по мешам, иначе предметы либо висят в воздухе, либо проваливаются.
5. **Steam Audio 4.8.1 и выход из Play Mode** — `NullReferenceException` в `ShutDown()`. Добавлен null-guard из двух строк (единственная правка в стороннем коде, см. выше).
6. **Демонстрация «силы удара» без реального контроллера.** В симуляторе нельзя бросить сильно — пришлось перейти на высоту падения и объяснить это в видео.
7. **Steam Audio — плагин с нативной библиотекой.** Пришлось разбираться, что `.dll` в `Binaries/Windows/x86_64` нужен именно для Windows-редактора, а `.unitypackage` содержит ещё и примеры/демо-сцены, которые в проект тащить не надо.
