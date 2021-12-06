using AtomicTorch.CBND.GameApi.Scripting;
using CryoFall.Automaton.Features;

namespace CryoFall.Automaton
{
  public class ItemDetectorBootstrapperClient : BaseBootstrapper
  {
    public override void ClientInitialize()
    {
      // Automaton automatically finds features
      // AutomatonManager.AddFeature(FeatureItemDetector.Instance);
    }
  }
}