// To allow modders to modify existing EFT files.
// This one is for all other EFT files.

using System.Reflection;
using System.Threading.Tasks;
using EFT;
using Haru.Reflection;
using Haru.Client.Shared;

namespace Haru.Client.FileChecker.Patches
{
    public class ConsistencyGeneral : APatch
    {
        public ConsistencyGeneral() : base()
        {
            Id = "com.haru.client.consistencygeneral";
            Type = EPatchType.Prefix;
        }

        protected override MethodBase GetOriginalMethod()
        {
            var flags = PatchConsts.PrivateFlags;
            return typeof(TarkovApplication).BaseType
                .GetMethod("RunFilesChecking", flags);
        }

        protected static bool Patch(ref Task __result)
        {
            foreach (var filepath in ConsistencyHelper.Results.Keys)
            {
                ConsistencyHelper.ValidateFile(filepath);
            }

            __result = Task.CompletedTask;
            return false;
        }
    }
}