using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultBanditDensityModel), "NumberOfMaximumBanditPartiesAroundEachHideout", MethodType.Getter)]
    internal class BanditsAroundHideoutsPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result)
        {
            __result = 5;
        }
    }
}