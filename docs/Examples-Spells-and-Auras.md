# Examples — Spells and Auras (Authoring with Existing Systems)

This guide shows how to author new spells and auras using your current data-driven pipeline.

Key types (already in codebase)
- SpellInfo (ScriptableObject): defines cast flags, costs, ranges, damage class, mechanics, effects, etc.
- SpellEffectInfo (ScriptableObject): plug-in effects such as EffectSchoolDamage, EffectApplyAura, EffectTeleportDirect, etc.
- SpellTargeting (ScriptableObject): Single, Area, Cone.
- AuraInfo (ScriptableObject): duration, stacks, attributes, interrupt flags, and a list of AuraEffectInfo.
- AuraEffectInfo (ScriptableObject): effects such as Stun, Silence, Pacify, Invisibility, Haste, Periodic Damage/Healing, Modifiers, Immunities.

Where to place assets
- Spells: Assets/Settings/Balance/Spells
- Auras:  Assets/Settings/Balance/Spells (or a separate Auras folder if you prefer)
- Visuals: Assets/Settings/Visuals/Effect Visuals Aura and Effect Visuals Spell
- Icons:   Assets/Graphics/Spell Icons

Authoring checklist — new damage spell (Arcane Bolt)
1) Create SpellInfo asset: Assets/Settings/Balance/Spells/Arcane Bolt Spell.asset
2) Configure fields (inspector targets listed from SpellInfo.cs):
   - ExplicitTargetType: Target
   - DamageClass: Magic
   - SpellDispel: None
   - Mechanic: None
   - ExplicitCastTargets: UnitEnemy
   - SchoolMask: Arcane
   - PreventionType: None
   - Attributes: 0 (not Passive)
   - AttributesExtra: SingleTargetSpell
   - AttributesCustom: 0
   - RangedFlags: Ranged (not Melee)
   - InterruptFlags: 0
   - CooldownTime: 700 ms
   - CategoryCooldownTime: 0
   - GlobalCooldownTime: 1500 ms
   - CastTime: 1500 ms (1.5 s)
   - MinCastTime: 0
   - Charges: 0
   - MinRangeHostile: 0
   - MaxRangeHostile: 30
   - Speed: 40 (projectile visuals)
3) Add PowerCosts: one SpellPowerCostInfo (Mana, 30)
4) Add Effects:
   - EffectSchoolDamage (base=65, variance=10, calc=Direct)
   - Optional: EffectApplyAura (On-Hit slow)
5) Targeting: SpellTargetingSingle (default)
6) Visuals: hook projectile VFX (Assets/Graphics/Spells/*) and an impact effect.

Illustrative inspector values (not code):
```yaml path=null start=null
SpellInfo:
  ExplicitTargetType: Target
  DamageClass: Magic
  ExplicitCastTargets: UnitEnemy
  SchoolMask: Arcane
  AttributesExtra: SingleTargetSpell
  Cooldowns:
    CooldownTime: 700
    GlobalCooldownTime: 1500
  Cast:
    CastTime: 1500
  Range:
    MaxRangeHostile: 30
  PowerCosts:
    - { SpellPowerType: Mana, PowerCost: 30, PowerCostPercentage: 0 }
  Effects:
    - EffectSchoolDamage { baseValue: 65, baseVariance: 10, calculationType: Direct }
```

Authoring checklist — crowd control aura (Cyclone-like)
1) Create AuraInfo asset: Assets/Settings/Balance/Spells/Cyclone Aura.asset
2) Set:
   - Duration: 6000 ms; MaxDuration: 6000; MaxStack: 1
   - Attributes: Negative
   - InterruptFlags: CombinedDamageTaken (as needed)
   - AuraEffects: AuraEffectInfoConfuse (mechanics: Confuse)
3) Create SpellInfo: Cyclone Spell.asset
   - ExplicitTargetType: Target; ExplicitCastTargets: UnitEnemy
   - Cooldown: 45s; CastTime: 1.7s; Range: 30
   - Effects: EffectApplyAura → Cyclone Aura

Authoring checklist — team buff aura (Arcane Intellect-like)
1) AuraInfo: +Intellect (or generic Spell Modifier)
   - Effects: AuraEffectInfoSpellModifier (ModifyStatPercent / SpellPower etc.)
   - Attributes: Positive
   - Duration: 180000 ms (3 min)
2) SpellInfo: Self-cast or ally-target buff
   - ExplicitTargetType: Target
   - ExplicitCastTargets: UnitAlly
   - GlobalCooldown: 1500 ms; CastTime: 0 ms
   - Effects: EffectApplyAura → the buff aura

Conditions & Validators
- Use SpellCastCondition assets for special gating (e.g., “target must be alive”, “caster is casting”, shape-shift checks).
- SpellInfo already validates range, LoS/visibility, prevention flags (Stun/Silence/Pacify), target type and state (dead/invisible), GCD, power, and shape-shift rules.

Composing multi-effect spells
- Example: “Fire Blast” that damages and applies a short slow on crit only:
  - Effects: [EffectSchoolDamage, EffectApplyAura(slow)]
  - Use ConditionalModifier on EffectSchoolDamage to increase crit chance when target is Frozen (see Fingers of Frost/Deep Freeze patterns in repo).

Testing tips
- Add the new SpellInfo to a ClassInfo (e.g., Mage) to expose it in the action bars.
- Verify SpellHistory updates: GCD and per-spell cooldown; charges if configured.
- Confirm visuals: projectile speed should match SpellInfo.Speed for consistent hit timing.
