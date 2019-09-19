## .Net Core Application Development (Crosstech Job Application)

### Basic Functions
 - Turn based game
 - Player can make a regular attack or use ability (no cool-down etc) every turn
 - Player and Bot has following attributes
	 - Name
	 - HitPoint
	 - ArmorClass
	 - Damage
	 - Ability (2 by default, each has attributes Name and Damage)

### Description

In this application I decided to follow the simplified [Dungeons & Dragons](https://dnd.wizards.com/) combat rules which I'll explain briefly, you can read original rules from [here](https://www.dndbeyond.com/sources/basic-rules/combat).

- Each round (2 turns, in this example, player and bot respectively), each player can use one action, which is a regular attack or an ability.
- In order to make a successful attack, attacker rolls a 20 sided dice (**d20**) and it must be equal or greater than the target's **ArmorClass (AC)**. Attack hits or misses according to result of this roll.
- On a *hit*, attacker deals damage to the target by the amount of its **Damage**.
- If it *miss*es, its turn ends.


.Net Core, EntityFramework, SQL Server have been used in this application.
  
### API

For the player and abilities, all CRUD functions have been implemented. You cannot update or delete fight logs. A default monster with *Id: b5774a2a-95da-e911-a603-80fa5b0fc197*, will be created after running provided SQL script, you don't need add a new monster. It will be used for each combat by default (Didn't use environment variable or anything else for this, Id can be changed to another existing DB Player entry from the FightsController.cs).

