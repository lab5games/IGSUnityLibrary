using UnityEngine;
using IGS.Unity.Pool;

namespace IGS.Unity.Audio
{
    internal class SoundEmitterPool
    {
        AudioManager _audioManager;
        IObjectPool<SoundEmitter> _pool;

        public SoundEmitterPool(AudioManager audioManager)
        {
            _audioManager = audioManager;
            _pool = new ObjectPool<SoundEmitter>(OnCreate);
        }

        public SoundEmitter Request()
        {
            return _pool.Get();
        }

        public void Recycle(SoundEmitter soundEmitter)
        {
            _pool.Recycle(soundEmitter);
        }

        private SoundEmitter OnCreate()
        {
            var audioSource = new GameObject("SoundEmitter").AddComponent<AudioSource>();
            audioSource.transform.parent = _audioManager.transform;

            return new SoundEmitter(audioSource);
        }
    }
}
