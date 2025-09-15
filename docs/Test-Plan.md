# Test Plan — 0.1 MOBA x Arena

Test Matrix
- Platforms: Windows, macOS, Linux desktop
- Network: LAN and Internet via Photon Cloud
- Players: 2v2 and 3v3

Smoke Tests (per build)
1) Host/Join
- Host in EU; join from 1–5 clients; see sessions in Lobby; join succeeds
2) Scene Load & Warmup
- All clients load Lordaeron; Warmup timer visible; inputs responsive
3) Waves & Towers
- Waves spawn both sides every 30s; towers engage minions first; aggro swap when attacked by player
4) Economy
- Last‑hit grants gold; assists credited; proximity XP levels players
5) Shop & Items
- Buy within base radius; items apply passives (auras/modifiers)
6) Recall & Respawn
- Recall returns to base; interrupt on move/damage; respawn timer increases over time
7) Victory & Return
- Destroy enemy base; End screen shows; return to Lobby cleanly

Latency/Replication
- With 100–150ms ping: spell casts and minion combat remain responsive; no desync in gold/XP and tower HP
- MatchState snapshot reflects phase/time/scores correctly after 15 minutes

Performance
- Monitor FPS during peak waves; target >= 60; profile GC during wave spawns

Negative Tests
- Attempt purchases outside base: blocked with tooltip
- Attempt to join full match: receive proper error; Lobby remains functional
- Disconnect during match: remaining clients continue; server cleans up on end
