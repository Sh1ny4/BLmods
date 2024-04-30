using HarmonyLib;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.Core;

namespace wipo.patches.CostPatch
{
    [HarmonyPatch(typeof(KingdomManager), nameof(KingdomManager.GetMercenaryWageAmount))]
    internal class GetMercenaryWageAmountPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, Hero hero)
        {
            int num = 0;
            foreach(MobileParty party in MobileParty.All)
            {
                if(party.LeaderHero == hero) { num += (int)party.PartySizeRatio*10; }
            }
            __result = num;
            return;
        }
    }
}
