Changelog

Version: Unreleased (post-Unity 2019 → 2022 freeze)

Overview
- Addressed networking robustness, removed scene-name hacks, introduced MapId plumbing, centralized network versioning, fixed UI listener leak, added safer spawns and chat sanitization, and improved map update threading safety. Added developer documentation and an alpha feature document.

Networking
- Centralized network version string:
  - `Assets/Scripts/Common/BuildInfo.cs` (new): Added `BuildInfo.NetworkVersion`.
  - `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs`: `Version` now returns `BuildInfo.NetworkVersion`.
- Robust scene gating (removed "Launcher" string hack):
  - `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs`: In `SceneLoadLocalDone`, proceed only if a `MapSettings` exists in the scene.
  - `Assets/Scripts/Core/Multiplayer/Listeners/PhotonBoltSharedListener.cs`: Initialize map only when `MapSettings` is present; use `settings.Definition.Id` instead of hardcoded `1`.
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/Base/PhotonBoltServerListener.cs`: Same gating; initializes by `settings.Definition.Id`.
- MapId token plumbing and usage:
  - `Assets/Scripts/Core/Multiplayer/Tokens/ServerRoomToken.cs`: Added `MapId` with serialization in `Read/Write`.
  - `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`: When starting Server/Single Player, pass `MapId = selectedMapSlot.MapDefinition.Id` in `ServerRoomToken`.
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/Base/PhotonBoltServerListener.cs`: On scene load, also sets `roomToken.MapId = settings.Definition.Id`.
  - `Assets/Scripts/Server/Multiplayer/WorldServer.cs`: Uses `serverRoomToken.MapId` (with fallback) to resolve the main map for player spawns.
- Connection and chat validation:
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/Base/PhotonBoltServerListener.cs`: Validates client name (trim, 1–24 chars) before accepting.
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/GeneralInputListener.cs`: Sanitizes chat (collapse whitespace, trim) and clamps to 256 characters.

Maps, World, and Visibility
- Safer spawn selection:
  - `Assets/Scripts/Core/Balance/World/MapSettings.cs`: `FindSpawnPoints` now falls back to `defaultSpawnPoint` with a warning when a team has no configured spawns.
- Debug drawing guards:
  - `Assets/Scripts/Core/World/Grid/MapGrid.Base.cs`: Wrapped `Drawing.DrawLine` in `#if UNITY_EDITOR || DEBUG_VIS_GRID` to remove cost in builds.
- Map update threading safety:
  - `Assets/Scripts/Core/Map/MapUpdater.cs`: Fixed `Wait()` to actually wait on pending work; improved `Deactivate()` (complete-adding, join workers); added graceful cancellation handling.

UI and Workflow
- Listener lifecycle fix:
  - `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`: Fixed `RemoveListener` call for `serverNameInput` in `PanelDeinitialized` to prevent duplicate listeners.
- Server/single-player start:
  - `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`: Server and Single Player launches now pass both `MapName` and `MapId` via `ServerRoomToken`.

Documentation
- New alpha feature document:
  - `alpha.md` (new): Exhaustive overview of features, architecture, examples, and authoring workflows.
- Code audit added to milestones:
  - `milestones.md`: Appended “Code Audit (Unity 2019 → 2022 Freeze)” with findings, best practices, upgrade guidance, and next steps.

Compatibility Notes
- Network protocol change: `ServerRoomToken` gained an `int MapId` in its Bolt serialization. Older client/server builds without this field are not wire-compatible. Rebuild both client and server from this revision.
- Debug visuals are now excluded from non-Editor builds unless `DEBUG_VIS_GRID` is defined.

Known Follow‑ups (tracked in audit)
- Replace `Mutex` in `MapManager` with in-process synchronization.
- Consider replacing tag-based lookup in `PhotonBoltReference` with explicit wiring.
- Decide on `MapUpdater` strategy (remain single-threaded vs. refactor for data-only background work).
- Evaluate Unity 6 upgrade after SDK support validation (Photon Bolt vs Fusion).

