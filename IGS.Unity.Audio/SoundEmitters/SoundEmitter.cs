using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using IGS.Unity.Tasks;

namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/SoundEmitters/SoundEmitter.cs
    /// </summary>
    public class SoundEmitter
    {
        AudioSource _audioSource;

        internal AudioCueKey audioCueKey;

        public event UnityAction<SoundEmitter> onSoundFinishedPlaying;

        public AudioClip Clip { get { return _audioSource.clip; } }

        public bool IsPlaying { get { return _audioSource.isPlaying; } }

        public bool IsLooping { get { return _audioSource.loop; } }

        public SoundEmitter(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioSource.playOnAwake = false;
        }

        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool loop)
        {
            PlayAudioClip(clip, settings, loop, Vector3.zero);
        }

        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool loop, Vector3 position)
        {
            _audioSource.clip = clip;
            _audioSource.transform.position = position;
            _audioSource.loop = loop;
            _audioSource.time = 0;

            settings.ApplyTo(_audioSource);

            _audioSource.Play();

            if(!loop)
            {
                UTask.Enumerator(FinishedPlaying(clip.length));
            }
        }

        public void FadeMusicIn(AudioClip clip, AudioConfigurationSO settings, float duration, float startTime = 0)
        {
            PlayAudioClip(clip, settings, true);
            _audioSource.volume = 0;

            if(startTime <= _audioSource.clip.length)
                _audioSource.time = startTime;

            UTask.Enumerator(Task_FadeVolume(settings.Volume, duration));
        }

        public void FadeMusicOut(float duration)
        {
            UTask.Enumerator(Task_FadeVolume(0, duration))
                .RegisterCallbacks(() =>
                {
                    onSoundFinishedPlaying.Invoke(this);
                });
        }

        public void UnPause()
        {
            _audioSource.UnPause();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Finish()
        {
            if(_audioSource.loop)
            {
                _audioSource.loop = false;
                float timeRemaining = _audioSource.clip.length - _audioSource.time;

                UTask.Enumerator(FinishedPlaying(timeRemaining));
            }
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void Release()
        {
            UnityEngine.Object.Destroy(_audioSource.gameObject);
        }

        IEnumerator FinishedPlaying(float clipLength)
        {
            yield return Yields.WaitSeconds(clipLength);

            if(onSoundFinishedPlaying != null)
                onSoundFinishedPlaying.Invoke(this);    
        }

        IEnumerator Task_FadeVolume(float endVolume, float duration)
        {
            float startVolume = _audioSource.volume;
            float diff = endVolume - startVolume;

            while(_audioSource.volume != endVolume)
            {
                _audioSource.volume = Mathf.Clamp(_audioSource.volume + (diff / duration) * Time.deltaTime, startVolume, endVolume);

                yield return Yields.NextFrame;
            }
        }
    }
}
