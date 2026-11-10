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
