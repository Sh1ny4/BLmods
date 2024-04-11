using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace wipo.patches.EliteInCastle
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
            else if(sellerHero.CurrentSettlement.IsTown)
            {
                string text = string.Concat(new object[] { "town_recruit_", sellerHero.Culture.StringId });
                __result = (Game.Current.ObjectManager.GetObject<CharacterObject>(text) ?? sellerHero.Culture.BasicTroop);
                return false;
            }
            __result = sellerHero.Culture.BasicTroop;
            return false;
        }
    }
}