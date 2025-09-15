# Examples — Networking & Regions (Photon Bolt Cloud)

Region selection
- UI dropdown already calls BoltRuntimeSettings.instance.UpdateBestRegion(...)
- Persist last selection in PlayerPrefs; default to EU or closest

BoltRuntimeSettings (observed defaults in repo)
- clientSendRate: 3 (≈20 Hz), clientDejitterDelay: 6 (≈100 ms buffer)
- serverSendRate: 3 (≈20 Hz), serverDejitterDelay: 6
- scopeMode: 1; disableDejitterBuffer: 0; useNetworkSimulation: 0

Recommended for 0.1 (start conservative)
- Keep 20 Hz, dejitter 5–6 frames; revisit after playtests
- Increase to 30 Hz (sendRate=2) only if necessary and perf allows

NAT/Cloud
- photonUsePunch: 1 (NAT punch enabled)
- photonAppId: configured in BoltRuntimeSettings (keep secret)

Latency display (optional doc-only)
- Add a tiny ping HUD for internal testing using BoltConnection.PingNetwork/PingAliased

Bandwidth hygiene
- Prefer scoping to cull distant minions/structures from clients
- Lower send rates or update frequencies for AI entities vs. players
