using System.Collections.Generic;
using Newtonsoft.Json;
using BepInEx.Logging;
using Haru.FileChecker;
using Haru.IO;

namespace Haru.Client.FileChecker
{
    public class ConsistencyHelper
    {
        private const string _bundleRootPath = "EscapeFromTarkov_Data/StreamingAssets/Windows/";
        public static Dictionary<string, ConsistencyResult> Results { get; private set; }

        private static ManualLogSource _logger;

        static ConsistencyHelper()
        {
            Results = new Dictionary<string, ConsistencyResult>();
            _logger = Logger.CreateLogSource(nameof(ConsistencyHelper));
        }

        public static string GetFullBundlePath(string bundlepath)
        {
            return VFS.CombinePath(_bundleRootPath, bundlepath).Replace("\\", "/");
        }

        public static void RunValidation()
        {
            var text = VFS.ReadText("ConsistencyInfo");
            var info = JsonConvert.DeserializeObject<ConsistencyInfo>(text);
            Results = ConsistencyChecker.ValidateFiles(string.Empty, info.Entries);
        }

        public static void ValidateFile(string filepath)
        {
            if (!Results.ContainsKey(filepath))
            {
                _logger.LogWarning($"[ConsistencyHelper] {filepath} no metadata.");
                return;
            }

            var result = Results[filepath];

            switch (result)
            {
                case ConsistencyResult.FileNotFound:
                    _logger.LogWarning($"[ConsistencyHelper] {filepath} doesn't exist.");
                    break;

                case ConsistencyResult.FileSizeMismatch:
                    _logger.LogWarning($"[ConsistencyHelper] {filepath} size mismatch.");
                    break;

                case ConsistencyResult.FileHashMismatch:
                    _logger.LogWarning($"[ConsistencyHelper] {filepath} hash mismatch.");
                    break;

                case ConsistencyResult.FileChecksumMismatch:
                    _logger.LogWarning($"[ConsistencyHelper] {filepath} checksum mismatch.");
                    break;

                case ConsistencyResult.Success:
                default:
                    break;
            }
        }
    }
}
