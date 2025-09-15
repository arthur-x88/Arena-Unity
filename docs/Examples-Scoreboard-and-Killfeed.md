# Examples — Scoreboard and Killfeed

Scoreboard (Tab overlay)
Columns
- Player: name, class icon, team color
- K/D/A
- CS (creep score = minion last-hits)
- Gold (current)
- Items (icons)

Row ordering
- By team, then by (K+A) descending, then deaths ascending, then name

Example row schema (client-side view)
```json path=null start=null
{
  "playerId": 42,
  "name": "Arcanist",
  "team": 0,
  "classType": "Mage",
  "kills": 5,
  "deaths": 2,
  "assists": 4,
  "cs": 63,
  "gold": 1850,
  "items": ["Mage Tome", "Boots of Swiftness"]
}
```

Killfeed
- Display recent combat outcomes: Killer → Victim [SpellIcon] (+assisters)
- Duration per entry: 6s; max entries shown: 3–5

Example killfeed entry
```json path=null start=null
{
  "killer": "Arcanist",
  "victim": "Berserker",
  "spell": "Pyroblast",
  "crit": true,
  "assists": ["Healer", "Scout"],
  "time": 641.2
}
```

Iconography
- Spell icons: Assets/Graphics/Spell Icons/*
- Class icons: Assets/Graphics/Class Icons/*

Notes
- CS increments on last-hit only; assists are time-boxed (e.g., 10s window).
- Keep string budgets short; prefer IDs internally and map to names/icons in UI.
