
private void AddSkinArmorWeaponMultiMeshesToEntity(uint teamColor1, uint teamColor2, bool needBatchedVersion, bool forceUseFaceCache = false)
{
    this.AddSkinMeshesToEntity((int)this._data.EquipmentData.GetSkinMeshesMask(), !needBatchedVersion, forceUseFaceCache);
    this.AddArmorMultiMeshesToAgentEntity(teamColor1, teamColor2);
    int hashCode = this._data.BodyPropertiesData.GetHashCode();
    for (int i = 0; i < 5; i++)
    {
        if (!this._data.EquipmentData[i].IsEmpty)
        {
            MissionWeapon missionWeapon = new MissionWeapon(this._data.EquipmentData[i].Item, this._data.EquipmentData[i].ItemModifier, this._data.BannerData);
            if (this._data.AddColorRandomnessData)
            {
                missionWeapon.SetRandomGlossMultiplier(hashCode);
            }
            WeaponData weaponData = missionWeapon.GetWeaponData(needBatchedVersion);
            WeaponData ammoWeaponData = missionWeapon.GetAmmoWeaponData(needBatchedVersion);
            this._data.AgentVisuals.AddWeaponToAgentEntity(i, weaponData, missionWeapon.GetWeaponStatsData(), ammoWeaponData, missionWeapon.GetAmmoWeaponStatsData(), this._data.GetCachedWeaponEntity((EquipmentIndex)i));
            weaponData.DeinitializeManagedPointers();
            ammoWeaponData.DeinitializeManagedPointers();
        }
    }
    this._data.AgentVisuals.SetWieldedWeaponIndices(this._data.RightWieldedItemIndexData, this._data.LeftWieldedItemIndexData);
    for (int j = 0; j < 5; j++)
    {
        if (!this._data.EquipmentData[j].IsEmpty && this._data.EquipmentData[j].Item.PrimaryWeapon.IsConsumable)
        {
            short num = this._data.EquipmentData[j].Item.PrimaryWeapon.MaxDataValue;
            if (j == this._data.RightWieldedItemIndexData)
            {
                num -= 1;
            }
            this._data.AgentVisuals.UpdateQuiverMeshesWithoutAgent(j, (int)num);
        }
    }
}

public void AddArmorMultiMeshesToAgentEntity(uint teamColor1, uint teamColor2)
{
    Random randomGenerator = null;
    uint color3;
    uint color4;
    if (this._data.AddColorRandomnessData)
    {
        int hashCode = this._data.BodyPropertiesData.GetHashCode();
        randomGenerator = new Random(hashCode);
        Color color;
        Color color2;
        AgentVisuals.GetRandomClothingColors(hashCode, Color.FromUint(teamColor1), Color.FromUint(teamColor2), out color, out color2);
        color3 = color.ToUnsignedInteger();
        color4 = color2.ToUnsignedInteger();
    }
    else
    {
        color3 = teamColor1;
        color4 = teamColor2;
    }
    for (EquipmentIndex equipmentIndex = EquipmentIndex.HorseHarness; equipmentIndex >= EquipmentIndex.WeaponItemBeginSlot; equipmentIndex--)
    {
        if (equipmentIndex == EquipmentIndex.NumAllWeaponSlots || equipmentIndex == EquipmentIndex.Body || equipmentIndex == EquipmentIndex.Leg || equipmentIndex == EquipmentIndex.Gloves || equipmentIndex == EquipmentIndex.Cape)
        {
            ItemObject item = this._data.EquipmentData[(int)equipmentIndex].Item;
            ItemObject itemObject = this._data.EquipmentData[(int)equipmentIndex].CosmeticItem ?? item;
            if (itemObject != null)
            {
                bool isFemale = this._data.BodyPropertiesData.Age >= 14f && this._data.SkeletonTypeData == SkeletonType.Female;
                bool hasGloves = equipmentIndex == EquipmentIndex.Body && this._data.EquipmentData[EquipmentIndex.Gloves].Item != null;
                MetaMesh multiMesh = this._data.EquipmentData[(int)equipmentIndex].GetMultiMesh(isFemale, hasGloves, true);
                if (multiMesh != null)
                {
                    if (this._data.AddColorRandomnessData)
                    {
                        multiMesh.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(randomGenerator));
                    }
                    if (itemObject.IsUsingTableau && this._data.BannerData != null)
                    {
                        for (int i = 0; i < multiMesh.MeshCount; i++)
                        {
                            Mesh currentMesh = multiMesh.GetMeshAtIndex(i);
                            Mesh currentMesh3 = currentMesh;
                            if (currentMesh3 != null && !currentMesh3.HasTag("dont_use_tableau"))
                            {
                                Mesh currentMesh2 = currentMesh;
                                if (currentMesh2 != null && currentMesh2.HasTag("banner_replacement_mesh"))
                                {
                                    BannerVisual bannerVisual = (BannerVisual)this._data.BannerData.BannerVisual;
                                    BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
                                    bannerVisual.GetTableauTextureLarge(bannerDebugInfo, delegate(Texture t)
                                    {
                                        this.ApplyBannerTextureToMesh(currentMesh, t);
                                    }, true);
                                    currentMesh.ManualInvalidate();
                                    break;
                                }
                            }
                            currentMesh.ManualInvalidate();
                        }
                    }
                    else if (itemObject.IsUsingTeamColor)
                    {
                        for (int j = 0; j < multiMesh.MeshCount; j++)
                        {
                            Mesh meshAtIndex = multiMesh.GetMeshAtIndex(j);
                            if (!meshAtIndex.HasTag("no_team_color"))
                            {
                                meshAtIndex.Color = color3;
                                meshAtIndex.Color2 = color4;
                                Material material = meshAtIndex.GetMaterial().CreateCopy();
                                material.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
                                meshAtIndex.SetMaterial(material);
                            }
                            meshAtIndex.ManualInvalidate();
                        }
                    }
                    if (itemObject.UsingFacegenScaling)
                    {
                        Skeleton skeleton = this._data.AgentVisuals.GetSkeleton();
                        multiMesh.UseHeadBoneFaceGenScaling(skeleton, this._data.MonsterData.HeadLookDirectionBoneIndex, this._data.AgentVisuals.GetFacegenScalingMatrix());
                        skeleton.ManualInvalidate();
                    }
                    this._data.AgentVisuals.AddMultiMesh(multiMesh, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
                    multiMesh.ManualInvalidate();
                }
            }
        }
    }
}

