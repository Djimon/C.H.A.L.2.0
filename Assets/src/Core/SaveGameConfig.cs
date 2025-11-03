using UnityEngine;

namespace CHAL.Core
{
    [CreateAssetMenu(fileName = "GameSaveConfig", menuName = "Config/GameSaveConfig")]
    public sealed class GameSaveConfig : ScriptableObject
    {
        [Header("Format")]
        public bool useJsonInEditor = true;      // .json im Editor
        public bool encodeInPlayer = true;       // .dat (encoded) im Player
        [Tooltip("Nicht hartkodieren – per Bootstrap/BuildConfig setzen.")]
        public string encodePassword = "changeme";

        [Header("Paths")]
        public string baseFolder = "profiles";
        public string singleProfileFolder = "main";
        public string fileStem = "profile";
        public string extensionJson = "json";
        public string extensionDat = "dat";

/// <summary>
/// Resolves the file ID at runtime based on the current settings.
/// </summary>
/// <returns>The resolved file ID as a string.</returns>
        public string ResolveFileIdRuntime()
        {
#if UNITY_EDITOR
            bool json = useJsonInEditor;
#else
            bool json = !encodeInPlayer ? true : false; // wenn nicht encodiert, dann json; sonst dat
#endif
            string ext = json ? extensionJson : extensionDat;
            return $"{baseFolder}/{singleProfileFolder}/{fileStem}.{ext}";
        }

/// <summary>
/// Determines if runtime encoding should be applied.
/// </summary>
/// <returns>True if encoding is needed; otherwise, false.</returns>
        public bool ShouldEncodeRuntime()
        {
#if UNITY_EDITOR
            return false; // Editor standard: kein Encoding (diffbar)
#else
            return encodeInPlayer;
#endif
        }
    }
}
