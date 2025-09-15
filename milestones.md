╔════════════════════════════════════════════════════════════════════╗
║ Warcraft Arena Early Access Milestones                             ║
╚════════════════════════════════════════════════════════════════════╝

╔════════════════════════════════════════════════════════════════════╗
║ M0 ▸ Prototype Audit & Roadmap (Weeks 1-2)                         ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Audit prototype scope versus early-access goals across combat, content, online stack, and tooling.
┃ • Surface networking, scriptable data, and automation tech debt before heavy feature work.
┃ • Define KPIs/analytics for the arena PvP/PvE loop (match completion, ability usage, queue time, retention).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `docs/Design-Pillars.md`, `docs/PRD.md`, `docs/Examples.md` for product vision.
┃ • `docs/File-By-File.md` plus `Arena.Core.csproj` / `Arena.Client.csproj` assembly mapping.
┃ • `Assets/Scripts/Workflow/GameManager.cs` (`ScriptableContainer` registration flow).
┃ • `Assets/Settings/Balance/Default Balance.asset` and `Assets/Settings/Balance/Maps/*.asset` for roster/map scope.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Feature audit spreadsheet by subsystem (Movement, Spells, AI, Scenario, UI/HUD, Multiplayer, Live Ops).
┃ 2. Risk-ranked tech-debt backlog referencing owning assemblies (`Arena.Core`, `Arena.Client`, `Arena.Server`).
┃ 3. Analytics/observability plan defining `GameEvents` hooks and payload schema.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Enumerate every `ScriptableReference` in `Common.ScriptableContainer`—missing registrations block content loads.
┃ • Confirm `GameManager.CreateWorld` raises `GameEvents.WorldStateChanged` for lifecycle dashboards.
┃ • Capture manual build/replay pain points (e.g., `Library/` rebuilds) and schedule automation budget.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • EA scope, risks, and dependencies ratified by stakeholders.
┃ • Telemetry requirements approved and scheduled for M7 delivery.
┃ • Shared roadmap published (Confluence/Notion) with weekly checkpoints.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M1 ▸ Combat Core Upgrade (Weeks 3-8)                               ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Lock deterministic movement, targeting, hit registration, and animation sync for PvP fairness.
┃ • Expand ability execution to support six actives plus passives, procs, and combo tracking per hero.
┃ • Build combat instrumentation for damage, misses, and GCD usage to inform balance.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Scripts/Core/Movement/Controller/WarcraftCharacterController.cs` for movement input/state.
┃ • `Assets/Scripts/Core/Entity/Unit/Controllers/Unit.SpellController.cs` & `Assets/Scripts/Core/Spells/Spell.cs`.
┃ • `Assets/Scripts/Core/Spells/SpellManager.cs` (server spell lifecycle).
┃ • `Assets/Scripts/Core/AI/Core/*` for PvE state machines.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Server-authoritative movement/hit validation with client prediction smoothing (review Bolt flow).
┃ 2. Spell pipeline audit covering cooldowns, costs, combo points, aura interactions for six-skill kits.
┃ 3. Combat logging/visual debug overlays (e.g., listeners on `GameEvents.ServerDamageDone`).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Keep authoritative movement on the server; only predict when `UpdateMovementControl` grants control.
┃ • Route all new spells through `SpellCast.ValidateCast` to respect cooldown, range, and state checks.
┃ • Use the movement handling pattern below for strafing/backpedal penalties.
┃ • Profile `SpellManager.DoUpdate` in 12-player + AI scenarios; offload heavy work as needed.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```csharp
                inputVelocity.Normalize();

                // slow down when moving backward
                if (inputVelocity.z < 0)
                    inputVelocity *= 0.3f;

                if (shouldJump && unit.IsMovementBlocked)
                    shouldJump = false;

                Vector3 rawInputVelocity = Vector3.zero;

                if (!unit.IsAlive)
                    inputVelocity = Vector3.zero;
                else if (!unit.HasMovementFlag(MovementFlags.Flying))
                {
                    // check roots and apply final move speed
                    inputVelocity *= unit.IsMovementBlocked ? 0 : unit.RunSpeed;

                    if (shouldJump)
                    {
                        unit.Motion.Jumping = true;
                        inputVelocity = new Vector3(inputVelocity.x, controllerDefinition.JumpSpeed, inputVelocity.z);
                    }

                    rawInputVelocity = inputVelocity;
                    inputVelocity = transform.TransformDirection(inputVelocity);
                }
                else
                    inputVelocity = Vector3.zero;

                bool movingRight = rawInputVelocity.x > 0;
                bool movingLeft = rawInputVelocity.x < 0;

                if (movingRight)
                {
                    unit.SetMovementFlag(MovementFlags.StrafeLeft, false);
                    unit.SetMovementFlag(MovementFlags.StrafeRight, true);
                }
                else if (movingLeft)
                {
                    unit.SetMovementFlag(MovementFlags.StrafeRight, false);
                    unit.SetMovementFlag(MovementFlags.StrafeLeft, true);
                }
                else
                    unit.SetMovementFlag(MovementFlags.StrafeRight | MovementFlags.StrafeLeft, false);

                unit.SetMovementFlag(MovementFlags.Backward, rawInputVelocity.z < 0);
                unit.SetMovementFlag(MovementFlags.Forward, rawInputVelocity.z > 0);
```

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • PvP duel captures show consistent hits across server/client replays.
┃ • Designers hotload ScriptableObject tweaks; combat logs verify outcomes.
┃ • Playmode/editmode tests cover invalid targets, combo costs, immunities.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M2 ▸ Hero & Loadout Systems (Weeks 6-12)                           ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Finalize hero archetypes with six marquee actives plus signature passives.
┃ • Build loadout management UI/data for skills, talents, and gear modifiers.
┃ • Establish progression scaffolding (account XP, hero mastery, PvE drops, currencies).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Settings/Balance/Classes/Class Info - *.asset` and `ClassInfo.cs`.
┃ • `Assets/Scripts/Core/Entity/Player/Controllers/Player.SpellController.cs` (runtime spell wiring).
┃ • `Assets/Scripts/Client/Input/Action Bars/ActionBarSettings*.cs` for presets and persistence.
┃ • `Assets/Scripts/Core/Entity/Unit/Unit.cs` (attributes/combo points, reward plumbing).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Six-slot active kit with curated passives per hero (`ClassInfo.ClassSpells`).
┃ 2. Loadout editor with controller/KB-M parity; serializes to `ActionBarSettings` and updates HUD live.
┃ 3. Progression schema (ScriptableObjects + server stubs) covering XP, currencies, crafting inputs.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Keep canonical spell lists in `ClassInfo`; `Player.SpellController` handles passives automatically.
┃ • Extend `ActionBarSettings` assets under `Assets/Settings/Balance/Action Bars` for six slots per class.
┃ • Mirror progression state to `Unit.Attributes` for UI while storing authoritative data server-side.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```csharp
            public void AddClassSpells(ClassInfo classInfo)
            {
                appliedClass = classInfo;

                for (int i = 0; i < classInfo.ClassSpells.Count; i++)
                {
                    SpellInfo classSpell = classInfo.ClassSpells[i];

                    knownSpells.Add(classSpell);

                    if (classSpell.IsPassive)
                        player.Spells.TriggerSpell(classSpell, player);
                }
            }
```

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Loadout changes in lobby propagate into matches instantly.
┃ • Mid-match class swaps refresh spells/action bars without stale passives or buffs.
┃ • Progression placeholders (XP gain, reward UI) operate in offline harness.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M3 ▸ Content Authoring Pipeline (Weeks 8-16)                       ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Give designers repeatable workflows for spells, auras, NPCs, and encounters via ScriptableObjects.
┃ • Version control balance data (Git + sheets) with automated validation.
┃ • Document VFX/SFX hooks, animation events, localization tagging for new content.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Scripts/Common/Scriptables/*` (`ScriptableUniqueInfo` + container behavior).
┃ • `Assets/Settings/Balance/Spells`, `/Targeting`, `/Auras` for authoring assets.
┃ • `docs/Examples-Spells-and-Auras.md`, `docs/Examples-UI-and-HUD-Wireup.md` for templates.
┃ • `Assets/Scripts/Core/Spells/Spell Effects/*.cs` and `Spell Auras/*.cs` for runtime logic.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Authoring checklist/templates covering SpellInfo, AuraInfo, ConditionalModifiers, targeting assets.
┃ 2. Export/import tooling (Unity menu or CLI) validating IDs, dependencies, missing assets on submission.
┃ 3. Automated smoke tests (spawn hero, cast new skill, verify damage/heal output) feeding reviews.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Leverage data-driven damage helpers (see sample below) instead of bespoke scripts.
┃ • Let `ScriptableUniqueInfo` manage IDs—never hand-edit serialized `id` fields.
┃ • Treat spreadsheets (Sheets/Airtable) as the source of truth; regenerate `.asset` files via scripted import.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```csharp
    public class EffectSchoolDamage : SpellEffectInfo
    {
        internal int CalculateSpellDamage(SpellInfo spellInfo, int effectIndex, Unit caster = null, Spell spell = null)
        {
            int rolledValue = RandomUtils.Next(baseValue, (int)(baseValue + baseVariance));

            if (usesComboPoints && spell != null && spell.ConsumedComboPoints > 0)
                rolledValue *= spell.ConsumedComboPoints;

            float baseDamage = 0;

            switch (calculationType)
            {
                case SpellDamageCalculationType.Direct:
                    baseDamage = rolledValue + additionalValue;
                    break;
                case SpellDamageCalculationType.SpellPowerPercent:
                    if (caster == null)
                        break;
                    baseDamage = additionalValue + caster.SpellPower.ApplyPercentage(rolledValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationType), $"Unknown type: {calculationType}");
            }

            if (caster != null)
                baseDamage = caster.Spells.ApplyEffectModifiers(spellInfo, baseDamage);

            return (int)baseDamage;
        }
    }
```

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Designers author new spell + aura combos end-to-end without engineering assistance.
┃ • CI validation fails on missing dependencies (e.g., SpellInfo lacking SpellEffectInfo).
┃ • Documentation updated with capture of the export/import workflow.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M4 ▸ Arena & Encounter Expansion (Weeks 10-18)                     ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Deliver at least three arena variants (biomes, layout changes, hazards).
┃ • Author PvE wave/boss mechanics that blend PvP and PvE objectives.
┃ • Build designer-friendly encounter scripting for teleports, spawns, modifiers.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Scenes/Lordaeron*.unity` and `Assets/Maps/Lordaeron` for baseline layout.
┃ • `Assets/Scripts/Core/Balance/World/MapSettings.cs` (spawn/team config).
┃ • `Assets/Scripts/Core/Scenario/*` for scenario behaviors.
┃ • `Assets/Scripts/Core/Map/Map.cs` & `MapManager.cs` for runtime management.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Three arenas documented with spawn points, hazards, LOS blockers.
┃ 2. Scenario scripts for PvE waves/bosses using `ScenarioAction` with difficulty tuning tables.
┃ 3. Dynamic modifiers (weather, resource multipliers) toggleable via live ops tooling.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Follow the scenario spawn pattern below for creatures and bosses.
┃ • Group scenario actions under `MapSettings`; use "Collect scenario actions" context tool.
┃ • Validate navmeshes/bounding boxes per arena; `MapSettings.BoundingBox` exposes extents.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```csharp
    public class SpawnCreature : ScenarioAction
    {
        private void OnServerLaunched()
        {
            Creature creature = World.UnitManager.Create<Creature>(BoltPrefabs.Creature, new Creature.CreateToken
            {
                Position = customSpawnSettings.SpawnPoint.position,
                Rotation = customSpawnSettings.SpawnPoint.rotation,
                OriginalAIInfoId = customSpawnSettings.UnitInfoAI?.Id ?? 0,
                DeathState = DeathState.Alive,
                FreeForAll = true,
                ClassType = ClassType.Warrior,
                ModelId = creatureInfo.ModelId,
                OriginalModelId = creatureInfo.ModelId,
                FactionId = Balance.DefaultFaction.FactionId,
                CreatureInfoId = creatureInfo.Id,
                CustomName = string.IsNullOrEmpty(customSpawnSettings.CustomNameId)
                    ? creatureInfo.CreatureName
                    : customSpawnSettings.CustomNameId,
                Scale = customSpawnSettings.CustomScale
            });

            creature.BoltEntity.TakeControl();
        }
    }
```

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Matchmaking rotates across arenas without manual intervention.
┃ • PvE waves run in dedicated server builds with logged timings.
┃ • Designers toggle modifiers/spawn sets through ScriptableObject configs.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M5 ▸ Visual & Audio Identity (Weeks 12-20)                         ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Establish art direction, readability rules, and performance budgets.
┃ • Produce animation sets for locomotion, abilities, reactions, deaths, emotes.
┃ • Deliver VFX/SFX packages per ability tier with accessibility-safe palettes and mix levels.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Graphics/*` for icons, shaders, VFX prefabs.
┃ • `Assets/Sound/*` for audio assets and middleware integration points.
┃ • `Assets/Scripts/Client/UI/Spell Overlay/*` for proc/readiness visuals.
┃ • `Assets/Animations/*` plus animator controllers used by `UnitStateMachine` behaviors.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Visual style guide + color language for combat readability.
┃ 2. Animation packs wired into `UnitStateMachine` controllers with correct import settings.
┃ 3. Audio system plan (Wwise/FMOD/Unity Audio) with ducking, mix states, latency handling.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Map procs to overlays via `SpellOverlaySettings` and reusable prefabs.
┃ • Profile VFX/particles with six-player scenarios to meet FPS targets.
┃ • Build colorblind-safe palettes and expose toggles (hooked in M8).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Internal art review approves readability from gameplay/spectator cameras.
┃ • Animation blending aligns with movement/ability states (no sliding/popping).
┃ • Audio cues fail safely (silence > wrong sound) with warning logs.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M6 ▸ Meta, UX, and Social (Weeks 14-22)                            ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Ship lobby, hero select, loadout management, quest log, progression flows.
┃ • Implement friends, party, and guild scaffolding (even if feature-light for EA).
┃ • Add onboarding/tutorial missions covering controls, rotations, PvE beats.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Scripts/Client/UI/Screens/*` (`BattleScreen`, `LobbyScreen`).
┃ • `Assets/Scripts/Client/UI/Panels/Battle/*` (HUD, action bars, cast frames, buffs).
┃ • `Assets/Scripts/Client/Input/Action Bars/*` (presets & persistence).
┃ • `docs/Examples-UI-and-HUD-Wireup.md` for wiring reference.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Lobby/hero select with loadout editing & preview, persisting through `ActionBarSettings` (future backend sync).
┃ 2. Quest/progression UI referencing server data with basic journal/explanations.
┃ 3. Social MVP (friend invites, party chat, Discord CTA) integrated into UI states.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Refresh HUD when class changes using the action bar pattern below.
┃ • Register/unregister UI canvases via `InterfaceContainer` to prevent lingering listeners.
┃ • Budget time for controller navigation graphs and accessibility (font size, contrast) ahead of M8.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```csharp
        public void ModifyContent(ClassType classType)
        {
            ActionBarSettings appliedSettings = container.SettingsByClassSlot.TryGetValue((classType, actionBarSettings.SlotId), out ActionBarSettings classSettings)
                ? classSettings
                : actionBarSettings;

            bool hasContent = classType == ClassType.Warrior
                ? actionBarSettings.SlotId == 1 && appliedSettings.DefaultPresets.Count > 0
                : appliedSettings.DefaultPresets.Count > 0;

            gameObject.SetActive(hasContent);

            if (hasContent)
            {
                for (int i = 0; i < buttonSlots.Count; i++)
                    buttonSlots[i].ButtonContent.UpdateContent(appliedSettings.ActiveButtonPresets[i]);
            }
        }
```

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • End-to-end flow: party creation → loadout edit → queue → match → return to lobby with correct visuals.
┃ • Tutorials track completion and feed analytics for drop-off.
┃ • Social features degrade gracefully offline with clear messaging.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M7 ▸ Online Services & Live Ops Prep (Weeks 16-24)                 ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Harden matchmaking, session management, inventory, and analytics services.
┃ • Integrate anti-cheat, authoritative validation, and secure savepaths.
┃ • Automate CI/CD for dedicated servers, clients, and Steam depot publishing.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Assets/Scripts/Core/Multiplayer/PhotonBoltController.cs` (network state machine).
┃ • `Arena.Workflow.*` assemblies (client vs dedicated startup routines).
┃ • `docs/Examples-Networking-and-Regions.md` for environment planning.
┃ • `PowerShell/` build scripts (if present) for packaging automation.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Scalable backend plan (Photon Bolt + ancillary services) with load/perf test results.
┃ 2. Anti-cheat & validation hooks (server cross-checks ability triggers, inventory writes) plus monitoring dashboards.
┃ 3. Automated pipeline producing nightly QA builds, weekly public tests, Steam depots.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Toggle Bolt listeners via `PhotonBoltController.SetListeners` per networking mode to avoid duplicates.
┃ • Emit `GameEvents.GameMapLoaded` and connection events to feed analytics dashboards.
┃ • Prepare incident response runbooks (rollback, feature flags) using event system + Scriptable references.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Dedicated server load tests (bot-driven) run 1+ hour without leaks/desyncs.
┃ • CI outputs versioned builds, publishes Steam branches, and posts notifications.
┃ • Live ops dashboard tracks session list, server CPU, error spikes.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M8 ▸ Quality Pass & Accessibility (Weeks 20-26)                    ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Execute full QA plan (functional, regression, network, soak, compliance).
┃ • Implement accessibility toggles (remappable inputs, colorblind filters, text/voice options, subtitles).
┃ • Optimize CPU/GPU/memory for target hardware and server concurrency.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `docs/Test-Plan.md`, `docs/Examples-Testing-and-Playtest-Checklists.md`.
┃ • `Assets/Scripts/Client/UI/Spell Overlay/*`, action bar, options UI.
┃ • Profiling entry points in `GameManager.Update`, `SpellManager.DoUpdate`.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. QA suite (automated smoke/regression, manual scripts, bug triage cadence).
┃ 2. Accessibility options menu persisting preferences (PlayerPrefs/backend) and applying at runtime.
┃ 3. Performance optimization report with before/after metrics meeting budgets.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Automate accessibility toggle tests (e.g., colorblind filters adjust shader globals).
┃ • Use Unity Profiler + RenderDoc to spot expensive VFX/physics (AoE spells, large waves).
┃ • Provide fallback bots (`World.UnitManager.Create<Player>` with AI) for low-population matchmaking.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • QA signoff with burndown tracking; remaining bugs triaged post-launch if low risk.
┃ • Accessibility checklist (WCAG-inspired) satisfied and documented.
┃ • Performance budgets met for min-spec PC and dedicated server tick rate.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ M9 ▸ Steam Early Access Launch Readiness (Weeks 24-28)             ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Prepare Steam store presence, marketing assets, and launch communications.
┃ • Integrate Steamworks features (achievements, cloud saves, beta branches).
┃ • Run marketing beats (closed alpha, open beta weekend, creator program, moderation plan).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `Warcraft-Arena-Unity-master.sln` & build scripts for packaging.
┃ • `Arena.Workflow.Standard` vs `Arena.Workflow.Dedicated` for entry points.
┃ • Steamworks configuration (external) referenced by deployment scripts.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Deliverables ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1. Steam store assets (capsules, trailer, screenshots) with PvP/PvE hybrid messaging.
┃ 2. Verified Steamworks integration (achievements, cloud saves, beta branches).
┃ 3. Launch plan covering marketing beats, creator outreach, community moderation, roadmap teaser.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Implementation Notes & Best Practices ━━━━━━━━━━━━━━━━━━━━━━━
┃ • Automate depot uploads (SteamCMD) in CI with build IDs tied to commits.
┃ • Use feature flags (Scriptable refs or remote config) for launch-day surprises.
┃ • Align patch notes, FAQ, and support workflows with community team.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Exit Criteria ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Steam review (assets/compliance) approved; release date announced.
┃ • Launch candidate build (client + dedicated) passes smoke checklist.
┃ • Marketing/community tooling ready (Discord bots, issue templates, FAQ).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ Content & Live Pipeline Post-EA                                   ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Focus ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Sustain cadence of balance patches, new heroes, gear, arenas with live metrics.
┃ • Run seasonal PvE/PvEvP events and cosmetics without code hotfixes.
┃ • Keep authoring tools, analytics, and pipelines healthy for rapid iteration.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Repo Touchpoints ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • `BalanceDefinition`, `BalanceReference`, `SpellInfoContainer` for balance/content drops.
┃ • `ActionBarSettings`, `ClassInfo` assets for new hero kits.
┃ • `docs/Examples-Spells-and-Auras.md`, `docs/Examples-Testing-and-Playtest-Checklists.md` as ongoing playbook.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Live Cadence Guidelines ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Fortnightly balance patches tuning `SpellInfo`, `AuraInfo`, AI behaviors.
┃ • Monthly drops (new hero with six skills, loadout items, arena variant) validated via smoke tests.
┃ • Seasonal PvE events introducing limited-time factions, modifiers, cosmetics through ScenarioActions + balance data.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Operational Best Practices ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Maintain telemetry dashboards for damage breakdowns, ability usage, queue health; feed design sprints.
┃ • Expand tooling (procedural encounter generator, combat telemetry overlays) to speed iteration.
┃ • Monitor crash logs, analytics funnels, community sentiment; adapt backlog and staffing.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

╔════════════════════════════════════════════════════════════════════╗
║ Code Audit ▸ Unity 2019 → 2022 Freeze                              ║
╚════════════════════════════════════════════════════════════════════╝

┏━━ Executive Summary ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Status: Unity 2022 build runs with Photon Bolt networking, solid gameplay core, and workflow variants.
┃ • Focus Areas: Networking lifecycle hacks, threading model, hard-coded map IDs, UI event disposal, versioning, perf guards, token handling.
┃ • Recommendation: Stay on Unity 2022 LTS for this release; defer Unity 6 until blockers resolved / Photon support verified.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Critical Findings (Must Fix) ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • MapUpdater.Wait ineffective; Map/MapGrid use Unity APIs—keep single-threaded or fully refactor with real waits.
┃ • Hard-coded `MapId = 1` across listeners/world; pass MapId via `ServerRoomToken` or derive from `MapSettings`.
┃ • Lobby UI leak: use `RemoveListener` on teardown instead of `AddListener`.
┃ • Networking scene hack for "Launcher"; gate on presence of `MapSettings` (gameplay scene) instead of string compare.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ High Priority Findings ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Network version string hard-coded (`PhotonBoltController.Version`); centralize via `BuildInfo.NetworkVersion`.
┃ • Runtime debug drawing in `MapGrid`; guard behind editor/debug defines.
┃ • Spawn point safety checks; default to `DefaultSpawnPoint` when missing.
┃ • Token validation/sanitization (player names, preferred class, connect rate limiting).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Medium Priority Findings ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Replace `GameObject.FindGameObjectWithTag` in `PhotonBoltReference` with serialized references.
┃ • Swap `Mutex` in `MapManager` for in-process locking (`lock`, `ReaderWriterLockSlim`).
┃ • Debounce PlayerPrefs writes in lobby; flush on panel hide.
┃ • Add chat relay safeguards (length, profanity filter hook, cooldowns).
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Low Priority Findings ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Normalize naming (e.g., `PrefferedClass` → `PreferredClass`).
┃ • Provide GUID fallback when `SystemInfo.deviceUniqueIdentifier` unsupported.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Best Practice Updates ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Scene handling: detect gameplay via `MapSettings`; pass MapId through tokens for spawns/visibility.
┃ • Versioning: single source for build/network version (CI-friendly).
┃ • Threading: keep gameplay on main thread unless Unity APIs removed; marshal background results back safely.
┃ • Debug controls: wrap heavy logs/draws behind defines/toggles.
┃ • Event lifecycle: pair every subscription with scoped removal.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Examples ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Scene load gate: use `MapSettings` detection before initializing map systems.
┃ • Spawn fallback: return `DefaultSpawnPoint` when team list empty.
┃ • Centralize network version via `BuildInfo` and reuse across tokens/controller.
┃ • Lobby cleanup: remove listeners on panel teardown.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Unity 6 Readiness & Recommendation ━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ • Stay on Unity 2022 until Photon Bolt support for Unity 6 is confirmed or migration to Fusion planned.
┃ • Pre-upgrade checklist: verify SDKs, run API Upgrader, parallel CI builds (2022 + 6000), eliminate Unity API calls on worker threads.
┃ • Go/No-Go: upgrade only after SDK checks pass and CI is green on 6000.x.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

┏━━ Next Steps ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
┃ 1) Implement critical fixes (MapId plumbing, lobby listeners, scene gate) and add `BuildInfo`.
┃ 2) Add debug guards and spawn fallbacks; sanitize tokens.
┃ 3) Decide threading direction: remove `MapUpdater` or refactor to main-thread dispatcher.
┃ 4) Profile 8–12 player scenarios; ensure no debug-induced frame spikes.
┃ 5) Reassess Unity 6 after confirming Photon Bolt support or planning Fusion migration.
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
