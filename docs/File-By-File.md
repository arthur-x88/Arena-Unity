# File-By-File Map (Expanded)

Use this as a compass when navigating the codebase. Paths are workspace‑relative.

## Core Gameplay

- Entities
  - `Assets/Scripts/Core/Entity/Unit/Unit.cs`: Base unit logic (attributes, motion, spells, auras, control states).
  - `Assets/Scripts/Core/Entity/Player/Player.cs`: Player‑specific state, control tokens, visibility, class switching.
  - `Assets/Scripts/Core/Entity/UnitManager.cs`: Instantiate/destroy units, collider→unit mappings.
  - Controllers (partials):
    - `Assets/Scripts/Core/Entity/Unit/Controllers/Unit.SpellController.cs`: Casting, hits, crits, absorbs, modifiers, triggers.
    - Additional controllers under `.../Attributes`, `.../Motion`, `.../Combat`.

- Spells & Auras
  - `Assets/Scripts/Core/Spells/SpellInfo.cs`: Scriptable definition with attributes/costs/effects/targeting checks.
  - `Assets/Scripts/Core/Spells/Spell.cs`: Runtime spell execution (casting → processing → complete).
  - Effects: `Assets/Scripts/Core/Spells/Spell Effects/*.cs` (damage, heal, trigger, dispel, teleport, etc.).
  - Auras: `Assets/Scripts/Core/Spells/Spell Auras/*.cs` and `AuraEffectInfo*` types.

- Maps & Visibility
  - `Assets/Scripts/Core/Map/Map.cs`: Map runtime (entities list, visibility helpers, AoE search).
  - `Assets/Scripts/Core/Map/MapManager.cs`: Map registry, init on scene load, update loop.
  - Grid: `Assets/Scripts/Core/World/Grid/MapGrid.*.cs`: Spatial partitioning, visibility updates.
  - `Assets/Scripts/Core/Balance/World/MapSettings.cs`: Scene component with bounding box, grid cell size, spawns, scenario actions.

- Balance Data (Scriptables)
  - `Assets/Scripts/Core/Balance/BalanceReference.cs`: Entry point to balance content; fast lookup dictionaries.
  - `Assets/Scripts/Core/Balance/World/MapDefinition.cs`: Map metadata (id, name, max visibility range).
  - `Assets/Scripts/Core/Entity/Player/Classes/ClassInfo.cs`: Class spells/powers, availability.
  - `Assets/Scripts/Core/Entity/Player/Classes/ClassInfoContainer.cs`: Registered class list.

## Multiplayer (Photon Bolt)

- `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs`: Bolt start/stop/connect/session logic; scene load gate.
- `Assets/Scripts/Core/Multiplayer/PhotonBoltReference.cs`: Scriptable bridge to controller.
- Tokens: `Assets/Scripts/Core/Multiplayer/Tokens/*.cs`
  - `ServerRoomToken`: Name/MapName/MapId/Version for sessions.
  - `ClientConnectionToken`: Player name, device id, version, preferred class.
- Listeners (Core):
  - `Assets/Scripts/Core/Multiplayer/Listeners/PhotonBoltBaseListener.cs`: Base GlobalEventListener.
  - `Assets/Scripts/Core/Multiplayer/Listeners/PhotonBoltSharedListener.cs`: Client/shared scene init and map registration.

## Server Orchestration

- `Assets/Scripts/Server/Multiplayer/WorldServer.cs`: Player lifecycle, default scoping, create players, attach/detach events.
- Game listeners: `Assets/Scripts/Server/Multiplayer/Game Listeners/*`
  - `GamePlayerListener.cs`: Speed/root/movement control replication.
  - `GameSpellListener.cs`: Broadcast spell launch, hit, damage, heal, cooldowns.
- Network listeners: `Assets/Scripts/Server/Multiplayer/Network Listeners/*`
  - `Base/PhotonBoltServerListener.cs`: Connect validation, session create/update, map loaded hook.
  - `NetworkSpellListener.cs`: Handle cast/cancel requests.
  - `GeneralInputListener.cs`: Targeting, emotes, chat, class change.

## Client UI/UX

- Lobby: `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`
  - Map/session listing, start server, start client, single‑player, version display.
- Battle HUD: `Assets/Scripts/Client/UI/Panels/Battle/BattleHudPanel.cs`
  - Unit frames, cast bar, buffs, action bars, action error display.
- UI Components live under `Assets/Scripts/Client/UI/*` (ActionBars, Frames, Tooltips, Screens).

## Workflow & Bootstrapping

- `Assets/Scripts/Workflow/GameManager.cs`: World creation/destruction, ScriptableContainer lifecycle, update loop.
- Standard: `Assets/Scripts/Workflow/Standard/WorkflowStandard.cs`: Lobby→Battle flow on host/join.
- Dedicated: `Assets/Scripts/Workflow/Dedicated/WorkflowDedicated.cs`: Headless server lifecycle and restart policy.

## Common Utilities

- Scriptables: `Assets/Scripts/Common/Scriptables/*`
  - `ScriptableReference` + `ScriptableContainer` registration system.
- Events/Assertions/Logging live under `Assets/Scripts/Common/*`.
- Build/version: `Assets/Scripts/Common/BuildInfo.cs` (central network version).

## Photon SDK & Settings

- Bolt runtime settings: `Assets/Photon/PhotonBolt/resources/BoltRuntimeSettings.asset`
- Editor scripts: `Assets/Photon/PhotonBolt/editor/scripts/*`