##### Players
```javascript
[GET] api/players
//returns all players as a list with their abilities
//Response:
[
    {
        "id": "887d38fe-93da-e911-a603-80fa5b0fc197",
        "name": "Caduceus",
        "hitPoint": 87,
        "armorClass": 15,
        "damage": 8,
        "abilities": [
            {
                "id": "b3a5488e-94da-e911-a603-80fa5b0fc197",
                "name": "Fireball",
                "damage": 24
            },
            {
                "id": "8b966264-afda-e911-a603-80fa5b0fc197",
                "name": "Holy Weapon",
                "damage": 16
            }
        ]
    },
    ...
]
    
[GET] api/players/{Id}
//returns player with given Id, Not Found if its not exist
//Response:
{
	"id": "887d38fe-93da-e911-a603-80fa5b0fc197",
	"name": "Caduceus",
	"hitPoint": 87,
	"armorClass": 15,
	"damage": 8,
	"abilities": [
		{
			"id": "b3a5488e-94da-e911-a603-80fa5b0fc197",
			"name": "Fireball",
			"damage": 24
        },
        {
            "id": "8b966264-afda-e911-a603-80fa5b0fc197",
            "name": "Holy Weapon",
            "damage": 16
        }
	]
}
    
[POST] api/players
//creates a player object with given attributes returns the created object
//Request body (application/json):
{
    "Name": "Caduceus",
    "HitPoint": 87,
    "ArmorClass": 15,
    "Damage": 8
}
    
[PUT] api/players/{Id}
//Updates player with given Id, returns updated player
//Request body (application/json):
{
    "Name": "Caduceus",
    "HitPoint": 87,
    "ArmorClass": 15,
    "Damage": 8
}
    
[DELETE] api/players/{Id}
//deletes player with given Id, returns HTTP OK
```    
##### Abilities
```javascript
[GET] api/abilities
//returns all abilities as a list
//Response:
[
    {
        "id": "b3a5488e-94da-e911-a603-80fa5b0fc197",
        "playerId": "887d38fe-93da-e911-a603-80fa5b0fc197",
        "name": "Fireball",
        "damage": 24
    },
    {
        "id": "48d1e63b-95da-e911-a603-80fa5b0fc197",
        "playerId": "b5774a2a-95da-e911-a603-80fa5b0fc197",
        "name": "Fire Breath",
        "damage": 24
    },
    ...
]
    
[GET] api/abilities/{Id}
//returns ability with given Id, Not Found if its not exist
//Response:
{
    "id": "2d10f344-95da-e911-a603-80fa5b0fc197",
    "playerId": "b5774a2a-95da-e911-a603-80fa5b0fc197",
    "name": "Wing Attack",
    "damage": 17
}
    
[POST] api/abilities
//creates a ability object with given attributes returns the created object
//Request body (application/json):
{
    "Name": "Holy Weapon",
    "PlayerId": "887d38fe-93da-e911-a603-80fa5b0fc197",
    "Damage": 16
}
    
[PUT] api/abilities/{Id}
//Updates ability with given Id, returns updated player
//Request body (application/json):
{
    "Name": "Holy Weapon",
    "PlayerId": "887d38fe-93da-e911-a603-80fa5b0fc197",
    "Damage": 16
}  
    
[DELETE] api/abilities/{Id}
//deletes ability with given Id, returns HTTP OK
```
##### Fights
```javascript
[GET] api/fights
//returns all fights as a list with their logs
//Response:
[
    {
        "id": "b8c0287f-44da-e911-a602-80fa5b0fc197",
        "playerId": "60668d63-44da-e911-a602-80fa5b0fc197",
        "botId": "4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197",
        "fightLogs": [
            {
                "id": "d92599b9-53da-e911-a602-80fa5b0fc197",
                "turn": 31,
                "playerHitPoint": 15,
                "botHitPoint": 0,
                "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 2 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197. Player won!"
            },
            {
                "id": "de1e57b2-53da-e911-a602-80fa5b0fc197",
                "turn": 30,
                "playerHitPoint": 15,
                "botHitPoint": 2,
                "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Dealt 10 damage to the Player 60668d63-44da-e911-a602-80fa5b0fc197."
            },
            {
                "id": "dd1e57b2-53da-e911-a602-80fa5b0fc197",
                "turn": 29,
                "playerHitPoint": 25,
                "botHitPoint": 2,
                "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 8 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197."
            },
            {
                "id": "dc1e57b2-53da-e911-a602-80fa5b0fc197",
                "turn": 28,
                "playerHitPoint": 25,
                "botHitPoint": 10,
                "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Couldn't exceed the armor class."
            },
            ...
        ]
    },
    ...
]
    
[GET] api/fights/{Id}
//returns the fight with given Id
//Response:
{
    "id": "b8c0287f-44da-e911-a602-80fa5b0fc197",
    "playerId": "60668d63-44da-e911-a602-80fa5b0fc197",
    "botId": "4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197",
    "fightLogs": [
        {
            "id": "d92599b9-53da-e911-a602-80fa5b0fc197",
            "turn": 31,
            "playerHitPoint": 15,
            "botHitPoint": 0,
            "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 2 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197. Player won!"
        },
        {
            "id": "de1e57b2-53da-e911-a602-80fa5b0fc197",
            "turn": 30,
            "playerHitPoint": 15,
            "botHitPoint": 2,
            "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Dealt 10 damage to the Player 60668d63-44da-e911-a602-80fa5b0fc197."
        },
        {
            "id": "dd1e57b2-53da-e911-a602-80fa5b0fc197",
            "turn": 29,
            "playerHitPoint": 25,
            "botHitPoint": 2,
            "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 8 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197."
        },
        {
            "id": "dc1e57b2-53da-e911-a602-80fa5b0fc197",
            "turn": 28,
            "playerHitPoint": 25,
            "botHitPoint": 10,
            "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Couldn't exceed the armor class."
        },
        ...
	]
}
    
[GET] api/fights/start/{playerId}
//starts new combat using the player with given Id, returns created combat
//Response:
{
    "id": "48c2112a-dcda-e911-a603-80fa5b0fc197",
    "playerId": "887d38fe-93da-e911-a603-80fa5b0fc197",
    "botId": "b5774a2a-95da-e911-a603-80fa5b0fc197",
    "fightLogs": [
        {
            "id": "49c2112a-dcda-e911-a603-80fa5b0fc197",
            "turn": 0,
            "playerHitPoint": 87,
            "botHitPoint": 75,
            "logEntry": "Fight started between Player: 887d38fe-93da-e911-a603-80fa5b0fc197 and Bot: b5774a2a-95da-e911-a603-80fa5b0fc197"
        }
    ]
}
    
[GET] api/fights/{Id}/attack
//player makes a regular attack on the fight with given Id
//returns fight object with its logs
    
[GET] api/fights/{Id}/ability/{abilityId}
//player uses an ability with given Id, it must be on his ability list otherwise it makes a regular attack
//returns fight object with its logs
```
##### Fight Logs
```javascript
[GET] api/fightlogs
//returns all fight logs as a list
//Response:
[
    {
        "id": "b9c0287f-44da-e911-a602-80fa5b0fc197",
        "fightId": "b8c0287f-44da-e911-a602-80fa5b0fc197",
        "turn": 0,
        "playerHitPoint": 25,
        "botHitPoint": 50,
        "logEntry": "Fight started between Player: 60668d63-44da-e911-a602-80fa5b0fc197 and Bot: 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197"
    },
    {
        "id": "e8c18e87-49da-e911-a602-80fa5b0fc197",
        "fightId": "d53c2d6f-49da-e911-a602-80fa5b0fc197",
        "turn": 0,
        "playerHitPoint": 25,
        "botHitPoint": 50,
        "logEntry": "Fight started between Player: 60668d63-44da-e911-a602-80fa5b0fc197 and Bot: 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197"
    },
    {
        "id": "5dbd73fd-52da-e911-a602-80fa5b0fc197",
        "fightId": "b8c0287f-44da-e911-a602-80fa5b0fc197",
        "turn": 1,
        "playerHitPoint": 25,
        "botHitPoint": 50,
        "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Couldn't exceed the armor class."
    },
    {
        "id": "5ebd73fd-52da-e911-a602-80fa5b0fc197",
        "fightId": "b8c0287f-44da-e911-a602-80fa5b0fc197",
        "turn": 2,
        "playerHitPoint": 25,
        "botHitPoint": 50,
        "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Couldn't exceed the armor class."
    },
    ...
]
    
[GET] api/fightlogs/{Id}
//returns a fight log with given Id
//Response:
{
    "id": "426fec46-53da-e911-a602-80fa5b0fc197",
    "fightId": "b8c0287f-44da-e911-a602-80fa5b0fc197",
    "turn": 12,
    "playerHitPoint": 25,
    "botHitPoint": 34,
    "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Couldn't exceed the armor class."
}
    
[GET] api/fightlogs/fight/{Id}
//returns all fight logs belongs to a fight with given Id as a list
//Response:
[
    {
        "id": "d92599b9-53da-e911-a602-80fa5b0fc197",
        "turn": 31,
        "playerHitPoint": 15,
        "botHitPoint": 0,
        "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 2 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197. Player won!"
    },
    {
        "id": "de1e57b2-53da-e911-a602-80fa5b0fc197",
        "turn": 30,
        "playerHitPoint": 15,
        "botHitPoint": 2,
        "logEntry": "Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197: Dealt 10 damage to the Player 60668d63-44da-e911-a602-80fa5b0fc197."
    },
    {
        "id": "dd1e57b2-53da-e911-a602-80fa5b0fc197",
        "turn": 29,
        "playerHitPoint": 25,
        "botHitPoint": 2,
        "logEntry": "Player 60668d63-44da-e911-a602-80fa5b0fc197: Dealt 8 damage to the Bot 4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197."
    },
    ...
]
```