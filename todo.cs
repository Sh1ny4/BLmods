
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

void IMissionListener.OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
{
	switch (creationType)
	{
	case Agent.CreationType.FromRoster:
	case Agent.CreationType.FromCharacterObj:
	{
		bool useTeamColor = false;
		Random randomGenerator = null;
		bool randomizeColors = agent.RandomizeColors;
		uint color3;
		uint color4;
		if (randomizeColors)
		{
			int bodyPropertiesSeed = agent.BodyPropertiesSeed;
			randomGenerator = new Random(bodyPropertiesSeed);
			Color color;
			Color color2;
			AgentVisuals.GetRandomClothingColors(bodyPropertiesSeed, Color.FromUint(agent.ClothingColor1), Color.FromUint(agent.ClothingColor2), out color, out color2);
			color3 = color.ToUnsignedInteger();
			color4 = color2.ToUnsignedInteger();
		}
		else
		{
			color3 = agent.ClothingColor1;
			color4 = agent.ClothingColor2;
		}
		for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
		{
			if (!agent.SpawnEquipment[equipmentIndex].IsVisualEmpty)
			{
				ItemObject itemObject = agent.SpawnEquipment[equipmentIndex].CosmeticItem ?? agent.SpawnEquipment[equipmentIndex].Item;
				bool hasGloves = equipmentIndex == EquipmentIndex.Body && agent.SpawnEquipment[EquipmentIndex.Gloves].Item != null;
				bool isFemale = agent.Age >= 14f && agent.IsFemale;
				MetaMesh multiMesh = agent.SpawnEquipment[equipmentIndex].GetMultiMesh(isFemale, hasGloves, true);
				if (multiMesh != null)
				{
					if (randomizeColors)
					{
						multiMesh.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(randomGenerator));
					}
					if (!itemObject.IsUsingTableau)
					{
						goto IL_226;
					}
					bool flag;
					if (agent == null)
					{
						flag = (null != null);
					}
					else
					{
						IAgentOriginBase origin = agent.Origin;
						flag = (((origin != null) ? origin.Banner : null) != null);
					}
					if (!flag)
					{
						goto IL_226;
					}
					for (int i = 0; i < multiMesh.MeshCount; i++)
					{
						Mesh currentMesh = multiMesh.GetMeshAtIndex(i);
						Mesh currentMesh3 = currentMesh;
						if (currentMesh3 != null && !currentMesh3.HasTag("dont_use_tableau"))
						{
							Mesh currentMesh2 = currentMesh;
							if (currentMesh2 != null && currentMesh2.HasTag("banner_replacement_mesh"))
							{
								BannerVisual bannerVisual = (BannerVisual)agent.Origin.Banner.BannerVisual;
								BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
								bannerVisual.GetTableauTextureLarge(bannerDebugInfo, delegate(Texture t)
								{
									MissionScreen.ApplyBannerTextureToMesh(currentMesh, t);
								}, true);
								currentMesh.ManualInvalidate();
								break;
							}
						}
						currentMesh.ManualInvalidate();
					}
					IL_29B:
					if (itemObject.UsingFacegenScaling)
					{
						multiMesh.UseHeadBoneFaceGenScaling(agent.AgentVisuals.GetSkeleton(), agent.Monster.HeadLookDirectionBoneIndex, agent.AgentVisuals.GetFacegenScalingMatrix());
					}
					Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
					int num = (skeleton != null) ? skeleton.GetComponentCount(GameEntity.ComponentType.ClothSimulator) : -1;
					agent.AgentVisuals.AddMultiMesh(multiMesh, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
					multiMesh.ManualInvalidate();
					int num2 = (skeleton != null) ? skeleton.GetComponentCount(GameEntity.ComponentType.ClothSimulator) : -1;
					if (skeleton != null && equipmentIndex == EquipmentIndex.Cape && num2 > num)
					{
						GameEntityComponent componentAtIndex = skeleton.GetComponentAtIndex(GameEntity.ComponentType.ClothSimulator, num2 - 1);
						agent.SetCapeClothSimulator(componentAtIndex);
						goto IL_342;
					}
					goto IL_342;
					IL_226:
					if (itemObject.IsUsingTeamColor)
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
								useTeamColor = true;
							}
							meshAtIndex.ManualInvalidate();
						}
						goto IL_29B;
					}
					goto IL_29B;
				}
				IL_342:
				if (equipmentIndex == EquipmentIndex.Body && !string.IsNullOrEmpty(itemObject.ArmBandMeshName))
				{
					MetaMesh copy = MetaMesh.GetCopy(itemObject.ArmBandMeshName, true, true);
					if (copy != null)
					{
						if (randomizeColors)
						{
							copy.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(randomGenerator));
						}
						if (itemObject.IsUsingTeamColor)
						{
							for (int k = 0; k < copy.MeshCount; k++)
							{
								Mesh meshAtIndex2 = copy.GetMeshAtIndex(k);
								if (!meshAtIndex2.HasTag("no_team_color"))
								{
									meshAtIndex2.Color = color3;
									meshAtIndex2.Color2 = color4;
									Material material2 = meshAtIndex2.GetMaterial().CreateCopy();
									material2.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
									meshAtIndex2.SetMaterial(material2);
									useTeamColor = true;
								}
								meshAtIndex2.ManualInvalidate();
							}
						}
						agent.AgentVisuals.AddMultiMesh(copy, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
						copy.ManualInvalidate();
					}
				}
			}
		}
		ItemObject item = agent.SpawnEquipment[EquipmentIndex.Body].Item;
		if (item != null)
		{
			int lodAtlasIndex = item.LodAtlasIndex;
			if (lodAtlasIndex != -1)
			{
				agent.AgentVisuals.SetLodAtlasShadingIndex(lodAtlasIndex, useTeamColor, agent.ClothingColor1, agent.ClothingColor2);
			}
		}
		break;
	}
	case Agent.CreationType.FromHorseObj:
		MountVisualCreator.AddMountMeshToAgentVisual(agent.AgentVisuals, agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot].Item, agent.SpawnEquipment[EquipmentIndex.HorseHarness].Item, agent.HorseCreationKey, agent);
		break;
	}
	ArmorComponent.ArmorMaterialTypes bodyArmorMaterialType = ArmorComponent.ArmorMaterialTypes.None;
	ItemObject item2 = agent.SpawnEquipment[EquipmentIndex.Body].Item;
	if (item2 != null)
	{
		bodyArmorMaterialType = item2.ArmorComponent.MaterialType;
	}
	agent.SetBodyArmorMaterialType(bodyArmorMaterialType);
}
