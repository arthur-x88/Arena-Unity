# Examples

This guide shows practical, copy‑pasteable examples for common tasks across networking, gameplay, and authoring.

## Networking

- Host a server with client logic from the lobby
  - File: `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`
  - Snippet:
    ```csharp
    private void OnServerButtonClicked() {
      var mapDef = selectedMapSlot.MapDefinition;
      var token = new ServerRoomToken(serverNameInput.text, playerNameInput.text, mapDef.MapName) { MapId = mapDef.Id };
      statusLabel.SetString(serverStartString);
      UpdateInputState(false);
      photonReference.StartServer(token, true, OnServerStartSuccess, OnServerStartFail);
    }
    ```

- Start Single‑Player (server+client in one process)
  - File: `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`
  - Snippet:
    ```csharp
    private void OnSinglePlayerButtonClicked() {
      var mapDef = selectedMapSlot.MapDefinition;
      var token = new ServerRoomToken(serverNameInput.text, playerNameInput.text, mapDef.MapName) { MapId = mapDef.Id };
      photonReference.StartSinglePlayer(token, OnServerStartSuccess, OnServerStartFail);
    }
    ```

- Client connects to a session with a preferred class
  - File: `Assets/Scripts/Client/UI/Panels/Lobby/LobbyPanel.cs`
  - Snippet:
    ```csharp
    var clientToken = new ClientConnectionToken {
      PrefferedClass = (ClassType) PlayerPrefs.GetInt(UnitUtils.PreferredClassPrefName, 0),
      Name = playerNameInput.text,
      Version = Common.BuildInfo.NetworkVersion
    };
    photonReference.StartConnection(slot.UdpSession, clientToken, OnConnectSuccess, OnConnectFail);
    ```

- Scene load gating (avoid non‑gameplay scenes)
  - File: `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs:150`
  - Snippet: `if (UnityEngine.Object.FindObjectOfType<MapSettings>() == null) return;`

- Server cast validation flow
  - File: `Assets/Scripts/Server/Multiplayer/Network Listeners/NetworkSpellListener.cs`
  - Steps:
    - Client raises `SpellCastRequestEvent`.
    - Server `HandleSpellCast(...)` resolves caster, looks up `SpellInfo`, constructs `SpellCastingOptions` (with movement flags or destination), and calls `caster.Spells.CastSpell`.
    - On success, server emits `GameEvents.ServerSpellLaunch` + `SpellCastRequestAnswerEvent` with Success; otherwise returns a `SpellCastResult` failure code.

## Spells and Auras

- Create a direct damage spell (fire bolt)
  - Author assets:
    - Create `Spell Info` under `Assets/Settings/Balance/Spells`.
    - Set `DamageClass = Magic`, `SchoolMask = Fire`, `ExplicitTargetType = Target`, and add an `EffectSchoolDamage` with base amount.
  - Runtime:
    - Add the spell to class via `ClassInfo.ClassSpells`.
    - The client can cast it; server applies crit/absorb and broadcasts results via `GameSpellListener`.

- Apply an aura (periodic slow)
  - Author assets:
    - Create `Aura Info` with an `AuraEffectInfo` (e.g., movement speed reduction) and duration.
    - Create a `Spell Info` with `EffectApplyAura` that references the `Aura Info`.
  - Runtime:
    - Server calculates duration using `Unit.SpellController.CalculateAuraDuration` and handles re‑application/refresh.

## Content Authoring

- Add a new arena map
  - Scene:
    - Add `MapSettings` to a scene object.
    - Set `BoundingBox`, `DefaultSpawnPoint`, `GridCellSize`, and reference your `BalanceReference`.
    - Configure `spawnInfos` (Alliance/Horde) and any `ScenarioAction`s (e.g., spawners).
  - Data:
    - Create a `Map Definition` asset: set Id, name, visibility range, type.
    - Ensure the `BalanceDefinition` includes your `MapDefinition` (so the Lobby sees it).
  - Networking:
    - Lobby passes both `MapName` and `MapId` in `ServerRoomToken` when launching.

- Spawn a boss on server launch via ScenarioAction
  - File: `Assets/Scripts/Core/Scenario/Scenario Behaviours/SpawnCreature.cs`
  - Steps:
    - Place `SpawnCreature` in the scene and wire `CustomSpawnSettings`.
    - On `GameEvents.ServerLaunched`, it calls `World.UnitManager.Create<Creature>(...)` and `TakeControl()` for server control.

- Add a new class and expose spells in HUD
  - Data:
    - Create `Class Info` and add spells to `ClassSpells`.
    - Add it to `ClassInfoContainer` and to `BalanceDefinition`.
  - Runtime:
    - Player class switching is handled via `Player.SwitchClass`; HUD action bars update on `GameEvents.UnitClassChanged`.

## UI and HUD

- Update action bars on class change
  - File: `Assets/Scripts/Client/UI/Panels/Battle/BattleHudPanel.cs`
  - Snippet:
    ```csharp
    private void OnPlayerClassChanged() {
      foreach (var actionBar in actionBars)
        actionBar.ModifyContent(localPlayer.ClassType);
    }
    ```

- Show cast bar and cancel on stun/root
  - Files:
    - Player cast state: `Assets/Scripts/Core/Entity/Player/Player.cs`
    - Root handling: `Assets/Scripts/Core/Entity/Unit/Unit.cs` (UpdateRootState)

- Chat relay and sanitization
  - File: `Assets/Scripts/Server/Multiplayer/Network Listeners/GeneralInputListener.cs`
  - Behavior: Reject empty messages, clamp length, and broadcast `UnitChatMessageEvent` to everyone.

## Map & Visibility

- AoE target search
  - File: `Assets/Scripts/Core/Map/Map.cs`
  - Snippet:
    ```csharp
    var results = new List<Unit>();
    map.SearchAreaTargets(results, radius: 8f, center: player.Position, referer: player, checkType: SpellTargetChecks.Enemy);
    ```

- Grid debug lines in Editor only
  - File: `Assets/Scripts/Core/World/Grid/MapGrid.Base.cs`
  - Enable by defining `DEBUG_VIS_GRID` or view in Editor; excluded in builds.

## World Lifecycle

- Create/Destroy world and observe events
  - File: `Assets/Scripts/Workflow/GameManager.cs`
  - Snippet:
    ```csharp
    void StartMatch(bool hasServer, bool hasClient) {
      var world = hasServer ? (World)new Server.WorldServer(hasClient) : new Client.WorldClient(false);
      gameManager.CreateWorld(world); // Emits GameEvents.WorldStateChanged(world, true)
    }
    ```

