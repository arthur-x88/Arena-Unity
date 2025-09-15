# Examples — Lane & Minions (Single-Lane 0.1)

Waypoints (lane path)
- Create empty GameObjects along the intended lane path (every 12–20m)
- Parent under an object named Lane_Waypoints with ascending order

Minion composition
- Every 30s, spawn per team: 3 melee + 1 caster
- Spawn from base transforms; assign team on spawn

Target selection (priority)
1) Enemy minion (nearest in range)
2) Enemy player (if no minions or provoked)
3) Enemy tower/base (when in range)

Aggro radius & leash
- Aggro radius: ~18m; leash to lane if kited too far; retarget every 0.5s

Rewards
- Last-hit gold; proximity XP to allies within 20m

Performance tips
- Cap total active minions (e.g., ≤ 30) for 6 players
- Use scoping to limit replication to nearby clients

Authoring checklist (doc-only)
- Add LaneDefinition SO with waypoint transforms
- Add MinionDefinition SO with HP/DMG/speed/gold/xp values
- Connect WaveManager to LaneDefinition and spawn points per team
