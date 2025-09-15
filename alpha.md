Alpha: Warcraft Arena – Features, Architecture, and Examples

Overview
- Scope: PvP arena sandbox with Photon Bolt networking, class/spell systems, map grid visibility, and UI workflows for lobby and battle.
- Code pillars: Core gameplay and balance live under `Assets/Scripts/Core` with Client UI/UX in `Assets/Scripts/Client` and Server glue in `Assets/Scripts/Server`.
- Networking: Photon Bolt drives client/server/session orchestration with listeners and tokens in `Assets/Scripts/Core/Multiplayer` and `Assets/Scripts/Server/Multiplayer`.
- Workflows: Two entry paths — Standard (host/join) and Dedicated (headless server orchestration) under `Assets/Scripts/Workflow`.

Gameplay Features
- Teams and FFA: Teams (`Assets/Scripts/Core/World/Enums/Team.cs`) and free‑for‑all logic on units. Hostility/friendliness checks in `Unit.IsHostileTo` / `Unit.IsFriendlyTo`.
- Player and Creatures: `Player`/`Creature` inherit from `Unit` with shared controllers for attributes, combat, motion, spells, auras.
- Movement and Control:
  - Movement flags/inputs, root, stun, confusion states in `Assets/Scripts/Core/Entity/Unit/Unit.cs` and motion controller.
  - Server relays speed/root/control state to clients via events (`GamePlayerListener`).
- Death and Resurrection:
  - Health/DeathState managed by `Unit.Attributes` and `Unit.ModifyDeathState`. Non‑death‑persistent auras are removed on death.
- Combat:
  - Damage/heal pipelines apply aura interrupts, absorb shields, crit multipliers, damage multipliers, diminishing mechanics (via enums/effects).
  - Server events broadcast results (damage/heal/miss) to UI and other clients.
- Targeting and Visibility:
  - Client target selection raises network requests; server validates and updates target state.
  - Server side visibility/LoV via grid cells; client HUD follows MapGrid notifications.

Classes and Resources
- Class Types: `Assets/Scripts/Core/World/Enums/ClassType.cs` enumerates classes (Warrior, Mage, etc.).
- Class Data: `ClassInfo`/`ClassInfoContainer` define availability, class spells, and power types.
- Power Types: `SpellPowerType` and `SpellPowerTypeInfo` model mana/rage/energy/health costs and display power.
- Switching: Players can switch classes at runtime; client HUD/action bars update to reflect current class spells.

Spells and Auras
- Spell Assets: Scriptable `SpellInfo` assets define casting attributes, ranges, effects, cost, target types, flags (`Assets/Scripts/Core/Spells/SpellInfo.cs`).
- Effects: Concrete effects in `Assets/Scripts/Core/Spells/Spell Effects` (damage, heal, add power, charge, dispel, teleport, trigger spell, etc.).
- Auras: `AuraInfo` with `AuraEffectInfo` entries define persistent modifiers (immunities, absorbs, states like stun/root/confuse, etc.).
- Casting Lifecycle:
  - Client requests cast (SpellCastRequest* events) with explicit targets or destination.
  - Server validates costs/shape‑shift/target checks (`SpellInfo.Check*`) then starts casting or returns error.
  - On launch/hit, server fires events to clients; damage/heal processed with crit/absorb/resist logic in `Unit.SpellController`.
- Cooldowns and Charges: Standard and per‑charge cooldowns are computed server‑side and sent to the controlling client.
- Modifiers and Triggers: Aura‑based `SpellModifier`s and `AuraEffectSpellTrigger`s allow dynamic adjustments and procs.

Maps and Scenarios
- Map Definition: `MapDefinition` sets ID, availability, max players, visibility range, type, expansion visuals.
- Scene Settings: `MapSettings` component in the scene links to `BalanceReference`, bounding box, grid cell size, default spawn point, team spawns, and scenario actions.
- Grid and Visibility:
  - `MapGrid` partitions the world into cells sized by `MapSettings.gridCellSize` and tracks cell membership.
  - Server updates visibility lists for players/creatures and relocates entities; out‑of‑bounds fallback to default spawn.
  - AoE queries use physics overlaps and faction checks (`Map.SearchAreaTargets`).
- Scenario Actions:
  - Hooked on server launch to spawn entities or AI (`SpawnCreature`, `SpawnPlayerAI`), configured via `CustomSpawnSettings`.

