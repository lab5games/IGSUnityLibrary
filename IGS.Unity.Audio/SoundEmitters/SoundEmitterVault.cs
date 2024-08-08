using System.Collections.Generic;

namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/SoundEmitters/SoundEmitterVault.cs
    /// </summary>
    public class SoundEmitterVault
    {
        int _nextUniqueKey = 0;
        List<AudioCueKey> _emitterKeys = null;
        List<SoundEmitter[]> _emittersList = null;

        public SoundEmitterVault()
        {
            _emitterKeys = new List<AudioCueKey>();
            _emittersList = new List<SoundEmitter[]>();
        }

        public AudioCueKey GetKey(AudioCueSO cue)
        {
            return new AudioCueKey(_nextUniqueKey++, cue);
        }

        public void Add(AudioCueKey key, SoundEmitter[] emitters)
        {
            _emitterKeys.Add(key);
            _emittersList.Add(emitters);    
        }

        public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitters)
        {
            AudioCueKey emitterKey = GetKey(cue);

            _emitterKeys.Add(emitterKey);
            _emittersList.Add(emitters);

            return emitterKey;
        }

        public bool Get(AudioCueKey key, out SoundEmitter[] emitters)
        {
            int indx = _emitterKeys.FindIndex(x => x == key);

            if(indx < 0)
            {
                emitters = null;
                return false;
            }

            emitters = _emittersList[indx];
            
            return true;
        }

        public bool Remove(AudioCueKey key)
        {
            int indx = _emitterKeys.FindIndex(x => x == key);
            return RemoveAt(indx);
        }

        public bool RemoveAt(int index)
        {
            if(index < 0)
            {
                return false;
            }

            _emitterKeys.RemoveAt(index);
            _emittersList.RemoveAt(index);

            return true;
        }
    }
}
