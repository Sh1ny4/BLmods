﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using HarmonyLib;

namespace wipo.patches.PerksPatch
{
    [HarmonyPatch(typeof(DefaultSkillEffects), "InitializeAll")]
    internal class InitializeAllPatch : DefaultSkillEffects
    {
        static bool Prefix(ref SkillEffect ____effectOneHandedSpeed, ref SkillEffect ____effectOneHandedDamage, ref SkillEffect ____effectTwoHandedSpeed, ref SkillEffect ____effectTwoHandedDamage, ref SkillEffect ____effectPolearmSpeed, ref SkillEffect ____effectPolearmDamage, ref SkillEffect ____effectBowLevel, ref SkillEffect ____effectBowDamage, ref SkillEffect ____effectBowAccuracy, ref SkillEffect ____effectThrowingSpeed, ref SkillEffect ____effectThrowingDamage, ref SkillEffect ____effectThrowingAccuracy, ref SkillEffect ____effectCrossbowReloadSpeed, ref SkillEffect ____effectCrossbowAccuracy, ref SkillEffect ____effectHorseLevel, ref SkillEffect ____effectHorseSpeed, ref SkillEffect ____effectHorseManeuver, ref SkillEffect ____effectMountedWeaponDamagePenalty, ref SkillEffect ____effectMountedWeaponSpeedPenalty, ref SkillEffect ____effectDismountResistance, ref SkillEffect ____effectAthleticsSpeedFactor, ref SkillEffect ____effectAthleticsWeightFactor, ref SkillEffect ____effectKnockBackResistance, ref SkillEffect ____effectKnockDownResistance, ref SkillEffect ____effectSmithingLevel, ref SkillEffect ____effectTacticsAdvantage, ref SkillEffect ____effectTacticsTroopSacrificeReduction, ref SkillEffect ____effectTrackingLevel, ref SkillEffect ____effectTrackingRadius, ref SkillEffect ____effectTrackingSpottingDistance, ref SkillEffect ____effectTrackingTrackInformation, ref SkillEffect ____effectRogueryLootBonus, ref SkillEffect ____effectCharmRelationBonus, ref SkillEffect ____effectTradePenaltyReduction, ref SkillEffect ____effectSurgeonSurvivalBonus, ref SkillEffect ____effectSiegeEngineProductionBonus, ref SkillEffect ____effectTownProjectBuildingBonus, ref SkillEffect ____effectHealingRateBonusForHeroes, ref SkillEffect ____effectHealingRateBonusForRegulars, ref SkillEffect ____effectGovernorHealingRateBonus, ref SkillEffect ____effectLeadershipMoraleBonus, ref SkillEffect ____effectLeadershipGarrisonSizeBonus, ref SkillEffect ____effectStewardPartySizeBonus, ref SkillEffect ____effectEngineerLevel)
        {
            ____effectOneHandedSpeed.Initialize(new TextObject("{=hjxRvb9l}One handed weapon speed: +{a0}%", null), new SkillObject[] { DefaultSkills.OneHanded }, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectOneHandedDamage.Initialize(new TextObject("{=baUFKAbd}One handed weapon damage: +{a0}%", null), new SkillObject[] { DefaultSkills.OneHanded }, SkillEffect.PerkRole.Personal, 0.15f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectTwoHandedSpeed.Initialize(new TextObject("{=Np94rYMz}Two handed weapon speed: +{a0}%", null), new SkillObject[] { DefaultSkills.TwoHanded }, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectTwoHandedDamage.Initialize(new TextObject("{=QkbbLb4v}Two handed weapon damage: +{a0}%", null), new SkillObject[] { DefaultSkills.TwoHanded }, SkillEffect.PerkRole.Personal, 0.16f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectPolearmSpeed.Initialize(new TextObject("{=2ATI9qVM}Polearm weapon speed: +{a0}%", null), new SkillObject[] { DefaultSkills.Polearm }, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectPolearmDamage.Initialize(new TextObject("{=17cIGVQE}Polearm weapon damage: +{a0}%", null), new SkillObject[] { DefaultSkills.Polearm }, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectBowLevel.Initialize(new TextObject("{=XN7BX0qP}Max usable bow difficulty: {a0}", null), new SkillObject[] { DefaultSkills.Bow }, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectBowDamage.Initialize(new TextObject("{=RUZHJMQO}Bow Damage: +{a0}%", null), new SkillObject[] { DefaultSkills.Bow }, SkillEffect.PerkRole.Personal, 0.11f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectBowAccuracy.Initialize(new TextObject("{=sQCS90Wq}Bow Accuracy: +{a0}%", null), new SkillObject[] { DefaultSkills.Bow }, SkillEffect.PerkRole.Personal, 0.09f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectThrowingSpeed.Initialize(new TextObject("{=Z0CoeojG}Thrown weapon speed: +{a0}%", null), new SkillObject[] { DefaultSkills.Throwing }, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectThrowingDamage.Initialize(new TextObject("{=TQMGppEk}Thrown weapon damage: +{a0}%", null), new SkillObject[] { DefaultSkills.Throwing }, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectThrowingAccuracy.Initialize(new TextObject("{=SfKrjKuO}Thrown weapon accuracy: +{a0}%", null), new SkillObject[] { DefaultSkills.Throwing }, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectCrossbowReloadSpeed.Initialize(new TextObject("{=W0Zu4iDz}Crossbow reload speed: +{a0}%", null), new SkillObject[] { DefaultSkills.Crossbow }, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectCrossbowAccuracy.Initialize(new TextObject("{=JwWnpD40}Crossbow accuracy: +{a0}%", null), new SkillObject[] { DefaultSkills.Crossbow }, SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectHorseLevel.Initialize(new TextObject("{=8uBbbwY9}Max mount difficulty: {a0}", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectHorseSpeed.Initialize(new TextObject("{=Y07OcP1T}Horse speed: +{a0}", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectHorseManeuver.Initialize(new TextObject("{=AahNTeXY}Horse maneuver: +{a0}", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, 0.04f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectMountedWeaponDamagePenalty.Initialize(new TextObject("{=0dbwEczK}Mounted weapon damage penalty: {a0}%", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 20f, 0f);
            ____effectMountedWeaponSpeedPenalty.Initialize(new TextObject("{=oE5etyy0}Mounted weapon speed & reload penalty: {a0}%", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 30f, 0f);
            ____effectDismountResistance.Initialize(new TextObject("{=kbHJVxAo}Dismount resistance: {a0}% of max. hitpoints", null), new SkillObject[] { DefaultSkills.Riding }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 40f, 0f);
            
            ____effectAthleticsSpeedFactor.Initialize(new TextObject("{=rgb6vdon}Running speed increased by {a0}%", null), new SkillObject[] { DefaultSkills.Athletics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectAthleticsWeightFactor.Initialize(new TextObject("{=WaUuhxwv}Weight penalty reduced by: {a0}%", null), new SkillObject[] { DefaultSkills.Athletics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectKnockBackResistance.Initialize(new TextObject("{=TyjDHQUv}Knock back resistance: {a0}% of max. hitpoints", null), new SkillObject[] { DefaultSkills.Athletics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 15f, 0f);
            ____effectKnockDownResistance.Initialize(new TextObject("{=tlNZIH3l}Knock down resistance: {a0}% of max. hitpoints", null), new SkillObject[] { DefaultSkills.Athletics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 40f, 0f);
           
            ____effectSmithingLevel.Initialize(new TextObject("{=ImN8Cfk6}Max difficulty of weapon that can be smithed without penalty: {a0}", null), new SkillObject[] { DefaultSkills.Crafting }, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
           
            ____effectTacticsAdvantage.Initialize(new TextObject("{=XO3SOlZx}Simulation advantage: +{a0}%", null), new SkillObject[] { DefaultSkills.Tactics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectTacticsTroopSacrificeReduction.Initialize(new TextObject("{=VHdyQYKI}Decrease the sacrificed troop number when trying to get away +{a0}%", null), new SkillObject[] { DefaultSkills.Tactics }, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectTrackingRadius.Initialize(new TextObject("{=kqJipMqc}Track detection radius +{a0}%", null), new SkillObject[] { DefaultSkills.Scouting }, SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            ____effectTrackingLevel.Initialize(new TextObject("{=aGecGUub}Max track difficulty that can be detected: {a0}", null), new SkillObject[] { DefaultSkills.Scouting }, SkillEffect.PerkRole.Scout, 1f, SkillEffect.PerkRole.None, 1f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            ____effectTrackingSpottingDistance.Initialize(new TextObject("{=lbrOAvKj}Spotting distance +{a0}%", null), new SkillObject[] { DefaultSkills.Scouting }, SkillEffect.PerkRole.Scout, 0.06f, SkillEffect.PerkRole.None, 0.03f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            ____effectTrackingTrackInformation.Initialize(new TextObject("{=uNls3bOP}Track information level: {a0}", null), new SkillObject[] { DefaultSkills.Scouting }, SkillEffect.PerkRole.Scout, 0.04f, SkillEffect.PerkRole.None, 0.03f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            
            ____effectRogueryLootBonus.Initialize(new TextObject("{=bN3bLDb2}Battle Loot +{a0}%", null), new SkillObject[] { DefaultSkills.Roguery }, SkillEffect.PerkRole.PartyLeader, 0.25f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectCharmRelationBonus.Initialize(new TextObject("{=c5dsio8Q}Relation increase with NPCs +{a0}%", null), new SkillObject[] { DefaultSkills.Charm }, SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectTradePenaltyReduction.Initialize(new TextObject("{=uq7JwT1Z}Trade penalty Reduction +{a0}%", null), new SkillObject[] { DefaultSkills.Trade }, SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectLeadershipMoraleBonus.Initialize(new TextObject("{=n3bFiuVu}Increase morale of the parties under your command +{a0}", null), new SkillObject[] { DefaultSkills.Leadership }, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            ____effectLeadershipGarrisonSizeBonus.Initialize(new TextObject("{=cSt26auo}Increase garrison size by +{a0}", null), new SkillObject[] { DefaultSkills.Leadership }, SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            
            ____effectSurgeonSurvivalBonus.Initialize(new TextObject("{=w4BzNJYl}Casualty survival chance +{a0}%", null), new SkillObject[] { DefaultSkills.Medicine }, SkillEffect.PerkRole.Surgeon, 0.01f, SkillEffect.PerkRole.None, 0.01f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            ____effectHealingRateBonusForHeroes.Initialize(new TextObject("{=fUvs4g40}Healing rate increase for heroes +{a0}%", null), new SkillObject[] { DefaultSkills.Medicine }, SkillEffect.PerkRole.Surgeon, 0.5f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectHealingRateBonusForRegulars.Initialize(new TextObject("{=A310vHqJ}Healing rate increase for troops +{a0}%", null), new SkillObject[] { DefaultSkills.Medicine }, SkillEffect.PerkRole.Surgeon, 1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectGovernorHealingRateBonus.Initialize(new TextObject("{=6mQGst9s}Healing rate increase +{a0}%", null), new SkillObject[] { DefaultSkills.Medicine }, SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            
            ____effectSiegeEngineProductionBonus.Initialize(new TextObject("{=spbYlf0y}Faster siege engine production +{a0}%", null), new SkillObject[] { DefaultSkills.Engineering }, SkillEffect.PerkRole.Engineer, 1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectTownProjectBuildingBonus.Initialize(new TextObject("{=2paRqO8u}Faster building production +{a0}%", null), new SkillObject[] { DefaultSkills.Engineering }, SkillEffect.PerkRole.Governor, 2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
            ____effectEngineerLevel.Initialize(new TextObject("{=aQduWCrg}Max difficulty of siege engine that can be built: {a0}", null), new SkillObject[] { DefaultSkills.Engineering }, SkillEffect.PerkRole.Engineer, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);

            ____effectStewardPartySizeBonus.Initialize(new TextObject("{=jNDUXetG}Increase party size by +{a0}", null), new SkillObject[] { DefaultSkills.Steward }, SkillEffect.PerkRole.Quartermaster, 0.25f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
            
            return false;
        }
    }
}
