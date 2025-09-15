# File-By-File Map (High Level)

## Core
- `Assets/Scripts/Core/Entity/*`: Units, Players, Creatures, controllers
- `Assets/Scripts/Core/Spells/*`: Spell/Aura infos and runtime logic
- `Assets/Scripts/Core/Map/*`: Map, MapManager; grid under `Core/World/Grid`
- `Assets/Scripts/Core/Balance/*`: Scriptable balance data (maps, classes, factions)

## Client
- `Assets/Scripts/Client/UI/*`: Lobby and Battle HUD
- `Assets/Scripts/Client/Multiplayer/*`: Client listeners/helpers

## Server
- `Assets/Scripts/Server/Multiplayer/*`: WorldServer, network/game listeners

## Multiplayer
- `Assets/Scripts/Core/Multiplayer/*`: PhotonBolt controller/reference/tokens

## Workflow
- `Assets/Scripts/Workflow/*`: GameManager, Standard/Dedicated workflows

