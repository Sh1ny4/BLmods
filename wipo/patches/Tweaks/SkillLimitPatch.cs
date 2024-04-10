using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Localization;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningLimit")]
    internal class SkillLimitPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref ExplainedNumber __result, int attributeValue, int focusValue, TextObject attributeName, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
            result.Add((float)(focusValue * 60), new TextObject("{=MRktqZwu}Skill Focus", null), null);
            result.LimitMin(0f);
            __result = result;
        }
    }
}