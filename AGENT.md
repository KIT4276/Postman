# AGENT.md

## Project Overview

- Project: `Postman`
- Engine: Unity
- Platform in README: Android
- Current repo also contains a Windows build in `B/`
- Current publishing target in project settings: WebGL/Yandex Games
- Core stack: Zenject, Unity UI, AI Navigation, SimpleInput, custom Yandex Games SDK bridge

Game premise: the player delivers parcels during a zombie outbreak, manages health/infection, fights enemies, collects aid kits, and uses save points.

## Repository Map

- `Assets/Code` - main gameplay/runtime code
- `Assets/Scenes` - scenes in build settings: `Initial`, `Main`
- `Assets/Resources/Prefabs` - runtime-loaded prefabs via `Resources.Load`
- `Assets/ScriptableObjects` - static gameplay data
- `Assets/Plugins/Zenject` - DI framework plus large optional/sample content
- `Assets/TextMesh Pro` - TMP package content, includes examples/extras
- `Assets/FromStore`, `Assets/JMO Assets` - third-party art/VFX packs
- `Assets/WebGLTemplates/Yandex` - custom WebGL shell for Yandex Games
- `Assets/Plugins/WebGL/YG.jslib` - JavaScript bridge to Yandex Games SDK
- `ProjectSettings` - Unity player/build settings
- `Packages/manifest.json` - package dependencies
- `B/` - committed build output, not source
- `Library`, `Temp`, `Logs`, `obj` - generated folders, avoid editing

## Runtime Architecture

- DI root is based on Zenject installers in `Assets/Code/Installers`
- State machine:
  - `BootstrapState`
  - `LoadProgressState`
  - `LoadLevelState`
  - `GameLoopState`
- `EnterPoint` resolves and initializes the state machine at startup
- `GameFactory` creates player, HUD, and start menu, and registers save-progress readers/writers
- Runtime assets are loaded through `AssetsProvider`, which currently uses `Resources.Load`
- Save system is `PlayerPrefs`-based through `SaveLoadService`
- Yandex SDK is initialized from `BootstrapState` through `IYandexService`
- Yandex language detection, fullscreen ads, and rewarded ads are partially bridged

## Main Scenes

- `Assets/Scenes/Initial.unity` - startup scene with start menu flow
- `Assets/Scenes/Main.unity` - gameplay scene

Build settings currently include only these two scenes.

## Important Data/Services

- `PersistantStaticData` - economy/spawn/healing balancing
- `PersistantPlayerStaticData` - player base HP/damage/infection stats
- `EnemyFactory` / `AIDFactory` - pooled runtime spawning
- `MaintenanceEnemyesCount` / `MaintenanceAIDCount` - refill systems
- `Post`, `ParcelGenerator`, `DeliveredParcelsCounter`, `Salary`, `Healing`, `Experience`
- `YandexPlatform` / `YandexService` / `YandexSDKBridge` - WebGL SDK wrapper
- `RewardedAdService` - rewarded ad entry point for gameplay bonuses
- `LocalizationService` - uses local system language first, then SDK language callback

## Working Rules For Agents

- Treat this as a Unity gameplay project first, not a generic C# app.
- Prefer changing files under `Assets/Code`, `ProjectSettings`, and source-controlled assets only when necessary.
- Do not modify `Library`, `Temp`, `Logs`, `obj`, or `B` unless the task is explicitly about generated/build output.
- Be careful with prefab and scene references: code often relies on serialized fields, tags, and `Resources` paths.
- When renaming or moving runtime-loaded assets, update `AssetPath` constants and verify `Resources` paths still match.
- Before removing packages or third-party folders, verify whether the content is referenced by scenes/prefabs.
- Assume there may be user changes in the worktree; do not revert unrelated modifications.
- For HUD work, preserve the current authored placement of `MappRoot` and `mapp_frame` unless the user explicitly asks to move them.
- For Yandex Games work, test WebGL behavior in browser/debug panel, not only in the Unity editor.

## Known Technical Risks

- Heavy reliance on `Resources.Load` and `GameObject.Find*` makes startup/runtime wiring fragile.
- Several systems subscribe to events but do not clearly unsubscribe across pooled object reuse.
- Save/load is tied to `PlayerPrefs`, which is fine for prototype data but weak for production saves.
- `SaveLoadService.SaveProgress()` now calls `PlayerPrefs.Save()` after writing progress. This improves WebGL persistence, but Yandex cloud saves are still not implemented.
- The repo contains substantial unused sample/demo/store content that likely inflates build size.
- `Packages/manifest.json` includes heavyweight packages that may not be needed in shipping builds, especially `com.unity.visualscripting` and `com.unity.feature.development`.
- Yandex `LoadingAPI.ready()` is now requested after the start menu is created and the loading curtain is hidden; verify the final timing in a real WebGL/Yandex debug build.
- The WebGL template now loads `/sdk.js`, which matches Yandex archive hosting; use an absolute SDK URL only for a custom-domain build.
- Yandex pause/resume events are bridged and routed through `YandexService`; gameplay markup still needs a dedicated audit.
- Cloud saves, leaderboards, payments, and rating prompts are not implemented. This is acceptable only if the draft does not declare/need those features.

