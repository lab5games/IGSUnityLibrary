using System.Collections.Generic;
using UnityEngine.Assertions.Comparers;

namespace IGS.Unity.Tasks
{
    public static class Yields
    {
        public static readonly UnityEngine.WaitForEndOfFrame NextFrame = new UnityEngine.WaitForEndOfFrame();

        static Dictionary<float, UnityEngine.WaitForSeconds> _secondsDict = new Dictionary<float, UnityEngine.WaitForSeconds>(new FloatComparer());

        public static UnityEngine.WaitForSeconds WaitSeconds(float waitSeconds)
        {
            UnityEngine.WaitForSeconds wfs = null;

            if(!_secondsDict.TryGetValue(waitSeconds, out wfs))
            {
                wfs = new UnityEngine.WaitForSeconds(waitSeconds);
                _secondsDict[waitSeconds] = wfs;
            }

            return wfs;
        }
    }
}
