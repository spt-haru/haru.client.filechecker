using BepInEx;
using Haru.Reflection;
using Haru.Client.FileChecker.Patches;

namespace Haru.Client.FileChecker
{
    [BepInPlugin("com.Haru.Client.FileChecker", "Haru.Client.FileChecker", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly APatch[] _patches;

        public Plugin()
        {
            _patches = new APatch[]
            {
                new ConsistencyBundles(),
                new ConsistencyGeneral()
            };
        }

        // used by bepinex
        private void Awake()
        {
            Logger.LogInfo("Loading: Haru.FileChecker");

            foreach (var patch in _patches)
            {
                patch.Enable();
            }

            ConsistencyHelper.RunValidation();
        }

        // used by bepinex
        private void OnApplicationQuit()
        {
            foreach (var patch in _patches)
            {
                patch.Disable();
            }
        }
    }
}