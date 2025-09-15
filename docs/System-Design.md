# System Design — 0.1 MOBA x Arena

Architecture
- Server authority: WorldServer owns MatchManager, WaveManager, Economy, Respawn. Clients only send intents.
- Data-driven knobs: SOs for lane, structures, minions, match tuning, items, level tables.
- Networking: use Bolt for entity sync; add a light MatchStateReplicator for phase/time/scores/structure HPs.

State Machines
- MatchManager (server)
  - Lobby → Loading → Warmup → Live → End → ReturnToLobby
  - Triggers: scene loaded, warmup timer elapsed, base destroyed, timeout.
- TowerBrain
  - Idle → AcquireTarget → Attack → (retarget) → Idle; priority: minion > player unless provoked.
- MinionBrain
  - FollowLane → Acquire → Attack → (die or retarget) → FollowLane.

Flows
1) Launch
- Launcher → Lobby → select region → Host/Join → Load Lordaeron → MatchManager starts Warmup.
2) Warmup
- Players can move; no minions; shops enabled; recall allowed.
3) Live
- Waves spawn; towers active; economy/XP accrues; UI updates timer, scores, tower HPs, KDA.
4) End
- On base death; freeze inputs; show EndPanel; leave back to Lobby.

Replication
- MatchState snapshot (phase, timeMs, team scores, tower HPs) broadcast at low rate or on change.
- Entities: rely on Bolt scoping; reduce send rate for AI where possible.

Performance
- Cap active minions per lane to a safe number; despawn outside combat if needed.
- Use pooled projectiles/effects; reuse existing VFX.

Security
- Validate purchases, upgrades, gold/XP sources on server.
- Discard client-side economy messages; only accept intents.
