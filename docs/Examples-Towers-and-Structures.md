# Examples — Towers & Structures

Towers
- Range: 20m; Attack rate: 1.0s; Damage: 60
- Priority: minion > player; switch to player when taking player damage, persist 3s since last player hit
- Use a Spell (StructureAttack) to apply damage so you reuse hit/crit/VFX logic

Bases
- Large HP pool (e.g., 6000)
- On base death → victory for opposing team; stop waves; open End screen

HP display
- Represent tower/base HP as percentages in MatchState and map to HUD bars

Target switching rules (textual)
- If minion exists in range → target nearest minion
- Else if player in range → target player
- If tower took player damage → force target that player for 3s
- If no valid target for 2s → Idle; poll again at 5Hz

Authoring notes
- Place StructureDefinition SO under Assets/Settings/Balance/Maps
- Prefabs: add colliders for range gizmos and attack origins