## Build Size Hotspots

- `Assets/Plugins/Zenject/OptionalExtras/...` sample content
- `Assets/TextMesh Pro/Examples & Extras`
- `Assets/FromStore/...` demo scenes/textures
- `Assets/JMO Assets/...` large VFX prefab collections
- Broad `Resources` usage can pull more assets than expected into the build

## Suggested First Improvements

- Replace `Resources.Load` paths with serialized prefab references or Addressables for runtime-created objects.
- Remove or exclude package samples/demo content from the player build.
- Audit installed packages and remove unused modules/packages.
- Add null-guards and remove duplicate initialization work in startup/state-loading code.
- Review pooled enemy lifecycle for duplicate event subscriptions.

## Yandex Games Publishing Checklist

Last checked against Yandex Games docs on 2026-04-19:

- Game requirements: https://yandex.com/dev/games/doc/en/concepts/requirements
- SDK connection: https://yandex.com/dev/games/doc/en/sdk/sdk-about
- Game Ready/gameplay markup: https://yandex.com/dev/games/doc/en/sdk/sdk-game-events
- Player data/cloud saves: https://yandex.com/dev/games/doc/en/sdk/sdk-player
- Advertising: https://yandex.com/dev/games/doc/en/sdk/sdk-adv

### Must Fix Before Moderation

1. Done in code: `Assets/WebGLTemplates/Yandex/index.html` loads the SDK by `/sdk.js` for archives uploaded to Yandex servers. Keep the absolute SDK URL only for a custom-domain build.
2. Done in code: `LoadingAPI.ready()` is requested after `BootstrapState.EnterLoadLevel()` creates the start menu and hides the loading curtain. Still verify in a real WebGL/Yandex debug build that moderation accepts the start menu as game-ready.
3. Done in code: `YG.jslib` subscribes to `game_api_pause` / `game_api_resume`, `YandexPlatform` forwards them to C#, and `YandexService` pauses `Time.timeScale`, `AudioListener.pause`, and `IInputService.IsEnabled` while preserving previous values.
4. Done in code: fullscreen and rewarded ad callbacks now request the same platform pause as `game_api_pause`, and resume only after both ad pause and platform pause are released.
5. Done in code: `SaveLoadService.SaveProgress()` calls `PlayerPrefs.Save()` after `PlayerPrefs.SetString()`. For a stronger release version, implement Yandex cloud save through `player.setData/getData`.
6. Build a WebGL archive and verify the uncompressed total size is under 100 MB. Remove/exclude unused demo/sample/store content if it exceeds the limit.
7. Verify the WebGL archive root contains `index.html`, and file/folder names in the uploaded archive do not contain spaces or Russian characters.
8. Test mobile and desktop aspect ratios: no cropped HUD, no overlapping texts, no distorted UI, no page scroll/swipe-to-refresh, no long-tap selection/context menu.
9. Confirm the game is playable for more than 10 minutes and has gradual difficulty/content depth. Add more levels/tasks/tutorial content if the current build is too short.
10. Prepare required draft materials: title, description, how to play, version, categories, icon, cover, screenshots, supported platforms, orientation, age rating, sources/licensing note.

### Strongly Recommended Before First Release

1. Add a short in-game controls/help screen or make sure the Yandex draft "How to play" fully explains controls.
2. Add a sound mute toggle and a pause menu; Yandex recommends both, and they also make pause/resume integration cleaner.
3. Add a first-session tutorial or guided start, because the mechanics combine delivery, infection, combat, healing, and saves.
4. Add English localization if publishing outside RU-region languages. Russian is the practical minimum for RU/BE/KZ/UK/UZ audiences.
5. Add SDK-driven automatic language selection coverage tests with Yandex debug panel language mocks.
6. If the game uses records, scores, or endless competition, add Yandex leaderboards/stats; otherwise leave them undeclared in the draft.
7. If the game will use rewarded ads, make the reward button text explicit: the player must understand they are watching an ad and what exact reward they get.
8. Add a final asset-license audit for third-party folders (`JMO Assets`, `Palmov Island Assets`, `Polygon`, `ToonyTinyPeople`) and keep purchase/import evidence outside the build.

### Current Yandex Integration Status

- Present: custom Yandex WebGL template, JS SDK bridge, SDK init, language callback, `LoadingAPI.ready()` wrapper, fullscreen ads, rewarded ads.
- Missing: gameplay start/stop markup, cloud save bridge, leaderboard/stats bridge, payments bridge, rating bridge, desktop/mobile moderation test pass.
- Watch: `ReadyOnce()` is requested by `BootstrapState.EnterLoadLevel()`; `YandexService` now waits for SDK readiness before sending the bridge call if the SDK initializes later.
