# Examples — Performance Tuning (Unity 2019 Built-in + Bolt)

General
- Keep total active minions bounded (e.g., ≤ 30 with 6 players)
- Pool VFX/projectiles; reuse particle systems where possible
- Prefer simple shaders for minions/structures; keep draw calls low

Physics & movement
- Use layer masks to avoid unnecessary raycasts; batch ground checks
- Fixed Timestep: 0.02 (50 Hz) is fine; avoid excessive physics queries per frame

Rendering
- Disable expensive camera effects; verify GraphicsSettings for minimal always-included shaders
- Use static batching for structures; dynamic batching for small props if beneficial

Networking
- Scope entities aggressively; adjust send rate lower for AI
- Keep MatchState payload small; send on change or low frequency

Profiling
- Use the Memory Profiler and built-in Profiler in a Development Build
- Track GC allocations per wave spawn; pre-allocate lists where possible
