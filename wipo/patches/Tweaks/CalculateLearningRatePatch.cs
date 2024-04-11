using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate", new Type[] {typeof(int),typeof(int),typeof(int),typeof(int),typeof(TextObject),typeof(bool)})]
    internal class CalculateLearningRatePatch
    {
        public static bool Prefix(ref ExplainedNumber __result, ref DefaultCharacterDevelopmentModel __instance, int attributeValue, int focusValue, int skillValue, int characterLevel, TextObject attributeName, bool includeDescriptions = false)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(1f, true, null);
            explainedNumber.AddFactor((float)(attributeValue/2), attributeName);
            int num = MathF.Round(__instance.CalculateLearningLimit(attributeValue, focusValue, null, false).ResultNumber);
            if (skillValue >= num-attributeValue)
            {
                explainedNumber.LimitMax(num - skillValue);
            }
            explainedNumber.LimitMin(0f);
            __result = explainedNumber;
            return false;
        }
    }
}