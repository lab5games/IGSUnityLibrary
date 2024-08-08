using UnityEngine;

namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/AudioData/AudioCueSO.cs
    /// </summary>
    [CreateAssetMenu(menuName ="IGS Unity/Audio/Audio Cue", fileName ="New Audio Cue")]
    public class AudioCueSO : ScriptableObject
    {
        [SerializeField] bool looping = false;
        [SerializeField] AudioClipsGroup[] audioClipGroups = null;

        public bool Looping
        {
            get { return looping; }
            set { looping = value; }
        }

        public AudioClip[] GetClips()
        {
            int numClips = audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numClips];

            for(int i=0; i<numClips; i++)
            {
                resultingClips[i] = audioClipGroups[i].GetNextClip();
            }

            return resultingClips;
        }
    }


    [System.Serializable]
    public class AudioClipsGroup
    {
        public enum SequenceMode
        {
            Random,
            RandomImmediateRepeat,
            Sequential
        }

        [SerializeField] SequenceMode sequenceMode = SequenceMode.RandomImmediateRepeat;
        [SerializeField] AudioClip[] audioClips;

        int _nextClipToPlay = -1;
        int _lastClipPlayed = -1;

        public AudioClip GetNextClip()
        {
            if(audioClips.Length == 1)
                return audioClips[0];

            if(_nextClipToPlay == -1)
            {
                _nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
            }
            else
            {
                switch(sequenceMode)
                {
                    case SequenceMode.Random:
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        break;

                    case SequenceMode.RandomImmediateRepeat:
                        do
                        {
                            _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } 
                        while(_nextClipToPlay == _lastClipPlayed);
                        break;

                    case SequenceMode.Sequential:
                        _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                        break;
                }
            }

            _lastClipPlayed = _nextClipToPlay;

            return audioClips[_nextClipToPlay];
        }
    }
}
