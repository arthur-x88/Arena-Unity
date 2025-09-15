# Heroes of Newerth Lore & Guidelines

This directory contains comprehensive hero guidelines for Heroes of Newerth (HoN), organized by primary attributes. Each hero guide is based on authentic game data extracted from the original HoN source files.

## Directory Structure

```
lore/
├── README.md                 # This file
├── strength/                 # Strength-based heroes
│   ├── armadon.md           # The Molten Giant - Tank/Initiator
│   ├── behemoth.md          # The Ancient Behemoth - Tank/Durable Initiator
│   ├── hellbringer.md       # The Demonic Warrior - Ranged Tank/Carry
│   ├── legionnaire.md       # The Legion Commander - Carry/Tank
│   ├── gladiator.md         # The Bloodsport Champion - Carry/Brawler
│   ├── panda.md             # The Furious Panda - Tank/Carry
│   ├── bephelgor.md         # The Archfiend - Carry/Nuker
│   └── deadwood.md          # The Treant Protector - Tank/Initiator
├── intelligence/             # Intelligence-based heroes
│   ├── pyromancer.md        # The Flame Mage - Nuker/Mage
│   ├── vindicator.md        # The Arcane Warrior - Burst Mage/Assassin
│   ├── zephyr.md            # The Wind Spirit - Support/Nuker
│   ├── defiler.md           # The Corrupted Priest - Nuker/Support
│   ├── frosty.md            # The Abominable Snowman - Nuker/Initiator
│   ├── bubbles.md           # The Water Elemental - Nuker/Support
│   └── dwarf_magi.md        # The Mountain King - Tank/Carry
└── agility/                  # Agility-based heroes
    ├── arachna.md           # The Broodmother - Carry/Ganker
    ├── chronos.md           # The Time Keeper - Carry/Assassin
    ├── vanya.md             # The Rogue Assassin - Assassin/Carry
    ├── engineer.md          # The Gadget Master - Carry/Support
    ├── mortred.md           # The Phantom Assassin - Carry/Assassin
    └── hiro.md              # The Dragon Warrior - Carry/Tank
```

## Hero Data Sources

All hero statistics and abilities are extracted from the authentic HoN game files located in:
- `hon/noh-game/game/heroes/[hero_name]/hero.entity` - Base stats and abilities
- `hon/noh-game/game/heroes/[hero_name]/ability_*/ability.entity` - Ability details

## Hero Classification System

### Primary Attributes
- **Strength (STR)**: Tanky heroes with high health scaling, melee focus
- **Intelligence (INT)**: Spell-casting heroes with magical damage and utility
- **Agility (AGI)**: Mobile heroes with high damage scaling and positioning

### Secondary Classifications
- **Carry**: High damage output, scales well in late game
- **Tank**: High survivability, protects teammates
- **Support**: Provides utility, healing, or crowd control
- **Initiator**: Starts team fights with crowd control
- **Assassin**: High burst damage, picks off isolated targets

## Using These Guidelines

### For New Players
1. **Choose Based on Playstyle**: Pick heroes that match your preferred role
2. **Follow Ability Progression**: Use the recommended leveling order
3. **Item Build Priority**: Follow the suggested itemization strategy
4. **Learn Counterplay**: Understand what heroes counter yours

### For Experienced Players
1. **Adapt to Meta**: Modify builds based on current game balance
2. **Team Composition**: Consider how your hero synergizes with teammates
3. **Situational Awareness**: Adapt to enemy hero picks and strategies
4. **Advanced Positioning**: Master hero positioning for maximum impact

## Hero Development Philosophy

### Early Game (Levels 1-6)
- Focus on farming efficiency
- Learn hero positioning and basic combos
- Secure runes and deny enemy runes
- Build first core items

### Mid Game (Levels 7-12)
- Start team coordination
- Learn to rotate for objectives
- Build team fight awareness
- Complete core item build

### Late Game (Levels 13-16)
- Master ultimate timing
- Focus on team fight positioning
- Coordinate with team for high-impact plays
- Complete luxury item build

## Item Build Categories

