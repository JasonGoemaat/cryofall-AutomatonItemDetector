using AtomicTorch.CBND.GameApi.Scripting;
using CryoFall.Automaton.Features;

namespace CryoFall.Automaton
{
  public class ItemDetectorBootstrapperClient : BaseBootstrapper
  {
    public override void ClientInitialize()
    {
      AutomatonManager.AddFeature(FeatureItemDetector.Instance);
    }
  }
}