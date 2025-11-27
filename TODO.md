``/singleplayer _MODULES_*Bannerlord.Harmony*Native*SandBoxCore*CustomBattle*SandBox*StoryMode*wipo*YetAnotherTroopOverhaul*EliteRecruitsInCastles*CharacterCreationRedone*TournamentEquipmentRedone*FullScreenCinematics*_MODULES_``

## Tweaks
 - Remove horses in castles
 - Loss of renown & influence after defeat
 - Button for the encyclopedia
 - Waiting in settlements costs money

## CS to check
```
private float _desiredTotalCompanionCount
private void CreateGenericScene()
character_menu_c
public SavedGameVM
public class SandBoxViewSubModule : MBSubModuleBase
```

## Reworks and Additions

#### Camps
 - Used by AI at night or if ratio of wounded troops > 50% && no enemy in sight
 - Not camping at night damages morale
 - Allows to train troops, read, hunt

#### Serve as a Soldier

#### Ability to join a caravan to travel
 - Companions use it to travel
 - On an attack you can choose to join the fight

#### Warband Skills & Perks

#### Castles can block passage through navmesh

## Recruitement Stuff
 - Can only recruit mercs until you join a kingdom
 - Levelling requires more XP
 - Need a deserialize, an xml that defines for each kingdom which type of recruitement and in each case what recruit we get
 - Three types :
   - Levy : You get a lot of low tier troops at the cost of prosperity / hearts.
   - Retinue : you hire a tier 5 noble troop. Alongside it you get a tier 2 noble troop, 3 tier 2 inf and a tier 2 ranged. Retinues are only in castles
   - Volunteer : same as vanilla, need to recruit a lot with a limited pool

## Tournament Stuff :
 - Tied to feasts
 - Fewer participants, only named NPCs (isHero)
 - One stage per day, entering a tournament means you can't leave without forfeiting
 - Stage types :
   - Bow Contest : first to hit all targets, the one with the most if not all are done, the one that did it the fastest as third decider
   - Chariot Race : Fastest to end the race
   - Duel : Similar to vanilla, only 1v1
   - Gladiator : Last alive wins
   - Horse Archery : Fastest to end a course with targets to destroy, penality for each missed target
   - Horse Race : Fastest to end the race
   - Jousting : Similar to vanilla, only 1v1
   - XvX battle : The one with the most kills wins, with bonus points if his team wins

## Clan Stuff
 - Clan tiers start when joining a faction, before that renown only influences the chances of receiving an invitation
 - No clan tiers but instead ranks of nobility granted by the kingdom, they are linked to the clan's influence
 - Loss of renown and influence
 - Noble opinion is separated from the clan's

## Kingdom Stuff
 - Unified empires
 - Rework of the Policies
 - Different kingdoms get different inner workings
 - Integrated with diplomacy
 - Voting and creating armies do not cost influence
   - Elected Monarchy : Clans elect a king, which stays in power until his death. choice overrules the vote
   - Inherited Monarchy : The ruler chooses a successor from his clan before dying, no voting only his decision
   - Oligarchy : No king, every decision is voted
   - Strongest Clan : clan leader of the most influential clan rules, needs 20% more influence than the previous leader, choice overrules the vote

|                    | Aserai             | Battania           | Empire             | Khuzait            | Sturgia            | Vlandia            | Notes
|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------|:-------------------
| Recruitement Type  | Volonteer          | Retinue            | Volonteer          | Levy               | Retinue            | Levy               | Can be changed with a policy
| Tournament Stage 1 | Placeholder        | Bow Contest        | Gladiator          | Horse Race         | Duels              | Jousting           |
| Tournament Stage 2 | Placeholder        | Duels              | Chariot Race       | Horse Archery      | XvX battle         | Horse Race         |
| Tournament Stage 2 | Placeholder        | Throwing Contest   | Boat Battle        | Bow Contest        | Throwing Contest   | Duels              |
| Internal Politics  | Inherited Monarchy | Elected Monarchy   | Oligarchy          | Strongest Clan     | Elected Monarchy   | Inherited Monarchy | Can be changed with a policy
| Clan Tier 1        | Faris              | Fian               | Cataphract         | Kheshig            | Druzhina           | Chevalier          | Gets a tier 6 weapon, has a banner, gains a retinue
| Clan Tier 2        | Sheikh             | Chieftain          | Centurion          | Noyan              | Boyar              | Baron              | Controls a village, can vote in the kingdom's decisions
| Clan Tier 3        | Muqaddam           |                    | Tribunes           |                    |                    | Comte              | Controls a castle, has a council Marshal, Steward, Chancellor
| Clan Tier 4        | Emir               | Earl               | Legatus            | Tarkhan            | Knyaz              | Duc                | Controls a town, can create an army, can initiate a vote
| Clan Tier 5        | Sultan             | High King          | General            | Khan               | Grand Prince       | Roi                | Controls a Faction

## commissions
 - horse armours => chainmail with tableau on top
 - pavise but good
