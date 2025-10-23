using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{

    [HarmonyPatch(typeof(DefaultPartyWageModel), nameof(DefaultPartyWageModel.GetCharacterWage))]
    public class GetCharacterWagePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, CharacterObject character)
        {
            int num;
            num = (int)(1.2 * Math.Exp(0.45 * character.Tier));
            if (character.IsMounted) { num = (int)((float)num * 1.3f); }
            if (character.IsRanged) { num = (int)((float)num * 1.1f); }
            __result = num;
        }
    }
}