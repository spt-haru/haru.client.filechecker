// To allow modders to modify existing EFT files.
// This one is specifically related to bundles.

using System.Reflection;
using System.Threading.Tasks;
using EFT;
using Haru.Reflection;
using Haru.Client.Shared;

namespace Haru.Client.FileChecker.Patches
{
    public class ConsistencyBundles : APatch
    {
        public ConsistencyBundles() : base()
        {
            Id = "com.haru.client.consistencybundles";
            Type = EPatchType.Prefix;
        }

        protected override MethodBase GetOriginalMethod()
        {
            var flags = PatchConsts.PrivateFlags;
            return typeof(TarkovApplication).BaseType
                .GetMethod("DefaultBundleCheck", flags);
        }

        protected static bool Patch(ref Task __result, string bundlePath)
        {
            var filepath = ConsistencyHelper.GetFullBundlePath(bundlePath);
            ConsistencyHelper.ValidateFile(filepath);

            __result = Task.CompletedTask;
            return false;
        }
    }
}