### Core Items (Essential)
- Items needed for basic functionality
- Usually completed by 15-20 minutes
- Critical for hero viability

### Situational Items
- Items that counter specific threats
- Based on enemy team composition
- Optional based on game state

### Luxury Items
- High-end items for maximum power
- Requires strong game state
- Situational based on team needs

## Team Synergy Principles

### With Carries
- Protect them while they farm safely
- Create space for them to deal damage
- Absorb damage and tank towers

### With Supports
- They sustain you while you deal damage
- Provide protection for their positioning
- Enable their utility with your damage

### With Initiators
- Follow up their crowd control
- Provide damage after they engage
- Help secure kills on disabled targets

## Advanced Concepts

### Positioning
- **Safe Lane**: Farm safely, poke from range
- **Aggressive Lane**: Pressure enemy, secure kills
- **Split Push**: Take towers while team fights elsewhere

### Timing
- **Ultimate Timing**: Coordinate with team fights
- **Rune Control**: Time rune spawns for advantage
- **Objective Timing**: Coordinate tower/tower takes

### Counterplay
- **Hero Counters**: Know what heroes hard counter yours
- **Item Counters**: Build items to counter enemy heroes
- **Position Counters**: Avoid positioning that favors enemies

## Contributing

To add more heroes to this lore system:
1. Extract hero data from `hon/noh-game/game/heroes/`
2. Follow the established format and structure
3. Include authentic stats, abilities, and playstyle advice
4. Test guidelines for accuracy and completeness

## Disclaimer

These guidelines are based on the original Heroes of Newerth game data and provide a foundation for understanding each hero. Actual gameplay may vary based on:
- Current game balance changes
- Team composition
- Player skill levels
- Game situation and objectives

## Remaining Heroes to Document

This collection represents a comprehensive sample of HoN heroes. The following heroes can be documented using the same format and methodology:

### Strength Heroes Remaining:
- Accursed, Admiral, Andromeda, Babayaga, Bephelgor (partially), Centaur, Devourer, Diseased Rider, Dragon Knight, Electrician, Gauntlet, Hammerstorm, Hantumon, Helldemon, Jereziah, Kraken, Magmar, Magnataur, Maliken, Predator, Rampage, Rocky, Sand Wraith, Shaman, Soulstealer, Tundra, Wolfman

### Intelligence Heroes Remaining:
- Babayaga, Chipper, Corrupted Disciple, Dwarf Magi (partially), Ebulus, Fairy, Kunas, Pollywog Priest, Puppetmaster, Shaman (partially), Succubis, Techies, Tempest, Witch Slayer

### Agility Heroes Remaining:
- Andromeda, Bounty Hunter, Corrupted Disciple, Ebulus, Fade, Forsaken Archer, Hunter, Javaras, Jedi, Krixi, Leech King, Nomad, Phantom Lancer, Scar, Scout, Sniper, Sordit, Soulstealer, Troll, Ursa, Valkyrie, Valmont, Windrunner, Wolfman, Xalynx, Yogi

## How to Create Additional Hero Guides

1. **Extract Hero Data**: Use the hero.entity files from `hon/noh-game/game/heroes/[hero_name]/`
2. **Follow the Format**: Use the established template with base stats, scaling, abilities, and gameplay guidelines
3. **Include Real Data**: Base all statistics on the authentic game files
4. **Test and Verify**: Ensure all information is accurate and helpful

## Hero Statistics Summary

**Heroes Documented**: 21 out of ~80+ total HoN heroes
- **Strength**: 8 heroes (Armadon, Behemoth, Hellbringer, Legionnaire, Gladiator, Panda, Bephelgor, Deadwood)
- **Intelligence**: 7 heroes (Pyromancer, Vindicator, Zephyr, Defiler, Frosty, Bubbles, Dwarf Magi)
- **Agility**: 6 heroes (Arachna, Chronos, Vanya, Engineer, Mortred, Hiro)

**Data Sources Verified**: All stats extracted from authentic HoN game files
**Format Established**: Comprehensive template for all hero roles and playstyles

Remember: These are guidelines, not rules. Adapt and experiment to find what works best for your playstyle!
