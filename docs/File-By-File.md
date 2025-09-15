# File-by-File Plan — 0.1 MOBA x Arena

This is the authoritative checklist of new/changed files. Paths are relative to repo root.

Legend
- [N] New file
- [M] Modify existing
- [SO] ScriptableObject asset

1) Match Lifecycle & Teams
- [N] Assets/Scripts/Core/Match/MatchState.cs — enum + struct snapshot (Phase, TimeMs, TeamScores, TowerHp[])
- [N] Assets/Scripts/Core/Match/MatchManager.cs — server authority; state machine; win checks; events; replication API
- [N] Assets/Scripts/Core/Match/MatchNetworkState.cs — Bolt state or lightweight replicator for MatchState
- [N] Assets/Scripts/Core/Match/TeamManager.cs — team assignment, team colors, spawn selection
- [M] Assets/Scripts/Server/Multiplayer/WorldServer.cs — hook MatchManager; spawn routing; end flow to lobby
- [M] Assets/Scripts/Core/Utils/MultiplayerUtils.cs — small helpers (team identifiers, colors)

2) Map — Lane & Structures
- [SO][N] Assets/Settings/Balance/Maps/LaneDefinition.asset — waypoints list; spawn points per team; base/tower transforms
- [SO][N] Assets/Settings/Balance/Maps/StructureDefinition.asset — tower/base stats (HP, damage, attackRate, range)
- [N] Assets/Scripts/Core/World/LaneDefinition.cs — runtime view over waypoints
- [N] Assets/Scripts/Core/World/StructureDefinition.cs — runtime data for towers/bases
- [N] Assets/Prefabs/Structures/Tower.prefab — collider, attack origin, script hook
- [N] Assets/Prefabs/Structures/Base.prefab — collider, script hook

3) Minion Waves
- [SO][N] Assets/Settings/Balance/Units/Minion/MinionDefinition.asset — HP, damage, move speed, gold/XP values
- [N] Assets/Prefabs/Units/Minion.prefab — model + Unit wiring
- [N] Assets/Scripts/Server/AI/Minion/MinionBrain.cs — simple state machine (FollowLane, Acquire, Attack)
- [N] Assets/Scripts/Server/AI/Minion/WaveManager.cs — spawn logic; interval; composition; team side
- [M] Assets/Scripts/Core/Entity/Unit/Unit.cs — ensure server-only AI ticks (or add a server tick hook)

4) Structures (Towers/Base)
- [N] Assets/Scripts/Server/AI/Structures/TowerBrain.cs — targeting rules; attack cadence; aggro swap logic
- [N] Assets/Scripts/Server/AI/Structures/BaseCore.cs — HP, death triggers victory
- [SO][N] Assets/Settings/Spells/StructureAttack Spell.asset — reuse Spell pipeline for tower shots (projectile or instant)

5) Economy & Progression
- [N] Assets/Scripts/Server/Match/Economy/PlayerEconomy.cs — gold, assists, CS; server-only state; events
- [N] Assets/Scripts/Server/Match/Economy/ExperienceSystem.cs — XP distribution; level-ups; stat scaling hooks
- [M] Assets/Scripts/Core/Entity/Player/Player.cs — attach PlayerEconomy; expose KDA/CS; level
- [N] Assets/Scripts/Core/Entity/Player/PlayerLevelDefinition.cs [SO] — per-level stat/rank tables

6) Items & Shop (passive only for 0.1)
- [SO][N] Assets/Settings/Items/ItemDefinition.asset (multiple) — cost, prerequisites, applied auras/modifiers
- [N] Assets/Scripts/Core/Items/ItemDefinition.cs — runtime data (list of AuraInfo/SpellModifier references)
- [N] Assets/Scripts/Core/Items/Inventory.cs — holds purchased items; applies/removes auras
- [N] Assets/Scripts/Client/UI/Panels/Shop/ShopPanel.cs — simple UI; buy/sell; base-radius check
- [M] Assets/Scripts/Client/UI/Panels/Battle/BattleHUD.cs — add gold/level display hooks

7) HUD, Scoreboard, End Screen
- [N] Assets/Scripts/Client/UI/HUD/MatchHud.cs — timer, scores, tower statuses, killfeed hooks
- [N] Assets/Scripts/Client/UI/Panels/Scoreboard/ScoreboardPanel.cs — tab toggle; list players; K/D/A/CS/gold/items
- [N] Assets/Scripts/Client/UI/Panels/End/EndPanel.cs — winner, summary, leave button
- [M] Assets/Prefabs/UI/Screens — add canvases/panels for new screens

8) Recall & Respawn
- [SO][N] Assets/Settings/Balance/Spells/Recall Spell.asset — uses TeleportDirect; long cast; cancel on damage/move
- [N] Assets/Scripts/Server/Match/RespawnSystem.cs — manage death timers; spawn at base; scale with game time
- [M] Assets/Scripts/Core/Entity/Unit/Unit.cs — ensure death event hooks exist for respawn/economy

9) Networking & Scoping
- [N] Assets/Scripts/Core/Networking/MatchStateReplicator.cs — packs MatchState diff; sends on tick or change
- [M] Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs — register/replicate match channel; cleanup on end
- [M] Assets/Photon/PhotonBolt/resources/BoltPrefabDatabase.asset — register Minion/Tower/Base prefabs

10) Data & Tuning
- [SO][N] Assets/Settings/Balance/Match/MatchTuning.asset — wave interval, counts, reward tables, respawn formula
- [SO][N] Assets/Settings/Balance/Teams/TeamColors.asset — UI colors per team

Migration & Integration Notes
- Keep server authority: all MinionBrain/TowerBrain/Respawn/Economy run server-only; clients receive events/state.
- Prefer SOs for tunables; MatchTuning aggregates primary knobs.
- Use existing Spell pipeline for tower/minion attacks to reuse VFX, hit logic, combat text.

Open Questions
- Player cap per team (2 or 3)?
- Assist window duration (10s default)?
- XP split radius and falloff?
