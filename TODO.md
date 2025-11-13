``/singleplayer _MODULES_*Bannerlord.Harmony*Native*SandBoxCore*CustomBattle*SandBox*StoryMode*wipo*YetAnotherTroopOverhaul*EliteRecruitsInCastles*CharacterCreationRedone*TournamentEquipmentRedone*FullScreenCinematics*_MODULES_``

## Reworks
#### Better tourneys :
> - tied to feasts
> - fewer participants, only named NPCs/nobles ?
> - balanced armors
> - scene changes with town level

| Culture  | Contest 1	    | Contest 2	                        
|:---------|:--------------|:-------------
| Aserai   | duels    	    | jousting
| Battania | bow contest	  | duels
| Empire   | gladiator	    | chariot race
| Khuzait  | horse archery | horse race
| Sturgia  | duels	        | 20v20 battle
| Vlandia  | horse race		  | jousting

#### Clan tier
 No clan tiers but instead levels of nobility granted by the kingdom, they are linked to the clan's influence
 | Level | Aserai   | Battania   | Empire    	| Khuzait | Sturgia      | Vlandia   | Perks
 |:------|:---------|:-----------|:-----------|:--------|:-------------|:----------|:--------------
 |   1   | Faris 		 | Fian	 		   | Cataphract	| Kheshig	| Druzhina     | Chevalier	| gets a tier 6 weapon
 |   2   | Sheikh 		| Chieftain		| Patrician		| Noyan			| Boyar			     | Baron			  | Controls a village, can vote in the kingdom's decisions, gains a retinue
 |   3   | Muqaddam	| 				       | Senator		  | 				    | 				         | Comte			  | Controls a castle, has a council Marshal, Steward, Chancellor
 |   4   | Emir			  | Earl			    | Consul 		  | Tarkhan | Knyaz			     | Duc			    | Controls a town, can create an army
 |   5   | Sultan		 | High King		| Imperator		| Khan			 | Grand Prince	| Roi			    | Controls a Kingdom

#### recruitement system
> can be changed with a policy
> - levy
> - volonteer
> - service
> - retinue

#### Warband Skills & Perks

## New Systems

#### Serve as a Soldier

#### Camps
> - Used by AI at night > not camping at night damages morale
> - Allows to train troops, read, hunt

#### Ability to join a caravan to travel

#### Castles can block passage through navmesh

## Tweaks
> - remove horses in castles
> - loss of renown & influence after defeat
> - button for the encyclopedia
> - waiting in settlements costs money
> - unified empires

## CS to check
```
private void AddSkinArmorWeaponMultiMeshesToEntity(uint teamColor1, uint teamColor2, bool needBatchedVersion, bool forceUseFaceCache = false)
public void AddArmorMultiMeshesToAgentEntity(uint teamColor1, uint teamColor2)
void IMissionListener.OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
```

## commissions
> - horse armours => chainmail with tableau on top
> - pavise but good
