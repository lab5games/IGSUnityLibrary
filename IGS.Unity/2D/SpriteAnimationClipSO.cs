using System;
using UnityEngine;

namespace IGS.Unity
{
    [CreateAssetMenu(menuName ="IGS Library/2D/Sprite Animation Clip", fileName ="New Clip")]
    public class SpriteAnimationClipSO : ScriptableObject
    {
        public enum WrapMode
        {
            Once,
            LoopRestart
        }

        [SerializeField] string clipName = "clip";
        [SerializeField] int fps = 30;
        [SerializeField] WrapMode wrapMode = WrapMode.Once;
        [SerializeField] Sprite[] frames = null;

        public string ClipName { get { return clipName; } }

        public int FPS { get { return fps; } }

        public WrapMode Wrap
        {
            get { return wrapMode; }
            set { wrapMode = value; }
        }

        public int TotalFrames { get { return frames.Length; } }  
        
        public float SecondsPerFrame { get { return 1f / fps; } }

        public float Length { get { return SecondsPerFrame * TotalFrames; } }

        public Sprite this[int index] { get { return frames[index]; } }
    }
}
