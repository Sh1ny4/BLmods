using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace wipo.patches.CharacterCreationPatch
{
    [HarmonyPatch(typeof(CharacterCreationContentBase), "ApplySkillAndAttributeEffects")]
    internal class ApplySkillAndAttributeEffectsPatch
    {
        [HarmonyPrefix]
        static bool Prefix(List<SkillObject> skills, int focusToAdd, int skillLevelToAdd, CharacterAttribute attribute, int attributeLevelToAdd, List<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocusPoints = 0, int unspentAttributePoints = 0)
        {
            
            foreach (SkillObject skill in skills)
            {
                Hero.MainHero.HeroDeveloper.AddFocus(skill, focusToAdd, false);
                if (Hero.MainHero.GetSkillValue(skill) == 1)
                {
                    Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skill, skillLevelToAdd - 1, false);
                }
                else
                {
                    Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skill, skillLevelToAdd, false);
                }
            }
            Hero.MainHero.HeroDeveloper.UnspentFocusPoints += unspentFocusPoints;
            Hero.MainHero.HeroDeveloper.UnspentAttributePoints += unspentAttributePoints;
            if (attribute != null)
            {
                Hero.MainHero.HeroDeveloper.AddAttribute(attribute, attributeLevelToAdd, false);
            }
            if (traits != null && traits.Count > 0)
            {
                foreach (TraitObject trait in traits)
                {
                    Hero.MainHero.SetTraitLevel(trait, Hero.MainHero.GetTraitLevel(trait) + traitLevelToAdd);
                }
            }
            GainRenownAction.Apply(Hero.MainHero, (float)renownToAdd, true);
            GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, goldToAdd, true);
            Hero.MainHero.HeroDeveloper.SetInitialLevel(1);
            return false;
        }

    }
}