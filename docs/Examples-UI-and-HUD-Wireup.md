# Examples — UI & HUD Wireup

Patterns (present in codebase)
- Panels under Client/UI/Panels/*, derived from UIWindow<TScreen>.
- Existing: LobbyPanel, Battle HUD components, Localization wrappers.

New UI for 0.1 (documentation-only)
- MatchHud (overlay)
  - Timer (mm:ss), team kills, tower HP bars
  - Gold counter, level bar
  - Recall button (disabled while moving/under attack)
- ScoreboardPanel (Tab)
  - Table of players with K/D/A, CS, gold, items
- ShopPanel (base-only)
  - Item list, cost, purchase button; disabled outside base
- EndPanel
  - Winner, summary, ‘Return to Lobby’

Anchors & layout (textual)
- Top center: Timer | Ally Kills — Enemy Kills | Tower HP bars
- Bottom left: Gold + Level bar
- Bottom center: Action bars (existing)
- Bottom right: Recall button
- Right side: Killfeed list
- Fullscreen (on key): Scoreboard overlay

Localization
- Use existing LocalizedTextMeshProUGUI wrappers where possible.

Data bindings
- Timer: MatchState.TimeMs → format mm:ss
- Kills: MatchState.TeamScores
- Tower HP: derived from MatchState.TowerHp[] normalized to bars
- Gold/Level: PlayerEconomy (server-synced fields or events)
- Items: Inventory list

Testing
- Toggle scoreboard while moving/casting to ensure no input lock
- Scale and anchor checks at 16:9/16:10/21:9
