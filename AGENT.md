# AGENT.md

## Project Overview

- Project: `Postman`
- Engine: Unity
- Platform in README: Android
- Current repo also contains a Windows build in `B/`
- Core stack: Zenject, Unity UI, AI Navigation, SimpleInput

Game premise: the player delivers parcels during a zombie outbreak, manages health/infection, fights enemies, collects aid kits, and uses save points.

## Repository Map

- `Assets/Code` - main gameplay/runtime code
- `Assets/Scenes` - scenes in build settings: `Initial`, `Main`
- `Assets/Resources/Prefabs` - runtime-loaded prefabs via `Resources.Load`
- `Assets/ScriptableObjects` - static gameplay data
- `Assets/Plugins/Zenject` - DI framework plus large optional/sample content
- `Assets/TextMesh Pro` - TMP package content, includes examples/extras
- `Assets/FromStore`, `Assets/JMO Assets` - third-party art/VFX packs
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

## Working Rules For Agents

- Treat this as a Unity gameplay project first, not a generic C# app.
- Prefer changing files under `Assets/Code`, `ProjectSettings`, and source-controlled assets only when necessary.
- Do not modify `Library`, `Temp`, `Logs`, `obj`, or `B` unless the task is explicitly about generated/build output.
- Be careful with prefab and scene references: code often relies on serialized fields, tags, and `Resources` paths.
- When renaming or moving runtime-loaded assets, update `AssetPath` constants and verify `Resources` paths still match.
- Before removing packages or third-party folders, verify whether the content is referenced by scenes/prefabs.
- Assume there may be user changes in the worktree; do not revert unrelated modifications.

## Known Technical Risks

- Heavy reliance on `Resources.Load` and `GameObject.Find*` makes startup/runtime wiring fragile.
- Several systems subscribe to events but do not clearly unsubscribe across pooled object reuse.
- Save/load is tied to `PlayerPrefs`, which is fine for prototype data but weak for production saves.
- The repo contains substantial unused sample/demo/store content that likely inflates build size.
- `Packages/manifest.json` includes heavyweight packages that may not be needed in shipping builds, especially `com.unity.visualscripting` and `com.unity.feature.development`.

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
