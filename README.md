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

## Как открыть и запустить

1. Открыть папку проекта в Unity **6000.3.28f1** (Unity Hub → Add → папка проекта).
2. При первом открытии: `Window → TextMeshPro → Import TMP Essential Resources` (штатное действие для шаблона TMP, консоль после него чистая).
3. Открыть сцену `Assets/Scenes/MainScene.unity` (она же единственная в Build Settings).
4. Play. Управление — **XR Device Simulator** (без шлема):
   - `WASD` + мышь — движение и обзор,
   - `G` — взять/отпустить предмет,
   - `Left Shift` + движение — телепорт по полу,
   - колесо/`Q`/`E` — поворот головы, `Tab` — панель симулятора.

Реального шлема не требуется; при подключённом OpenXR-устройстве сцена работает как обычная VR-сцена.

---

## Часть 1. Сцена

- **Комната 10×10 м, 1:1.** Пол — верхняя грань ровно на `y = 0`, четыре стены, потолка нет (свет сверху). Всё в метрах, масштаб не «подгонялся».
- **Телепорт** по всей игровой зоне: `TeleportationArea` на полу, `Teleport Interactor` на контроллере из официального префаба.
- **Обстановка — фотограмметрические модели Poly Haven (CC0):** диван и подушки, кофейный столик (на нём раскрытый ноутбук и бейсбольный мяч), деревянный стеллаж, растение в горшке, картина в раме на стене, консольный стол `ClassicConsole_01` (топ на `y = 0.949`) — он же «сцена» для интерактивных предметов. Пол и стены — PBR-текстуры ambientCG (CC0).
- **5 интерактивных предметов, 4 материала.** У каждого: `Rigidbody` с реальной массой, коллайдер по форме (`BoxCollider`/выпуклый `MeshCollider`), `XRGrabInteractable` + `XRGeneralGrabTransformer`. Хватаются, бросаются, сталкиваются друг с другом, с полом и с мебелью.

| Предмет | Материал | Масса | Звук удара |
|---|---|---|---|
| `Prop_Bowl_Wood` — деревянная чаша | дерево | 0.60 кг | `impactWood_medium` ×3 |
| `Prop_Vase_Ceramic` — керамическая ваза | керамика | 1.20 кг | `impactGlass_medium` ×3 |
| `Prop_Tin_Metal` — жестяная банка | металл (лёгкий) | 0.35 кг | `impactTin_medium` ×3 |
| `Prop_Jug_Metal` — металлический кувшин | металл (тяжёлый) | 0.90 кг | `impactMetal_heavy` ×3 |
| `Prop_Duck_Plastic` — резиновая уточка | пластик | 0.15 кг | `impactPlate_medium` ×3 |

