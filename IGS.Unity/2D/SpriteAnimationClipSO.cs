using UnityEngine;

namespace IGS.Unity
{
    [CreateAssetMenu(menuName ="IGS Unity/2D/Sprite Animation Clip", fileName ="New Clip")]
    public class SpriteAnimationClipSO : ScriptableObject
    {
        public enum WrapMode
        {
            Once,
            LoopRestart
        }

        [SerializeField] string clipName = "clip";
        [SerializeField, Min(1)] int fps = 30;
        [SerializeField] WrapMode wrapMode = WrapMode.Once;
        [SerializeField] Sprite[] frames = null;

        public string ClipName { get { return clipName; } }

        public int FPS { get { return fps; } }

        public WrapMode Wrap
        {
            get { return wrapMode; }
            set { wrapMode = value; }
        }

        public int TotalFrames { get { return (frames == null) ? 0 : frames.Length; } }  
        
        public float SecondsPerFrame { get { return 1f / fps; } }

        public float Length { get { return SecondsPerFrame * TotalFrames; } }

        public Sprite this[int index] { get { return (frames == null || frames.Length == 0) ? null : frames[index]; } }
    }
}
