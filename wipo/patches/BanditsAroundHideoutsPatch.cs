using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultBanditDensityModel), nameof(DefaultBanditDensityModel.NumberOfMaximumBanditPartiesAroundEachHideout), MethodType.Getter)]
    internal class BanditsAroundHideoutsPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result)
        {
            __result = 3;
        }
    }
}