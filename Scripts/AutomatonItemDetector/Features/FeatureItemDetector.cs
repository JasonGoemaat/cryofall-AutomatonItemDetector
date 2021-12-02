namespace CryoFall.Automaton.Features
{
  using AtomicTorch.CBND.CoreMod.StaticObjects;
  using AtomicTorch.CBND.CoreMod.StaticObjects.Deposits;
  using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
  using AtomicTorch.CBND.CoreMod.StaticObjects.Minerals;
  using AtomicTorch.CBND.CoreMod.StaticObjects.Misc.Events;
  using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
  using AtomicTorch.CBND.CoreMod.Systems.Resources;
  using AtomicTorch.CBND.GameApi.Data;
  using AtomicTorch.CBND.GameApi.Scripting;
  using CryoFall.Automaton.ClientSettings;
  using System.Collections.Generic;

  public class FeatureItemDetector : ProtoFeatureNearbyObjects<FeatureItemDetector>
  {
    private FeatureItemDetector() { }


    public override string Name => "AutoItemDetector";

    public override string Description => "Find items in your area.";

    protected override void PrepareFeature(List<IProtoEntity> entityList, List<IProtoEntity> requiredItemList)
    {
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectMineral>());
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectHackableContainer>());
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectVegetation>());
      entityList.AddRange(Api.FindProtoEntities<ObjectGroundItemsContainer>());
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectLoot>());
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectDeposit>());
      entityList.AddRange(Api.FindProtoEntities<IProtoObjectGatherable>());      
    }

    //public override void PrepareOptions(SettingsFeature settingsFeature)
    //{
    //  AddOptionIsEnabled(settingsFeature);
    //}
  }
}
