using UnityEngine;

namespace Dissertation.Core
{
    public class PlatformPicker : MonoBehaviour
    {
        public PlatformStatics.BuildPlatform platform = PlatformStatics.BuildPlatform.MOBILE;

        private void Awake()
        {
            PlatformStatics.platform = platform;
        }
    }
}

