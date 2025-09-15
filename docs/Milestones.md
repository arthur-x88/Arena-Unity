# Milestones — 0.1 MOBA x Arena

M1 — Match skeleton (2–3 days)
- MatchManager (states, timer, victory)
- TeamManager (assignment, spawns)
- End → return to Lobby
- Minimal HUD (timer, phase)

M2 — Lane & AI (4–6 days)
- LaneDefinition + StructureDefinition SOs
- WaveManager + MinionBrain; basic combat
- TowerBrain + BaseCore; victory on base death
- Prefabs: Minion, Tower, Base wired to Bolt prefab DB

M3 — Economy & Respawn (3–5 days)
- PlayerEconomy (gold, KDA, CS)
- XP/level system; simple scaling
- Respawn timers; recall spell

M4 — Items & HUD (3–5 days)
- ItemDefinition SOs; Inventory; ShopPanel (base-only)
- HUD: gold, level, K/D/A; Scoreboard; EndPanel

M5 — QA & polish (2–3 days)
- Acceptance criteria run; perf pass on waves
- Network checks (150ms)
- Known issues list

Notes
- Keep all tunables in SOs under Settings/Balance/* for iteration.
- Defer bells/whistles (FOW, actives, multi-lane) to 0.2+.
