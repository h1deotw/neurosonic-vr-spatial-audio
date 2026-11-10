# NeuroSonic Art — тестовое задание (Unity VR Technical Artist)

VR-сцена на Unity 6 + URP + OpenXR/XRI: комната 10×10 м в масштабе 1:1, шесть интерактивных предметов с физикой и **пространственным звуком** (Steam Audio), громкость удара зависит от силы столкновения.

**Видео:** _ссылка на релиз появится здесь после публикации_ (`NeuroSonic_demo.mp4`, 1920×1080, 60 fps, 34.6 с)

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

1. Открыть папку проекта в Unity **6000.3.28f1** (Unity Hub → Add → `D:\NeuroSonicTest`).
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
- **6 предметов, 4 материала:** дерево (брусок, ящик), металл (банка, шар), пластик (брусок), керамика (ваза).
  У каждого: `Rigidbody` с реальной массой (лёгкая банка — тяжёлый ящик), `BoxCollider`/`MeshCollider` по форме, `XRGrabInteractable` + `XRGeneralGrabTransformer`. Хватаются, бросаются, сталкиваются друг с другом и с полом/стойкой.
- **Стойка `Environment/Counter`** (2.8 × 1.2 × 0.9 м, `M_Wood`) — предметы лежат на уровне глаз, а не на полу.
- **Свет:** baked/mixed (Directional + запечённые lightmap'ы), тени на полу и стенах.
- **Консоль:** 0 ошибок, 0 предупреждений.

## Часть 2. Звук

- **Steam Audio Spatializer** включён в `Project Settings → Audio → Spatializer Plugin`; на сцене один `SteamAudioManager`. Источники — `AudioSource` в режиме 3D с `directBinaural` (HRTF): слышно направление, а не просто громкость.
- **Скрипт `Assets/Scripts/ImpactSound.cs`** — единственный собственный C# скрипт задания. Идея:
  ```csharp
  var speed = collision.relativeVelocity.magnitude;      // сила удара
  var t     = Mathf.InverseLerp(0.6f, 7f, speed);        // 0…1
  src.volume = Mathf.Lerp(0.15f, 1f, t);                 // тихо → громко
  ```
  Плюс случайный выбор одного из трёх сэмплов материала, случайный питч 0.93–1.07 и кулдаун 0.08 с, чтобы один удар не давал очередь коллизий. Доплер выключен.
- **У каждого материала свой набор из трёх сэмплов** — дерево, металл, пластик, керамика звучат по-разному.
- **Демонстрация силы — высотой падения.** В симуляторе рука ограничена по скорости (~0.2 м/с), поэтому «бросок изо всех сил» физически невозможен. Вместо этого каждый предмет роняется дважды: с рук (~1.35 м) и с поднятой руки (~2.1 м) — на слух разница громкости очевидна.

## Часть 3. Видео

- Запись — **Unity Recorder** (Movie Recorder, Game View, 1920×1080, 60 fps, MP4/H.264, capture audio). Кадр чистый: без редакторского UI, без курсора, без оверлеев симулятора.
- Звук пишется с `AudioListener` сцены, то есть **уже пространственный** (Steam Audio HRTF + затухание), а не «звук системы».
- Монтаж — ffmpeg: титул, подписи по материалам, финальные титры, watermark, fade in/out, выравнивание громкости (`volume` + `alimiter`, пик **−3.1 dBFS**, клиппинга нет).
- Музыки нет: в сцене её нет, и подложка только мешала бы слышать разницу громкости ударов.

---

## Что сделано / что нет

**Сделано**

- [x] Unity 6 LTS, URP, OpenXR + XRI, VR-риг из официального префаба.
- [x] Комната 10×10 м 1:1, телепорт по игровой зоне.
- [x] 6 интерактивных предметов, 4 материала, Rigidbody + коллайдеры + массы.
- [x] Baked/mixed освещение, чистая консоль.
- [x] Steam Audio spatializer, HRTF, 3D-источники.
- [x] Свой скрипт `ImpactSound.cs`: громкость от силы удара.
- [x] Разные звуки на материал, 3 варианта на материал.
- [x] Видео 1080p60, 34.6 с, выраженный звук сцены, титры и подписи.
- [x] README, `.gitignore`, репозиторий без `Library/` и `Temp/`.

**Не сделано (сознательно)**

- [ ] Окклюзия, отражения и запечённые аудио-пробы Steam Audio. Для 10×10 м комнаты и задачи «показать пространственность» хватает HRTF + затухания; полный `SteamAudioSource`-конвейер (occlusion/reflections/probe bake) — лишняя сложность и время, и я не хочу выдавать недоделанное за готовое. Включить можно за один вечер: `Steam Audio Manager → Bake` (probes) + `occlusion/reflections = on` у источников.
- [ ] Музыка/эмбиент — в задании «опционально, не должно перебивать звук».

## Реальные часы

| Этап | Часы |
|---|---|
| Установка, пакеты, XR-настройка, сцена, предметы, свет | ~4 |
| Steam Audio, скрипт, звуки, подбор параметров | ~2.5 |
| Запись (Recorder) + монтаж + проверки | ~3 |
| README / документация / сдача | ~1 |
| **Итого** | **~10.5** |

## Трудности

1. **Захват чистого кадра.** OBS давал в кадр обвязку Game View, а WGC-захват свёрнутого окна — чёрный кадр. Решение: Unity Recorder, который пишет ровно картинку Game View.
2. **Громкость vs клиппинг.** `alimiter` в ffmpeg по умолчанию (`asc=1`) нормализует сигнал *вверх* до 0 dBFS — формально «ровно», фактически клиппинг. Нужно `asc=0:level=0`; итог — пик −3.1 dBFS, ноль сэмплов в полной шкале.
3. **Steam Audio 4.8.1 и выход из Play Mode** — `NullReferenceException` в `ShutDown()`. Добавлен null-guard из двух строк (единственная правка в стороннем коде, см. ниже).
4. **Демонстрация «силы удара» без реального контроллера.** В симуляторе нельзя бросить сильно — пришлось перейти на высоту падения и объяснить это в видео.
5. **Steam Audio — плагин с нативной библиотекой.** Пришлось разбираться, что `.dll` в `Binaries/Windows/x86_64` нужен именно для Windows-редактора, а `.unitypackage` содержит ещё и примеры/демо-сцены, которые в проект тащить не надо.

## Ассеты и лицензии

| Ассет | Источник | Лицензия |
|---|---|---|
| Steam Audio 4.8.1 | https://github.com/ValveSoftware/steam-audio/releases (в проекте — `Assets/Plugins/SteamAudio/`) | Apache 2.0, https://github.com/ValveSoftware/steam-audio/blob/master/LICENSE.md |
| Kenney Impact Sounds | https://kenney.nl/assets/impact-sounds | CC0, см. `Assets/Audio/License_Kenney_ImpactSounds.txt` |
| XR Interaction Toolkit Samples | пакет Unity (Starter Assets) | Unity Companion License |
| Все материалы сцены | созданы в проекте | — |

**Единственная правка стороннего кода:** две строки null-guard в `SteamAudioManager.ShutDown()` (`Assets/Plugins/SteamAudio/`) — иначе плагин кидает исключение при остановке Play Mode. Всё остальное в плагине как в оригинале.

## Структура

```
Assets/
  Scenes/MainScene.unity      — единственная сцена
  Scripts/ImpactSound.cs      — единственный собственный скрипт
  Audio/Impacts/              — 15 сэмплов ударов (5 наборов × 3)
  Materials/                  — 6 материалов
  Settings/                   — URP-ассеты
  Plugins/SteamAudio/         — плагин Steam Audio
  XR/, XRI/, Samples/         — настройки XR и сэмплы XRI
Packages/manifest.json
ProjectSettings/
EXPLAIN.md                    — шпаргалка «простыми словами» к созвону
STEPS.md                      — дневник работы по шагам
```