Networking Architecture (Photon Bolt)
- Controller and Reference:
  - `PhotonBoltController` (MonoBehaviour) manages Bolt lifecycle (start server/client/single‑player, connect, session list) and scene load hooks.
  - `PhotonBoltReference` (ScriptableReference) bridges gameplay code/Scriptable workflows to the active controller instance.
- Listeners:
  - Shared/Client/Server listeners are toggled based on networking mode; they persist across Bolt restarts.
  - Server listener (`Server/Multiplayer/Network Listeners/Base/PhotonBoltServerListener.cs`) gates connection tokens, creates players, sets default scoping, and pipelines events.
- Tokens:
  - `ServerRoomToken`: map name, server name, version.
  - `ClientConnectionToken`: client name, unique device ID, preferred class, version; validated in `ConnectRequest`.
  - `ClientRefuseToken`: reason for refusal; mapped to client‑side disconnect reasons.
- Event Flow (cast example):
  1) Client sends `SpellCastRequestEvent` or `SpellCastRequestDestinationEvent`.
  2) Server `PhotonBoltServerListener.HandleSpellCast` validates and triggers `Unit.Spells.CastSpell`.
  3) On success, server raises `GameEvents.ServerSpellLaunch` and replies with `SpellCastRequestAnswerEvent`.
  4) On hit/damage/heal, server raises `GameEvents.ServerSpellHit` / `ServerDamageDone` / `ServerHealingDone`; corresponding Bolt events update clients/UI.
- Sessions and Scenes:
  - Server may create a session using `BoltMatchmaking.CreateSession`; loads the selected map.
  - Client connects and on `SceneLoadLocalDone` map managers initialize (`MapManager.InitializeLoadedMap`).
  - Note: Listeners explicitly ignore a “Launcher” scene due to Bolt version quirk.

Worlds and Managers
- `World` base holds `MapManager`, `UnitManager`, `SpellManager` and updates them each tick.
- `WorldServer` adds:
  - Player management (create/attach/assign control, disconnect grace timers), default scoping per connection.
  - Listeners for player/spell events (`GamePlayerListener`, `GameSpellListener`).
  - Single‑player flow creates the local server player as well.
- `UnitManager` wraps Bolt instantiation, default health, motion control, collider‑>unit lookups.
- `MapManager` manages active scenes/maps, optionally with multi‑threaded updates via `MapUpdater` when no client logic is present.

Client UI and Input
- Lobby:
  - `LobbyPanel` lists maps from `BalanceReference.Maps` and active sessions from `PhotonBoltReference.Sessions`.
  - Start Single Player, Start Server (host with client logic), or Start Client; region selection updates Bolt best region.
- Battle HUD:
  - `BattleHudPanel` wires unit frames, buff frames, cast bar, action bars, and error display; updates when local player gains/loses control.
  - Action bars reconfigure based on player class, driving spell requests.
- Input/Targeting:
  - Client sends `TargetSelectionRequestEvent` to set server authoritative target.
  - Chat requests are relayed to clients as `UnitChatMessageEvent`.

Balance Data and Scriptables
- `BalanceReference` aggregates data:
  - Spells, Auras, Factions, Classes, MapDefinition list, AI unit infos.
  - Provides fast ID/type maps and helpers (e.g., IsStealthAura).
- Content structure:
  - Spells: `Assets/Scripts/Core/Spells` (effects, auras, modifiers, powers, processing enums/logic).
  - Classes: `Assets/Scripts/Core/Entity/Player/Classes`.
  - World/Maps: `Assets/Scripts/Core/Balance/World` and scene `MapSettings`.

