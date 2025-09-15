# Warcraft Arena Early Access Milestones

## M0 — Prototype Audit & Roadmap (Weeks 1-2)
- Inventory prototype features, tech debt, and stability gaps; classify items as must-have for PvP/PvE arena core.
- Define target early access scope: hero roster size, map count, PvE enemy variety, monetization stance, and community goals.
- Produce schedule, staffing plan, and tooling requirements (design, engineering, art, audio, QA, live ops).
- Establish success metrics (retention, queue time, combat satisfaction) and analytics events needed to capture them.

## M1 — Combat Core Upgrade (Weeks 3-8)
- Refine movement, targeting, and hit registration; integrate authoritative server simulation for PvP fairness.
- Implement ability framework supporting six active skills per hero plus passive traits, cooldowns, and combo flags.
- Create enemy AI archetypes for PvE waves using behavior trees/state machines; tune for arena pacing.
- Build encounter scripting layer (waves, objectives, timers) with designer-exposed parameters.
- Add combat logging/debug visualization for designers to validate balance quickly.

## M2 — Hero & Loadout Systems (Weeks 6-12)
- Finalize hero class templates (tank, support, damage, control, hybrid) with shared stat curves and animation hooks.
- Implement loadout manager enabling players to equip six skills, talents, and gear mods per hero.
- Create unlock progression (account level, hero mastery, PvE drops) feeding currency and crafting loops.
- Add data-driven balance tables (damage, cooldown, resource cost) editable without code recompile.

## M3 — Content Authoring Pipeline (Weeks 8-16)
- Stand up versioned design database (e.g., Google Sheets + exporter, Airtable, or ScriptableObject source of truth).
- Build Unity editor tooling for skill authoring: effect composition, VFX/SFX links, animation events, localization keys.
- Create reusable prefab kits for heroes, enemies, and arena props; document assembly workflow in Confluence/Notion.
- Establish review cadence (weekly hero/skill review, bi-weekly encounter review) with checklists for QA and balance.
- Automate export/import scripts to push design changes into build and validate via automated smoke tests.

## M4 — Arena & Encounter Expansion (Weeks 10-18)
- Produce at least three arena biomes with traversal variants, line-of-sight blockers, and environmental hazards.
- Author PvE wave playlists escalating in difficulty; include elite/boss encounters with telegraphed mechanics.
- Build matchmaking buckets for PvP, PvEvP (mixed teams vs AI + rival team), and cooperative PvE survival.
- Implement dynamic event modifiers (double resource nodes, sudden death, weather effects) for replayability.

## M5 — Visual & Audio Identity (Weeks 12-20)
- Lock visual style bible; ensure hero silhouettes readable at gameplay camera distance.
- Create animation sets covering six skills per hero, locomotion, hit reactions, death, and emotes.
- Develop VFX pass for each ability tier, readability-focused color coding, and performance budgets.
- Deliver adaptive music layers and responsive combat SFX; integrate audio middleware if needed.

## M6 — Meta, UX, and Social (Weeks 14-22)
- Build lobby, hero select, loadout management, quest journal, and progression UI flows with controller + KB/M parity.
- Integrate friends list, party formation, and guild/warband scaffolding (even if some features stubbed for EA).
- Add tutorial/onboarding missions covering controls, six-skill rotation concepts, and PvE objectives.
- Implement telemetry dashboards (match outcome, damage breakdown, ability usage) for live balance monitoring.

## M7 — Online Services & Live Ops Prep (Weeks 16-24)
- Deploy scalable backend (matchmaking, inventory, progression, analytics) on staging; run load/perf tests.
- Integrate anti-cheat and server authoritative validation for ability triggers and inventory transactions.
- Create automated build pipeline (CI/CD) producing nightly QA builds, weekly public test builds, and Steam depots.
- Establish incident response runbooks, rollback procedure, and live ops tooling (GM commands, feature flags).

## M8 — Quality Pass & Accessibility (Weeks 20-26)
- Run full QA test plans (functional, regression, network, performance) with bug triage and daily burndown.
- Implement accessibility options: remappable inputs, colorblind filters for ability effects, text-to-speech hooks.
- Optimize memory/CPU/GPU budgets for target hardware; profile in live-like 6v6 + AI scenarios.
- Add fail-safe bots for backfilling matches and smoothing queue experience during low concurrency.

## M9 — Steam Early Access Launch Readiness (Weeks 24-28)
- Prepare Steam store assets (capsules, trailer, screenshots), description focused on arena PvP/PvE hybrid.
- Configure Steamworks (achievements, cloud saves, beta branches) and verify build submission pipeline.
- Plan marketing beats: closed alpha, open beta weekend, creator program, and community Discord moderation.
- Draft early access roadmap showcasing upcoming hero classes, new arenas, ranked ladder, and seasonal events.

## Content & Live Pipeline Post-EA
- Maintain fortnightly balance patches targeting ability data sheets and AI behaviors.
- Ship monthly content drops: new hero classes with six skills, themed loadout items, arena variants.
- Run seasonal PvE events introducing limited-time enemy factions and cosmetic rewards.
- Expand authoring toolkit with procedural encounter generator and combat telemetry overlays for designers.
- Continually monitor player feedback, analytics, and crash reports; adjust backlog and staffing accordingly.
