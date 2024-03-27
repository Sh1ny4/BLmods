using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.Leveling
{
    [HarmonyPatch(typeof(DefaultSkills), "InitializeAll")]
    internal class DefaultSkillInitializePatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref DefaultSkills __instance, ref SkillObject ____skillOneHanded, ref SkillObject ____skillTwoHanded, ref SkillObject ____skillPolearm, ref SkillObject ____skillBow, ref SkillObject ____skillCrossbow, ref SkillObject ____skillThrowing, ref SkillObject ____skillRiding, ref SkillObject ____skillAthletics, ref SkillObject ____skillCrafting, ref SkillObject ____skillScouting, ref SkillObject ____skillTactics, ref SkillObject ____skillRoguery, ref SkillObject ____skillCharm, ref SkillObject ____skillLeadership, ref SkillObject ____skillTrade, ref SkillObject ____skillSteward, ref SkillObject ____skillMedicine, ref SkillObject ____skillEngineering)
        {
            ____skillOneHanded.Initialize(new TextObject("{=PiHpR4QL}One Handed", null), new TextObject("{=yEkSSqIm}Mastery of fighting with one-handed weapons either with a shield or without.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
            ____skillTwoHanded.Initialize(new TextObject("{=t78atYqH}Two Handed", null), new TextObject("{=eoLbkhsY}Mastery of fighting with two-handed weapons of average length such as bigger axes and swords.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
            ____skillPolearm.Initialize(new TextObject("{=haax8kMa}Polearm", null), new TextObject("{=iKmXX7i3}Mastery of the spear, lance, staff and other polearms, both one-handed and two-handed.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
            ____skillBow.Initialize(new TextObject("{=5rj7xQE4}Bow", null), new TextObject("{=FLf5J3su}Familarity with bows and physical conditioning to shoot with them effectively.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
            ____skillCrossbow.Initialize(new TextObject("{=TTWL7RLe}Crossbow", null), new TextObject("{=haV3nLYA}Knowledge of operating and maintaining crossbows.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
            ____skillThrowing.Initialize(new TextObject("{=2wclahIJ}Throwing", null), new TextObject("{=NwTpATW5}Mastery of throwing projectiles accurately and with power.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
            ____skillRiding.Initialize(new TextObject("{=p9i3zRm9}Riding", null), new TextObject("{=H9Zamrao}The ability to control a horse, to keep your balance when it moves suddenly or unexpectedly, as well as general knowledge of horses, including their care and breeding.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
            ____skillAthletics.Initialize(new TextObject("{=skZS2UlW}Athletics", null), new TextObject("{=bVD9j0wI}Physical fitness, speed and balance.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
            ____skillCrafting.Initialize(new TextObject("{=smithingskill}Smithing", null), new TextObject("{=xWbkjccP}The knowledge of how to forge metal, match handle to blade, turn poles, sew scales, and other skills useful in the assembly of weapons and armor", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
            ____skillScouting.Initialize(new TextObject("{=LJ6Krlbr}Scouting", null), new TextObject("{=kmBxaJZd}Knowledge of how to scan the wilderness for life. You can follow tracks, spot movement in the undergrowth, and spot an enemy across the valley from a flash of light on spearpoints or a dustcloud.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Cunning);
            ____skillTactics.Initialize(new TextObject("{=m8o51fc7}Tactics", null), new TextObject("{=FQOFDrAu}Your judgment of how troops will perform in contact. This allows you to make a good prediction of when an unorthodox tactic will work, and when it won't.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Cunning);
            ____skillRoguery.Initialize(new TextObject("{=V0ZMJ0PX}Roguery", null), new TextObject("{=81YLbLok}Experience with the darker side of human life. You can tell when a guard wants a bribe, you know how to intimidate someone, and have a good sense of what you can and can't get away with.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Cunning);
            ____skillCharm.Initialize(new TextObject("{=EGeY1gfs}Charm", null), new TextObject("{=VajIVjkc}The ability to make a person like and trust you. You can make a good guess at people's motivations and the kinds of arguments to which they'll respond.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Social);
            ____skillLeadership.Initialize(new TextObject("{=HsLfmEmb}Leadership", null), new TextObject("{=97EmbcHQ}The ability to inspire. You can fill individuals with confidence and stir up enthusiasm and courage in larger groups.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Social);
            ____skillTrade.Initialize(new TextObject("{=GmcgoiGy}Trade", null), new TextObject("{=lsJMCkZy}Familiarity with the most common goods in the marketplace and their prices, as well as the ability to spot defective goods or tell if you've been shortchanged in quantity", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Social);
            ____skillSteward.Initialize(new TextObject("{=stewardskill}Steward", null), new TextObject("{=2K0iVRkW}Ability to organize a group and manage logistics. This helps you to run an estate or administer a town, and can increase the size of a party that you lead or in which you serve as quartermaster.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
            ____skillMedicine.Initialize(new TextObject("{=JKH59XNp}Medicine", null), new TextObject("{=igg5sEh3}Knowledge of how to staunch bleeding, to set broken bones, to remove embedded weapons and clean wounds to prevent infection, and to apply poultices to relieve pain and soothe inflammation.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
            ____skillEngineering.Initialize(new TextObject("{=engineeringskill}Engineering", null), new TextObject("{=hbaMnpVR}Knowledge of how to make things that can withstand powerful forces without collapsing. Useful for building both structures and the devices that knock them down.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
            return false;
        }

    }
}
