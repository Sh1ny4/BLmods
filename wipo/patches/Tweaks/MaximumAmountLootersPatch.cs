﻿using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultBanditDensityModel), "NumberOfMaximumLooterParties", MethodType.Getter)]
    internal class MaximumAmountLootersPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result)
        {
            __result = 100;
        }
    }
}