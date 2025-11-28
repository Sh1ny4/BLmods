using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.ClanStuff
{
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), nameof(DefaultArmyManagementCalculationModel.CanPlayerCreateArmy))]
    internal class CanPlayerCreateArmyPatch : DefaultArmyManagementCalculationModel
    {
        [HarmonyPrefix]
        static bool Prefix(ref DefaultArmyManagementCalculationModel __instance, ref bool __result ,out TextObject disabledReason)
        {
            if (Clan.PlayerClan.Kingdom == null)
            {
                disabledReason = new TextObject("{=XSQ0Y9gy}You need to be a part of a kingdom to create an army.", null);
                __result = false;
                return false;
            }
            if (Clan.PlayerClan.IsUnderMercenaryService)
            {
                disabledReason = new TextObject("{=aRhQzJca}Mercenaries cannot create or manage armies.", null);
                __result = false;
                return false;
            }
            if (Clan.PlayerClan.Tier < 4)
            {
                disabledReason = new TextObject("{=wipo_clan_tier_not_enough}Your clan tier is too low", null);
                __result = false;
                return false;
            }
            if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
            {
                disabledReason = new TextObject("{=NAA4pajB}You need to leave your current army to create a new one.", null);
                __result = false;
                return false;
            }
            if (MobileParty.MainParty.IsCurrentlyAtSea)
            {
                disabledReason = GameTexts.FindText("str_cannot_gather_army_at_sea", null);
                __result = false;
                return false;
            }
            if (Hero.MainHero.IsPrisoner)
            {
                disabledReason = GameTexts.FindText("str_action_disabled_reason_prisoner", null);
                __result = false;
                return false;
            }
            if (MobileParty.MainParty.IsInRaftState)
            {
                disabledReason = GameTexts.FindText("str_action_disabled_reason_raft_state", null);
                __result = false;
                return false;
            }
            if (CampaignMission.Current != null)
            {
                disabledReason = new TextObject("{=FdzsOvDq}This action is disabled while in a mission", null);
                __result = false;
                return false;
            }
            if (PlayerEncounter.Current != null)
            {
                if (PlayerEncounter.EncounterSettlement == null)
                {
                    disabledReason = GameTexts.FindText("str_action_disabled_reason_encounter", null);
                    __result = false;
                    return false;
                }
                Village village = PlayerEncounter.EncounterSettlement.Village;
                if (village != null && village.VillageState == Village.VillageStates.BeingRaided)
                {
                    MapEvent mapEvent = MobileParty.MainParty.MapEvent;
                    if (mapEvent != null && mapEvent.IsRaid)
                    {
                        disabledReason = GameTexts.FindText("str_action_disabled_reason_raid", null);
                        __result = false;
                        return false;
                    }
                }
                if (PlayerEncounter.EncounterSettlement.IsUnderSiege)
                {
                    disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
                    __result = false;
                    return false;
                }
            }
            else
            {
                if (PlayerSiege.PlayerSiegeEvent != null)
                {
                    disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
                    __result = false;
                    return false;
                }
                if (MobileParty.MainParty.MapEvent != null)
                {
                    disabledReason = new TextObject("{=MIylzRc5}You can't perform this action while you are in a map event.", null);
                    __result = false;
                    return false;
                }
            }
            disabledReason = TextObject.GetEmpty();
            __result = true;
            return false;

        }
    }
}
