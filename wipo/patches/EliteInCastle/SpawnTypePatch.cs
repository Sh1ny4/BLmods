using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace wipo.patches
{

    [HarmonyPatch(typeof(DefaultVolunteerModel), nameof(DefaultVolunteerModel.GetBasicVolunteer))]
    public class SpawnTypePatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref CharacterObject __result, Hero sellerHero)
        {
            if (sellerHero.CurrentSettlement.IsCastle)
            {
                __result = sellerHero.Culture.EliteBasicTroop;
                return false;
            }
            __result = sellerHero.Culture.BasicTroop;
            return false;
        }
    }
}