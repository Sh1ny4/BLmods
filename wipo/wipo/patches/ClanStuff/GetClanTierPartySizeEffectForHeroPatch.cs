using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches.ClanStuff
{
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), "GetClanTierPartySizeEffectForHero")]
    internal class GetClanTierPartySizeEffectForHeroPatch : DefaultPartySizeLimitModel
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, Hero hero)
        {
            __result = 0;
        }
    }
}
