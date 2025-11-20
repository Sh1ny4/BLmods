``/singleplayer _MODULES_*Bannerlord.Harmony*Native*SandBoxCore*CustomBattle*SandBox*StoryMode*wipo*YetAnotherTroopOverhaul*EliteRecruitsInCastles*CharacterCreationRedone*TournamentEquipmentRedone*FullScreenCinematics*_MODULES_``

## Tweaks
> - remove horses in castles
> - loss of renown & influence after defeat
> - button for the encyclopedia
> - waiting in settlements costs money

## CS to check
```
private float _desiredTotalCompanionCount
private void CreateGenericScene()
character_menu_c
public SavedGameVM
```
## Reworks and Additions

#### Camps
> - Used by AI at night > not camping at night damages morale
> - Allows to train troops, read, hunt

#### Serve as a Soldier

#### Ability to join a caravan to travel
> - Companions use it to travel
> - On an attack you can choose to join the fight


#### Warband Skills & Perks

#### Castles can block passage through navmesh

## Recruitement Stuff
> - can be changed with a kingdom policy
> - can only recruit mercs until you join a kingdom
> - need a deserialize, an xml that defines for each kingdom which type of recruitement and in each case what recruit we get
> - three types :
>   - Retinue : you hire a tier 5 noble troop. Alongside it you get a tier 2 noble troop, 2 tier 3 inf and a tier 2 ranged.
>   - Volunteer : same as vanilla, need to recruit a lot
>   - Levy : You get a lot of low tier troops at the cost of prosperity / hearts.

|                    | Aserai             | Battania           | Empire             | Khuzait            | Sturgia            | Vlandia            |
|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:------------------ |
| Recruitement Type  | Volonteer          | Retinue            | Volonteer          | Levy               | Retinue            | Levy               |

## Tournament Stuff :
> - more types of tournaments
> - tied to feasts
> - fewer participants, only named NPCs/nobles ?
> - balanced armors
> - scene changes with town level

|                    | Aserai             | Battania           | Empire             | Khuzait            | Sturgia            | Vlandia            |
|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:------------------ |
| Tournament Stage 1 | Placeholder        | Bow Contest        | Gladiator          | Horse Race         | Duels              | Jousting           |
| Tournament Stage 2 | Placeholder        | Duels              | Chariot Race       | Horse Archery      | 20v20 battle       | Horse Race         |

## Clan Stuff
> - No clan tiers but instead ranks of nobility granted by the kingdom, they are linked to the clan's influence
> - Loss of renown and influence
> - Noble opinion is separated from the clan's

|                    | Aserai             | Battania           | Empire             | Khuzait            | Sturgia            | Vlandia            | Note
|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:------------------ |:--------------
| Clan Tier 1        | Faris              | Fian               | Cataphract         | Kheshig            | Druzhina           | Chevalier          | gets a tier 6 weapon, has a banner
| Clan Tier 2        | Sheikh             | Chieftain          | Centurion          | Noyan              | Boyar              | Baron              | Controls a village, can vote in the kingdom's decisions, gains a retinue
| Clan Tier 3        | Muqaddam           |                    | Tribunes           |                    |                    | Comte              | Controls a castle, has a council Marshal, Steward, Chancellor
| Clan Tier 4        | Emir               | Earl               | Legatus            | Tarkhan            | Knyaz              | Duc                | Controls a town, can create an army, can initiate a vote
| Clan Tier 5        | Sultan             | High King          | General            | Khan               | Grand Prince       | Roi                | Controls a Faction
| Placeholder        | Placeholder        | Placeholder        | Placeholder        | Placeholder        | Placeholder        | Placeholder        |

## Kingdom Stuff
> - Different kingdom get different inner working
> - Type of ruling can be changed with a policy
> - internal rebellions can happen
> - unified empires

|                    | Aserai             | Battania           | Empire             | Khuzait            | Sturgia            | Vlandia            |
|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:------------------ |
| Internal Politics  | Inherited Monarchy | Elected Monarchy   | Oligarchy          | Strongest General  | Elected Monarchy   | Inherited Monarchy |

## commissions
> - horse armours => chainmail with tableau on top
> - pavise but good
