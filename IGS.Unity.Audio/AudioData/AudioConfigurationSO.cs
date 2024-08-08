using UnityEngine;
using UnityEngine.Audio;

namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/AudioData/AudioConfigurationSO.cs
    /// </summary>
    [CreateAssetMenu(menuName ="IGS Unity/Audio/Audio Configuration", fileName ="New Audio Configuration")]
    public class AudioConfigurationSO : ScriptableObject
    {
        private enum PriorityLevel
        {
            Highest = 0,
            High = 64,
            Standard = 128,
            Low = 194,
            VeryLow = 256,
        }

        [SerializeField] AudioMixerGroup outputAudioMixerGroup = null;
        public AudioMixerGroup OutputAudioMixerGroup 
        { 
            get { return outputAudioMixerGroup; }
            set { outputAudioMixerGroup = value; }  
        }

        [SerializeField] PriorityLevel priorityLevel = PriorityLevel.Standard;
        public int Priority
        {
            get { return (int)priorityLevel; }
            set { priorityLevel = (PriorityLevel)value; }
        }

        [Header("Sound Properties")]
        [SerializeField] bool mute = false;
        [SerializeField, Range(0f, 1f)] float volume = 1f;
        [SerializeField, Range(-3f, 3f)] float pitch = 1f;
        [SerializeField, Range(-1f, 1f)] float panStereo = 0f;
        [SerializeField, Range(0f, 1.1f)] float reverbZoneMix = 1f;

        public bool Mute
        {
            get { return mute; }
            set { mute = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = Mathf.Clamp(value, 0f, 1f); }
        }

        public float Pitch
        {
            get { return pitch; }
            set { pitch = Mathf.Clamp(value, -3f, 3f); }
        }

        public float PanStereo
        {
            get { return panStereo; }
            set { panStereo = Mathf.Clamp(value, -1f, 1f); }
        }

        public float ReverbZoneMix
        {
            get { return  reverbZoneMix; }
            set { reverbZoneMix = Mathf.Clamp(value, 0f, 1.1f); }
        }

        [Header("Spatialisation")]
        [SerializeField] AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [SerializeField, Range(0f, 1f)] float spatialBlend = 0f;
        [SerializeField, Range(0.01f, 5f)] float minDistance = 0.1f;
        [SerializeField, Range(5f, 100)] float maxDistance = 50f;
        [SerializeField, Range(0, 360)] int spread = 0;
        [SerializeField, Range(0f, 5f)] float dopplerLevel = 1f;

        public AudioRolloffMode RolloffMode
        {
            get { return rolloffMode; }
            set { rolloffMode = value; }
        }

        public float SpatialBlend
        {
            get { return spatialBlend; }
            set { spatialBlend = Mathf.Clamp(value, 0f, 1f); }
        }

        public float MinDistance
        {
            get { return minDistance; }
            set { minDistance = Mathf.Clamp(value, 0.01f, 5f); }
        }

        public float MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = Mathf.Clamp(value, 5f, 100f); }
        }

        public int Spread
        {
            get { return spread; }
            set { spread = Mathf.Clamp(value, 0, 360); }
        }

        public float DopplerLevel
        {
            get { return dopplerLevel; }
            set { dopplerLevel = Mathf.Clamp(value, 0f, 5f); }
        }

        [Header("Ignores")]
        [SerializeField] bool bypassEffects = false;
        [SerializeField] bool bypassListenerEffects = false;
        [SerializeField] bool bypassReverbZones = false;
        [SerializeField] bool ignoreListenerVolume = false;
        [SerializeField] bool ignoreListenerPause = false;

        public bool BypassEffects
        {
            get { return bypassEffects; }
            set { bypassEffects = value; }
        }

        public bool BypassListenerEffects
        {
            get { return bypassListenerEffects; }
            set { bypassListenerEffects = value; }
        }

        public bool BypassReverbZones
        {
            get { return bypassReverbZones; }
            set { bypassReverbZones = value; }
        }

        public bool IgnoreListenerVolume
        {
            get { return ignoreListenerVolume; }
            set { ignoreListenerVolume = value; }
        }

        public bool IgnoreListenerPause
        {
            get { return ignoreListenerPause; }
            set { ignoreListenerPause = value; }
        }

        public void ApplyTo(AudioSource audioSource)
        {
            audioSource.outputAudioMixerGroup = OutputAudioMixerGroup;
            audioSource.priority = Priority;

            audioSource.mute = Mute;
            audioSource.volume = Volume;
            audioSource.pitch = Pitch;
            audioSource.panStereo = PanStereo;
            audioSource.reverbZoneMix = ReverbZoneMix;

            audioSource.rolloffMode = RolloffMode;
            audioSource.spatialBlend = SpatialBlend;
            audioSource.minDistance = MinDistance;
            audioSource.maxDistance = MaxDistance;
            audioSource.spread = Spread;
            audioSource.dopplerLevel = DopplerLevel;

            audioSource.bypassEffects = BypassEffects;
            audioSource.bypassListenerEffects = BypassListenerEffects;
            audioSource.bypassReverbZones = BypassReverbZones;
            audioSource.ignoreListenerVolume = IgnoreListenerVolume;
            audioSource.ignoreListenerPause = IgnoreListenerPause;
        }
    }
}
