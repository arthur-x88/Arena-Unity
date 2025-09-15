# Acceptance Criteria — 0.1 MOBA x Arena

Gameplay
- Can host/join via Lobby and start a match on Lordaeron
- Warmup (>= 10s), then Live; waves spawn every 30s for both teams
- Towers target minions first; switch to player when attacked by player
- Last‑hit gold, XP distribution on lane proximity; level‑ups affect stats/ability ranks
- Shop purchases only possible in base radius
- Recall works; interrupts on damage/movement; returns to base
- Victory when enemy base dies; show End screen; return to Lobby without crash

Networking
- Server authoritative on economy, towers, waves, respawn
- No desync observed in gold/XP/items after 20 minutes of play
- MatchState snapshot replicated (phase/time/scores/tower HPs)

UX/HUD
- HUD shows: timer, team scores, tower status, gold, level, K/D/A
- Scoreboard shows: players, K/D/A, CS, gold, items
- End screen shows winner and basic summary

Performance
- With 6 players + typical waves (<= 30 minions active): FPS >= 60 on target PC
- No GC spikes > 20ms during wave spawns (profiling in a dev build)

Stability
- No hard errors/exceptions in logs during a 15‑minute match
- Clean return to lobby, no leaked entities on server shutdown
