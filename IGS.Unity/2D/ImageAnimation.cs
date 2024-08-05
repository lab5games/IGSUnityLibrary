using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace IGS.Unity
{
    [System.Serializable]
    public class SpriteAnimationEvent : UnityEvent<int> { }

    [RequireComponent(typeof(Image))]
    [AddComponentMenu("IGS Library/2D/Image Animation")]
    public class ImageAnimation : MonoBehaviour
    {
        [SerializeField] SpriteAnimationClipSO defaultClip = null;
        [SerializeField] float speed = 1f;
        [SerializeField] bool playOnStart = false;
        [SerializeField] bool autoResetSize = false;

        [Space]
        public SpriteAnimationEvent onFinish;

        bool _playing = false;
        bool _ending = false;
        float _elapsedTime = 0;
        int _finishedCount = 0;

        Image _renderer = null;

        public Image Renderer
        {
            get
            {
                if(_renderer == null)
                    _renderer = GetComponent<Image>();  

                return _renderer;
            }
        }

        public SpriteAnimationClipSO Clip { get; private set; }

        public int CurrentFrame { get; private set; }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool AutoResetSize
        {
            get { return autoResetSize; }
            set { autoResetSize = value; }
        }

        public float Transparency
        {
            get { return Renderer.color.a; }
            set { Renderer.SetAlpha(value); }
        }

        public bool IsPlaying
        {
            get
            {
                if(Clip == null)
                    return false;


                return enabled && _playing;
            }
        }

        #region Unity Calls
        void Start()
        {
            if(playOnStart)
            {
                Play(defaultClip); 
            }
        }

        void Update()
        {
            UpdateFrame();
        }
        #endregion

        public void Play(SpriteAnimationClipSO clip, int startFrame = 0)
        {
            Clip = clip;
            CurrentFrame = startFrame;

            _playing = true;
            _ending = false;
            _elapsedTime = startFrame * clip.SecondsPerFrame;
            _finishedCount = 0;

            // set sprite
            Renderer.sprite = Clip[CurrentFrame];
        }

        public void Stop(bool resetFrame = true)
        {
            _playing = false;

            if(resetFrame && Clip)
            {
                Renderer.sprite = Clip[0];
            }
        }

        private void UpdateFrame()
        {
            if(IsPlaying)
            {
                _elapsedTime += Time.deltaTime * speed;
                float clampTime = Mathf.Clamp(_elapsedTime, 0, Clip.Length);

                if(clampTime < Clip.Length)
                {
                    // keep forward playing
                    _ending = false;
                }
                else
                {
                    _ending = true;

                    switch(Clip.Wrap)
                    {
                        case SpriteAnimationClipSO.WrapMode.LoopRestart:
                            {
                                clampTime = _elapsedTime - Clip.Length;
                                _elapsedTime = clampTime;
                                break;
                            }
                    }
                }

                // fetech desired frame
                int desiredFrame = Mathf.FloorToInt(clampTime / Clip.SecondsPerFrame);

                if(desiredFrame != CurrentFrame)
                {
                    // frame changed
                    CurrentFrame = desiredFrame;
                    Renderer.sprite = Clip[CurrentFrame];

                    if(AutoResetSize)
                        Renderer.SetNativeSize();
                }

                if(_ending)
                {
                    ++_finishedCount;

                    if(onFinish != null)
                        onFinish.Invoke(_finishedCount);

                    if(Clip.Wrap == SpriteAnimationClipSO.WrapMode.Once)
                    {
                        _playing = false;
                    }
                }
            }
        }
    }
}
