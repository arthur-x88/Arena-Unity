# Examples — Items and Shop (Passive Items for 0.1)

Concept
- Items are match-local purchases that apply passive effects via existing AuraInfo and SpellModifier systems.
- Buying is allowed only in base radius; selling restricted or disabled for 0.1.

Suggested asset structure
- Items: Assets/Settings/Items/
- Each ItemDefinition references 1–3 AuraInfo assets (passives) and has a cost.

Example items (passive)
1) Mage Tome (450g)
- +10% Spell Power; +5% Spell Haste
- Implementation: an AuraInfo with two AuraEffectInfo modifiers:
  - AuraEffectInfoModifyDamagePercentDone (Spell school mask = Magic, value +10%)
  - AuraEffectInfoModifySpellHaste (value +5%)

2) Boots of Swiftness (300g)
- +10% movement speed
- Implementation: AuraEffectInfoIncreaseSpeed (+10%)

3) Guardian Amulet (500g)
- -10% damage taken
- Implementation: AuraEffectInfoModifyDamagePercentTaken (-10%)

Shop rules (0.1)
- Purchase only if player is within base radius (e.g., 12m from BaseCore).
- Duplicate stacking: allow linear stacking for simple items or mark items as unique to prevent duplicates.
- Refunds: disabled; selling off (keep 0.1 minimal).

Illustrative item definition (not code):
```yaml path=null start=null
ItemDefinition:
  Name: Mage Tome
  Cost: 450
  Auras:
    - Aura: SpellPowerUpAura
    - Aura: SpellHasteUpAura
  Unique: false
```

Inventory & application
- On purchase, apply referenced Auras to the player (they persist through death in 0.1 unless you choose otherwise).
- On drop/sell (if enabled later), remove Auras.

UI
- Shop panel lists items, cost, and passive descriptions.
- Purchase button is enabled only when: in base radius AND enough gold.
- Show passive icons sourced from item icons or aura/ability icons.

Testing
- Verify gold decrements; aura tooltips reflect active passives.
- Confirm passives persist through respawn (or not) per your chosen rule.
