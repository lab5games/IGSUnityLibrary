using UnityEngine;
using UnityEngine.UI;

namespace IGS.Unity
{
    public static class UIExtensions
    {
        public static void SetAlpha(this Image image, float alpha)
        {
            Color col = image.color;
            col.a = Mathf.Clamp01(alpha);
            image.color = col;
        }
    }
}
