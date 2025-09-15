# Examples — Match State & Replication

Payload fields (suggested minimal)
- Phase (byte): Warmup=0, Live=1, End=2
- TimeMs (uint): server time since match start
- TeamScores (ushort,ushort): allyKills, enemyKills
- TowerHp (ushort,ushort): allyTowerHp, enemyTowerHp (0–10000 normalized)
- BaseHp (ushort,ushort): allyBaseHp, enemyBaseHp (0–10000 normalized)

Example snapshot
```json path=null start=null
{
  "phase": 1,
  "timeMs": 475000,
  "allyKills": 4,
  "enemyKills": 3,
  "allyTowerHp": 6800,
  "enemyTowerHp": 10000,
  "allyBaseHp": 10000,
  "enemyBaseHp": 10000
}
```

Update cadence
- Send at low frequency (e.g., 2–4 Hz) or on change; keep under MTU.

Authority
- Server is source of truth; clients render HUD from most recent snapshot.

Frame/time alignment
- Use BoltNetwork.ServerFrame for converting cooldown/GCD and history timelines (as already implemented in SpellHistory and others)

Interest management
- Do not replicate minions/structures to distant clients; rely on scope to reduce bandwidth.
