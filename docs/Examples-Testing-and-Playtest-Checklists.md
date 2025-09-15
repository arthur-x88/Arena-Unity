# Examples — Testing & Playtest Checklists

Smoke (quick)
- Host EU; 3 clients join
- Warmup → Live; waves spawn both sides
- Last-hit gold works; shop purchases in base only
- Tower aggro prioritizes minions; swap to player on provocation
- Killfeed and scoreboard update
- Victory on base death → End screen → Return to Lobby

Latency
- 100–150ms: verify cast responsiveness; no desync in gold/XP or tower HP

Performance
- Peak wave: FPS ≥ 60; no GC spikes > 20ms on spawn

Edge cases
- Disconnect mid-match: others continue; server cleans up on End
- Attempt to purchase outside base: blocked; tooltip shown
- Recall cancel on damage/move

Exploratory
- Different regions; dejitter and send rate variations