Key Files and Where to Look
- Networking:
  - `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs`
  - `Assets/Scripts/Core/Multiplayer/PhotonBoltReference.cs`
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/Base/PhotonBoltServerListener.cs`
  - `Assets/Scripts/Core/Multiplayer/Listeners/PhotonBoltSharedListener.cs`
- Gameplay Systems:
  - `Assets/Scripts/Core/Entity/Unit/Unit.cs` (+ controllers sub‑partials)
  - `Assets/Scripts/Core/Entity/Player/Player.cs`
  - `Assets/Scripts/Core/Spells/SpellInfo.cs`, `Spell.cs`, effects under `Spell Effects`, auras under `Spell Auras`
  - `Assets/Scripts/Core/Map/Map.cs`, `Assets/Scripts/Core/Map/MapManager.cs`
  - `Assets/Scripts/Core/World/Grid/*` (visibility grid)
- Server Glue:
  - `Assets/Scripts/Server/Multiplayer/WorldServer.cs`
  - `Assets/Scripts/Server/Multiplayer/Game Listeners/*`
  - `Assets/Scripts/Server/Multiplayer/Network Listeners/*`
- Client UI/UX:
  - `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`
  - `Assets/Scripts/Client/UI/Panels/Battle/BattleHudPanel.cs`
  - Action bars, cast frames, unit frames under `Assets/Scripts/Client/UI/*`

Examples

Start Single‑Player From Lobby
```csharp
// LobbyPanel.cs (calls through the PhotonBoltReference)
photonReference.StartSinglePlayer(
    new ServerRoomToken(serverNameInput.text, playerNameInput.text, selectedMapSlot.MapDefinition.MapName),
    OnServerStartSuccess,
    OnServerStartFail
);
```

Host a Server With Client Logic
```csharp
// LobbyPanel.cs
photonReference.StartServer(
    new ServerRoomToken(serverNameInput.text, playerNameInput.text, selectedMapSlot.MapDefinition.MapName),
    withClientLogic: true,
    onStartSuccess: OnServerStartSuccess,
    onStartFail: OnServerStartFail
);
```

Join a Session (Client)
```csharp
// LobbyPanel.cs
var clientToken = new ClientConnectionToken {
    PrefferedClass = (ClassType)PlayerPrefs.GetInt(UnitUtils.PreferredClassPrefName, 0),
    Name = playerNameInput.text
};
photonReference.StartConnection(lobbySessionSlot.UdpSession, clientToken, OnConnectSuccess, OnConnectFail);
```

Request a Spell Cast (Client → Server)
```csharp
// Client creates and raises a Bolt event (handled by server listener)
var e = SpellCastRequestEvent.Create(GlobalTargets.OnlyServer);
e.SpellId = mySpellInfo.Id;
e.MovementFlags = (int)player.MovementFlags; // e.g., to validate cast conditions
e.Send();
```

Server‑Side Cast Handling
```csharp
// Assets/Scripts/Server/Multiplayer/Network Listeners/NetworkSpellListener.cs
private void HandleSpellCast(Event request, int spellId, MovementFlags flags, Vector3? dest = null) {
  Player caster = World.FindPlayer(request.RaisedBy);
  var answer = request.FromSelf
    ? SpellCastRequestAnswerEvent.Create(GlobalTargets.OnlyServer)
    : SpellCastRequestAnswerEvent.Create(request.RaisedBy);

  if (caster == null) { answer.Result = (int)SpellCastResult.CasterNotExists; answer.Send(); return; }
  if (!balance.SpellInfosById.TryGetValue(spellId, out var info)) { answer.Result = (int)SpellCastResult.SpellUnavailable; answer.Send(); return; }

  var options = dest.HasValue
    ? new SpellCastingOptions(new SpellExplicitTargets { Destination = dest.Value })
    : new SpellCastingOptions(movementFlags: flags);

  var result = caster.Spells.CastSpell(info, options);
  if (result != SpellCastResult.Success) { answer.Result = (int)result; answer.Send(); }
}
```

Spawn Creature on Server Start (Scenario)
```csharp
// Attach SpawnCreature to a scene object and configure CustomSpawnSettings
// Assets/Scripts/Core/Scenario/Scenario Behaviours/SpawnCreature.cs
Creature creature = World.UnitManager.Create<Creature>(BoltPrefabs.Creature, new Creature.CreateToken {
  Position = customSpawnSettings.SpawnPoint.position,
  Rotation = customSpawnSettings.SpawnPoint.rotation,
  CreatureInfoId = creatureInfo.Id,
  FactionId = Balance.DefaultFaction.FactionId,
  DeathState = DeathState.Alive,
  FreeForAll = true,
});
creature.BoltEntity.TakeControl();
```

Add a New Spell (Content Workflow)
- Create a `Spell Info` asset via the menu: Game Data → Spells → Spell Info.
- Configure effects under `Effects` (e.g., `EffectSchoolDamage` with school mask and base amount).
- Set casting attributes (GCD, cast time, range, target type) and power costs.
- Add the spell to a `ClassInfo`’s `classSpells` list to expose in the class toolkit.
- Optional: Create `AuraInfo` for periodic or stateful behavior and use `EffectApplyAura` to apply it.

Add a New Arena Map
- Scene Setup:
  - Add a `MapSettings` component somewhere in the scene hierarchy.
  - Assign `BalanceReference`, `boundingBox` (BoxCollider enclosing the playable area), `defaultSpawnPoint`.
  - Populate `spawnInfos` with team spawn transforms.
  - Optionally add scenario actions (e.g., `SpawnCreature`, `SpawnPlayerAI`).
- Scriptable Setup:
  - Create a `Map Definition` asset and set ID, name, max players, and visibility range.
  - Add it to the main `BalanceDefinition` so it appears in the lobby (through `BalanceReference`).

Switch Classes In‑Game
```csharp
// Server listener handles requests
public override void OnEvent(PlayerClassChangeRequestEvent req) {
  var player = World.FindPlayer(req.RaisedBy);
  var type = (ClassType)req.ClassType;
  if (player != null && type.IsDefined()) player.SwitchClass(type);
}
```

Target Selection (Client → Server → All)
```csharp
// Client request
var t = TargetSelectionRequestEvent.Create(GlobalTargets.OnlyServer);
t.TargetId = target.BoltEntity.NetworkId;
t.Send();

// Server applies authoritative target and relays visibility via MapGrid
```

Data Extension: Auras and Immunities
- To add an immunity aura: create an `Aura Info` asset and add `AuraEffectInfoSchoolImmunity` with the desired `SpellSchoolMask`.
- Apply it via a spell using `EffectApplyAura`. Server will respect immunities in `Unit.SpellController.IsImmune*` checks.

Workflows
- Standard (`Assets/Scripts/Workflow/Standard/WorkflowStandard.cs`):
  - On game map loaded: creates `WorldServer` (if hosting) and `WorldClient` view, shows `BattleScreen`.
  - On disconnects: returns to `LobbyScreen` with reason.
- Dedicated (`Assets/Scripts/Workflow/Dedicated/WorkflowDedicated.cs`):
  - Applies headless settings, starts server via `PhotonBoltReference`, restarts on failures with backoff.

Event Reference (Selected)
- Player/Movement
  - `GameEvents.ServerPlayerSpeedChanged`, `ServerPlayerRootChanged`, `ServerPlayerMovementControlChanged` → sent to controller to keep client in sync.
- Spells/Combat
  - `GameEvents.ServerSpellLaunch`, `ServerSpellHit`, `ServerDamageDone`, `ServerHealingDone` → relayed as Bolt events for UI/feedback.
- Session/Network
  - `GameEvents.SessionListUpdated`, `DisconnectedFromHost`, `DisconnectedFromMaster`, `GameMapLoaded`.

Folder Guide
- `Assets/Scripts/Core`: Engine‑level gameplay (entities, spells, map/visibility, balance definitions, conditions).
- `Assets/Scripts/Client`: UX (UI panels, action bars, HUD, camera, audio, localization).
- `Assets/Scripts/Server`: Server‑only orchestration and authoritative listeners.
- `Assets/Scripts/Workflow`: Bootstrapping flows for Standard and Dedicated use cases.
- `Assets/Photon/PhotonBolt`: Bolt SDK and runtime configuration (e.g., `resources/BoltRuntimeSettings.asset`).

Operational Notes
- Bolt Scenes: Listeners skip the “Launcher” scene during callbacks to avoid duplicate player creation; ensure gameplay scenes host `MapSettings`.
- Versions: `PhotonBoltController.Version` is compared in connect tokens; update both client and server if changed.
- Regions: Lobby region dropdown sets `BoltRuntimeSettings.instance.UpdateBestRegion` before client connect.

Troubleshooting
- Cannot cast: Check `SpellCastResult` from the server’s `SpellCastRequestAnswerEvent` and review `SpellInfo` target/cost/range flags.
- No sessions listed: Ensure client has started (`Start Client`) and `SessionListUpdated` is firing, region is correct, and transport is reachable.
- Spawn issues: Confirm `MapSettings.boundingBox` encloses spawn points; entities outside are repositioned to `defaultSpawnPoint`.
- Visibility anomalies: Verify `MapGrid` cell size vs. map scale and `MapDefinition.MaxVisibilityRange`.

Extensibility Checklist
- New spell:
  - Add `SpellInfo` asset + effects → add to class.
  - If it applies auras, author `AuraInfo` and use `EffectApplyAura`.
- New status mechanic:
  - Add a new `AuraEffectInfo*` and handle it in `Unit` state update functions if it affects control/movement.
- New map/arena:
  - Create scene + `MapSettings` + `MapDefinition`; add to `BalanceDefinition`.
- New UI element:
  - Hook into events in `GamePlayerListener`/`GameSpellListener` or create a new client‑side listener/dispatcher.