- **Свет:** mixed/baked (Directional + запечённые lightmap'ы), тени на полу и стенах, мягкий заполняющий свет. Статичная геометрия помечена как `Contribute GI`.
- **Консоль при запекании и в Play Mode:** 0 ошибок, 0 предупреждений.

## Часть 2. Звук

- **Steam Audio Spatializer** включён в `Project Settings → Audio → Spatializer Plugin`; на сцене один `SteamAudioManager`. Источники — `AudioSource` в режиме 3D с `directBinaural` (HRTF): слышно направление, а не просто громкость.
- **Скрипт `Assets/Scripts/ImpactSound.cs`** — единственный собственный C# скрипт задания. Идея:
  ```csharp
  var speed = collision.relativeVelocity.magnitude;      // сила удара
  var t     = Mathf.InverseLerp(0.6f, 7f, speed);        // 0…1
  src.volume = Mathf.Lerp(0.15f, 1f, t);                 // тихо → громко
  ```
  Плюс случайный выбор одного из трёх сэмплов материала, случайный питч 0.93–1.07 и кулдаун 0.08 с, чтобы один удар не давал очередь коллизий. Доплер выключен.
- **У каждого материала свой набор из трёх сэмплов** — дерево, керамика, лёгкий металл, тяжёлый металл и пластик звучат по-разному (всего 15 сэмплов, в проекте лежит и запасной набор).
- **Демонстрация силы — высотой падения.** В симуляторе рука ограничена по скорости (~0.2 м/с), поэтому «бросок изо всех сил» физически невозможен. Вместо этого каждый предмет роняется на столешницу дважды: с ~0.16 м и с ~0.80–0.85 м над ней. Разница громкости в записанной дорожке — 12–19 дБ, на слух она очевидна.

## Часть 3. Видео

- Запись — **Unity Recorder** (Movie Recorder, Game View, 1920×1080, 60 fps, MP4/H.264, capture audio), три дубля: посценный обзор предметов, фиксированный кадр со стола и проход по комнате.
- Звук пишется с `AudioListener` сцены, то есть **уже пространственный** (Steam Audio HRTF + затухание), а не «звук системы».
- Монтаж — ffmpeg: титул, подписи по материалам, watermark, fade in/out, выравнивание громкости по сегментам (`volume` + `alimiter`, пик **−6.4 dBFS**, клиппинга нет).
- Итог: **1920×1080, 30 fps, 39 с**. 30 fps — потому что финальная сборка шла с 30-кадрового монтажа; исходные дубли записаны в 60 fps. Требование «1080p» выполнено полностью, «60 fps — по возможности»: сцену можно пересобрать в 60 fps из исходных дублей без потерь.
- Музыки нет: в сцене её нет, и подложка только мешала бы слышать разницу громкости ударов.

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
- [ ] 60 fps в итоговом файле — см. выше, монтаж 30-кадровый.
- [ ] Музыка/эмбиент — в задании «опционально, не должно перебивать звук».

## Реальные часы

| Этап | Часы |
|---|---|
| Установка, пакеты, XR-настройка, комната, свет | ~4 |
| Steam Audio, скрипт, звуки, подбор параметров | ~2.5 |
| Обстановка Poly Haven/ambientCG, 17 PBR-материалов, коллайдеры мебели | ~3.5 |
| Запись (3 дубля с Recorder) + монтаж + перепроверки | ~5 |
| README / документация / сдача | ~1.5 |
| **Итого** | **~16.5** |

## Трудности

1. **Захват чистого кадра.** OBS давал в кадр обвязку Game View, а WGC-захват свёрнутого окна — чёрный кадр. Решение: Unity Recorder, который пишет ровно картинку Game View.
2. **Громкость vs клиппинг.** `alimiter` в ffmpeg по умолчанию (`asc=1`) нормализует сигнал *вверх* до 0 dBFS — формально «ровно», фактически клиппинг. Нужно `asc=0:level=0`; итог — пик −6.4 dBFS, ноль сэмплов в полной шкале.
3. **Материалы Poly Haven в URP.** Модели отдают `nor_gl` (OpenGL-нормали) и раздельные карты roughness/metallic, а URP-лит ожидает упакованную `metallic/smoothness`. Пришлось собирать `*_mask.png` (metallic в B, smoothness в A) и отдельно разбираться с листвой: RGBA-альбедо, alpha clip 0.5, очередь рендера 2450 — иначе листья либо непрозрачные, либо с чёрной каймой.
4. **Коллайдеры мебели.** Импортированные модели приходят с коллайдерами не по габаритам: у стола, дивана, стеллажа и растения пришлось вручную выставлять размер и центр по мешам, иначе предметы либо висят в воздухе, либо проваливаются.
5. **Steam Audio 4.8.1 и выход из Play Mode** — `NullReferenceException` в `ShutDown()`. Добавлен null-guard из двух строк (единственная правка в стороннем коде, см. ниже).
6. **Демонстрация «силы удара» без реального контроллера.** В симуляторе нельзя бросить сильно — пришлось перейти на высоту падения и объяснить это в видео.
7. **Steam Audio — плагин с нативной библиотекой.** Пришлось разбираться, что `.dll` в `Binaries/Windows/x86_64` нужен именно для Windows-редактора, а `.unitypackage` содержит ещё и примеры/демо-сцены, которые в проект тащить не надо.

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

## Структура

```
Assets/
  Scenes/MainScene.unity      — единственная сцена
  Scripts/ImpactSound.cs      — единственный собственный скрипт
  Audio/Impacts/              — 18 сэмплов ударов (6 наборов × 3)
  PolyHaven/                  — 14 моделей с текстурами и 17 PBR-материалов
  Materials/                  — материалы комнаты и предметов, RoomTextures/
  Settings/                   — URP-ассеты
  Plugins/SteamAudio/         — плагин Steam Audio
  XR/, XRI/, Samples/         — настройки XR и сэмплы XRI
Packages/manifest.json
ProjectSettings/
```
