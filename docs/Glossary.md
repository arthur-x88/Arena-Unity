# Glossary — Warcraft Arena 0.1

- SpellInfo: ScriptableObject defining a spell (costs, cooldowns, ranges, effects, flags).
- SpellEffectInfo: ScriptableObject representing a spell effect (damage, heal, apply aura, teleport, trigger spell).
- SpellTargeting: ScriptableObject defining how targets are selected (single, area, cone).
- AuraInfo: ScriptableObject defining an aura (duration, stacks, attributes, interrupt flags, list of AuraEffectInfo).
- AuraEffectInfo: ScriptableObject defining an effect applied by an aura (stun, silence, pacify, periodic, modifiers, immunities, stealth/invisibility, speed, stats).
- ClassInfo: ScriptableObject defining class availability, power types, and class spell list.
- BalanceReference: ScriptableReference aggregating maps, spells, auras, classes, factions, AI into dictionaries for fast lookup.
- WaveManager (doc): Server-side system to spawn and manage minion waves.
- TowerBrain/BaseCore (doc): Server-side logic for towers and base behavior and HP.
- PlayerEconomy (doc): Server-side gold/XP/KDA/CS tracking for scoreboard and HUD.
- MatchState: Compact replicated snapshot (phase/time/scores/structure HP) for HUD.
- Scoping: Limiting network replication of entities to relevant clients to reduce bandwidth.
- GCD (Global Cooldown): Common cooldown period applied after casting many spells; handled in SpellHistory.
- Charges: Per-spell charge system; handled in SpellHistory as SpellChargeCooldowns.