[BannerlordConfig.ConfigPropertyUnbounded]
public static float UIScale { get; set; } = 1f;

public override bool CanPlayerCreateArmy(out TextObject disabledReason)
{
	if (Clan.PlayerClan.Kingdom == null)
	{
		disabledReason = new TextObject("{=XSQ0Y9gy}You need to be a part of a kingdom to create an army.", null);
		return false;
	}
	if (Clan.PlayerClan.IsUnderMercenaryService)
	{
		disabledReason = new TextObject("{=aRhQzJca}Mercenaries cannot create or manage armies.", null);
		return false;
	}
	if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
	{
		disabledReason = new TextObject("{=NAA4pajB}You need to leave your current army to create a new one.", null);
		return false;
	}
	if (MobileParty.MainParty.IsCurrentlyAtSea)
	{
		disabledReason = GameTexts.FindText("str_cannot_gather_army_at_sea", null);
		return false;
	}
	if (Hero.MainHero.IsPrisoner)
	{
		disabledReason = GameTexts.FindText("str_action_disabled_reason_prisoner", null);
		return false;
	}
	if (MobileParty.MainParty.IsInRaftState)
	{
		disabledReason = GameTexts.FindText("str_action_disabled_reason_raft_state", null);
		return false;
	}
	if (CampaignMission.Current != null)
	{
		disabledReason = new TextObject("{=FdzsOvDq}This action is disabled while in a mission", null);
		return false;
	}
	if (PlayerEncounter.Current != null)
	{
		if (PlayerEncounter.EncounterSettlement == null)
		{
			disabledReason = GameTexts.FindText("str_action_disabled_reason_encounter", null);
			return false;
		}
		Village village = PlayerEncounter.EncounterSettlement.Village;
		if (village != null && village.VillageState == Village.VillageStates.BeingRaided)
		{
			MapEvent mapEvent = MobileParty.MainParty.MapEvent;
			if (mapEvent != null && mapEvent.IsRaid)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_raid", null);
				return false;
			}
		}
		if (PlayerEncounter.EncounterSettlement.IsUnderSiege)
		{
			disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
			return false;
		}
	}
	else
	{
		if (PlayerSiege.PlayerSiegeEvent != null)
		{
			disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
			return false;
		}
		if (MobileParty.MainParty.MapEvent != null)
		{
			disabledReason = new TextObject("{=MIylzRc5}You can't perform this action while you are in a map event.", null);
			return false;
		}
	}
	disabledReason = TextObject.GetEmpty();
	return true;
}
