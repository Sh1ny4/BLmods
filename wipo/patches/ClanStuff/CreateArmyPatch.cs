using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace wipo.patches.ClanStuff
{
    [HarmonyPatch(typeof(Kingdom), nameof(Kingdom.CreateArmy))]
    internal class CreateArmyPatch
    {
        [HarmonyPrefix]
        static bool Prefix(Hero armyLeader, Settlement targetSettlement, Army.ArmyTypes selectedArmyType)
        {
            if (armyLeader.Clan.Tier >= 4)
            {
                return true;
            }
            return false;
        }
    }
}
