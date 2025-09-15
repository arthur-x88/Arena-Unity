# Examples — Economy & Progression

Gold (suggested)
- Melee minion last-hit: 20g
- Caster minion last-hit: 25g
- Player kill: 200g to killer; 150g split among assisters (max 4)
- Tower: 250g global to team
- Base: 300g global to team

XP (suggested)
- Melee: 40xp; Caster: 50xp
- Kill: 150xp to killer; 25xp to each assister
- Proximity XP: full credit within 20m of kill/minion

Level curve (simple)
```csharp path=null start=null
int XpForLevel(int level) => 150 * level; // Level 2: 300, L3: 450, etc.
```

Stat scaling
- Use a PlayerLevelDefinition SO to map level → stat multipliers (HP, power regen, etc.) and ability rank unlocks

CS (creep score)
- Increment on last-hits only; display per player on scoreboard

Events
- On minion death: award gold to last-hitter; grant proximity XP
- On player kill: award killer gold/xp; distribute assists by time window (e.g., 10s)
