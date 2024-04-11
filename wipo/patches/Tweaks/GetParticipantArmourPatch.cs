using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultTournamentModel), "GetParticipantArmor")]
    internal class GetParticipantArmourPatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref Equipment __result, CharacterObject participant)
        {
            if (CampaignMission.Current != null && CampaignMission.Current.Mode != MissionMode.Tournament && Settlement.CurrentSettlement != null)
            {
                __result = (Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_" + Settlement.CurrentSettlement.MapFaction.Culture.StringId) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_empire")).RandomBattleEquipment;
                return false;
            }
            __result = participant.RandomBattleEquipment;
            if (CampaignMission.Current.Mode == MissionMode.Tournament)
            {
                string text = string.Concat(new object[] { "tournament_", Settlement.CurrentSettlement.MapFaction.Culture.StringId });
                __result = (Game.Current.ObjectManager.GetObject<CharacterObject>(text) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_empire")).RandomBattleEquipment;
            }
            return false;
        }
    }
}