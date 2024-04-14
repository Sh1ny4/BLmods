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
            // catsles recruit are elite troops
            if (sellerHero.CurrentSettlement.IsCastle)
            {
                __result = sellerHero.Culture.EliteBasicTroop;
                return false;
            }
            // town can vhave a custom troop , basic troop name has to be town_recruit_<culture ID>, default to regular basic troop if no corresponding NPC can be found
            else if(sellerHero.CurrentSettlement.IsTown)
            {
                string text = string.Concat(new object[] {sellerHero.Culture.StringId, "_town_recruit" });
                __result = (Game.Current.ObjectManager.GetObject<CharacterObject>(text) ?? sellerHero.Culture.BasicTroop);
                return false;
            }
            __result = sellerHero.Culture.BasicTroop;
            return false;
        }
    }
}