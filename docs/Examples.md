# Examples — 0.1 MOBA x Arena

This document provides concrete, end-to-end examples that align with your current codebase and data model. They illustrate values, flows, and data authoring choices for a 0.1 alpha without adding new code.

1) Example match timeline (single-lane, 2v2)
- 00:00 Lobby → Load Lordaeron → Warmup starts (15s)
- 00:15 Live starts; first wave scheduled at 00:30
- 00:30 Wave 1 spawns: 3 melee, 1 caster per side
- 02:00 Both towers at ~70% HP; teams ~Level 3
- 05:00 First tower destroyed; team gold swing +250 global
- 08:30 Skirmish → 2 kills → respawns staggered; second push
- 10:42 Enemy base destroyed → End screen → Return to Lobby

2) Example default numbers (tuning targets)
- Match
  - Warmup: 15s; Live hard cap for test: 20m (overtime off for 0.1)
  - Victory: enemy base HP reaches 0
- Waves
  - Interval: 30s; Composition: 3 melee (HP 300, DMG 18), 1 caster (HP 220, DMG 26)
  - Waypoint spacing: 12–20m; minion aggro radius: 18m
- Towers
  - Range: 20m; Attack Rate: 1.0s; Damage: 60; Projectile Speed: 40 m/s (if used)
  - Priority: minion > player; swap to player if tower takes player damage; leash back after 3s without further aggression
- Base
  - HP: 6000; No damage until allied tower down (optional for 0.1)
- Economy
  - Minion gold: melee 20g, caster 25g (last-hit only)
  - Player kill: 200g; Assist: split 150g evenly among up to 4 assisters
  - Tower kill: 250g global; Base kill: 300g global
- XP
  - Melee: 40xp; Caster: 50xp; Kill: 150xp + 25xp per assist (within 20m)
  - Level curve (simple): Level N requires 150 × N XP; per-level stat bumps via SO table
- Respawn & Recall
  - Respawn time: 10s + 1s × floor(elapsed_minutes)
  - Recall cast: 6s; cancel on move or damage; teleports to base

3) Example respawn schedule
- 0–0:59 → 10s; 1–1:59 → 11s; 5–5:59 → 15s; 10–10:59 → 20s (clamp at 25s for 0.1)

Illustrative formula (not code to commit):
```csharp path=null start=null
// t: elapsed match time in seconds
int RespawnSeconds(float t) {
  int minutes = Mathf.FloorToInt(t / 60f);
  return Mathf.Clamp(10 + minutes, 10, 25);
}
```

4) Example economy distribution
- Scenario: Ally A last-hits a caster minion, Allies B and C are within 20m
  - Gold: +25g to A only
  - XP: +50xp split as proximity XP: A/B/C all receive full XP (for 0.1 simplicity)
- Scenario: Player kill with 2 assisters
  - Killer gets +200g; Assisters split +150g → +75g each
  - XP: +150xp to killer; +25xp to each assister

5) Example tower aggro rules
- Default target: nearest enemy minion in range
- If tower takes damage from a player, switch target to that player and persist for 3s since last player damage, then re-evaluate
- If no valid target for >2s, return to Idle and poll at 5Hz

6) Example MatchState payload (replication)
- Phase: Warmup/Live/End
- TimeMs: server clock for match time
- TeamScores: [ally_kills, enemy_kills]
- TowerHp: [ally_tower_hp, enemy_tower_hp]
- BaseHp: [ally_base_hp, enemy_base_hp]

Illustrative snapshot struct (client-side view):
```csharp path=null start=null
public struct MatchStateSnapshot {
  public byte Phase;        // 0=Warmup,1=Live,2=End
  public uint TimeMs;       // server time since match start
  public ushort AllyTowerHp, EnemyTowerHp;
  public ushort AllyBaseHp,  EnemyBaseHp;
  public ushort AllyKills,   EnemyKills;
}
```

7) Example end-of-match flow
- On base death (server):
  - Freeze inputs: ignore movement/cast intents
  - Stop waves; disable tower AI; suppress new economy events
  - Emit End event with winning team → clients open End screen
  - After 10s or on user dismiss, return to Lobby scene

8) Example HUD layout (textual)
- Top center: Match timer (mm:ss) | Ally Kills — Enemy Kills | Tower HP bars
- Bottom left: Gold (icon + value) | Level bar | Recall button (grayed when in combat)
- Right side: Killfeed (most recent 3 entries)
- Tab: Scoreboard overlay (players, K/D/A, CS, gold, items)
