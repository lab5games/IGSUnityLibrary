using UnityEngine;
using UnityEngine.Audio;

namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/AudioManager.cs
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioMixer audioMixer = null;

        SoundEmitterPool _soundEmitterPool = null;
        SoundEmitterVault _soundEmitterVault = null;
        SoundEmitter _musicSoundEmitter = null;

        [Header("Settings")]
        [SerializeField] float musicFadeDuration = 2;


        public float MusicFadeDuration
        {
            get { return musicFadeDuration; }
            set { musicFadeDuration = value; }
        }

        #region Unity Calls
        void Start()
        {
            _soundEmitterPool = new SoundEmitterPool(this);
            _soundEmitterVault = new SoundEmitterVault();
        }
        #endregion


        #region Volume
        public void SetGroupVolume(string parameterName, float normalizedVolume)
        {
            if(audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume)))
            {
                GameLogger.Log(string.Format("Set {0} volume {1}", parameterName, normalizedVolume), LogFilter.System, this);
            }
            else
            {
                GameLogger.Log(string.Format("The AudioMixer parameter({0}) was not found", parameterName), LogFilter.Error, this);
            }
        }

        public float GetGroupVolume(string parameterName)
        {
            if(audioMixer.GetFloat(parameterName, out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                GameLogger.Log(string.Format("The AudioMixer parameter({0}) was not found", parameterName), LogFilter.Error, this);
                return 0f;
            }
        }

        private float NormalizedToMixerValue(float normalizedValue)
        {
            // [0 to 1] becomes [-80db to 0db]
            return (normalizedValue - 1f) * 80f;
        }

        private float MixerValueToNormalized(float mixerValue)
        {
            // [-80db to 0db] becomes [0 to 1]
            return 1f + (mixerValue / 80f);
        }
        #endregion


        #region Play Controls
        public AudioCueKey PlayMusic(AudioCueSO audioCue, AudioConfigurationSO settings)
        {
            if(_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying)
            {
                AudioClip soundToPlay = audioCue.GetClips()[0];
                if(_musicSoundEmitter.Clip == soundToPlay)
                    return AudioCueKey.Invalide;

                _musicSoundEmitter.FadeMusicOut(MusicFadeDuration);
            }


            _musicSoundEmitter = _soundEmitterPool.Request();
            _musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], settings, MusicFadeDuration);
            _musicSoundEmitter.onSoundFinishedPlaying += StopMusicEmitter;

            return AudioCueKey.Invalide;
        }

        public bool StopMusic()
        {
            if(_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying)
            {
                _musicSoundEmitter.Stop();
                return true;
            }

            return false;
        }

        public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings)
        {
            return PlayAudioCue(audioCue, settings, Vector3.zero);
        }

        public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position)
        {
            AudioCueKey audioCueKey = _soundEmitterVault.GetKey(audioCue);
            AudioClip[] clipsToPlay = audioCue.GetClips();
            SoundEmitter[] soundEmitters = new SoundEmitter[clipsToPlay.Length];

            for(int i=0; i<clipsToPlay.Length; i++)
            {
                soundEmitters[i] = _soundEmitterPool.Request();

                if(soundEmitters[i] != null)
                {
                    soundEmitters[i].audioCueKey = audioCueKey;
                    soundEmitters[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.Looping, position);

                    if(!audioCue.Looping)
                    {
                        soundEmitters[i].onSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                    }
                }
            }

            return audioCueKey;
        }

        public bool FinishAudioCue(AudioCueKey key)
        {
            SoundEmitter[] soundEmitters = null;
            bool isFound = _soundEmitterVault.Get(key, out soundEmitters);

            if(isFound)
            {
                for(int i = 0; i < soundEmitters.Length; i++)
                {
                    soundEmitters[i].Finish();
                    soundEmitters[i].onSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }

            return isFound;
        }

        public bool StopAudioCue(AudioCueKey key)
        {
            SoundEmitter[] soundEmitters = null;
            bool isFound = _soundEmitterVault.Get(key, out soundEmitters);

            if(isFound)
            {
                for(int i=0; i<soundEmitters.Length; i++)
                {
                    StopAndCleanSoundEmitter(soundEmitters[i]);
                }
            }

            return isFound;
        }

        private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
        {
            StopAndCleanSoundEmitter(soundEmitter);
        }

        private void StopAndCleanSoundEmitter(SoundEmitter soundEmitter)
        {
            if(!soundEmitter.IsLooping)
                soundEmitter.onSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

            soundEmitter.Stop();
            _soundEmitterPool.Recycle(soundEmitter);

            // remove key
            _soundEmitterVault.Remove(soundEmitter.audioCueKey);
        }

        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.onSoundFinishedPlaying -= StopMusicEmitter;
            _soundEmitterPool.Recycle(soundEmitter);
        }
        #endregion
    }
}
