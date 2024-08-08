
namespace IGS.Unity.Audio
{
    /// <summary>
    /// https://github.com/UnityTechnologies/open-project-1/blob/main/UOP1_Project/Assets/Scripts/Audio/AudioData/AudioCueKey.cs
    /// </summary>
    public struct AudioCueKey
    {
        public static readonly AudioCueKey Invalide = new AudioCueKey(-1, null);

        internal int value;
        internal AudioCueSO audioCue;

        internal AudioCueKey(int value, AudioCueSO audioCue)
        {
            this.value = value;
            this.audioCue = audioCue;
        }

        public override bool Equals(object obj)
        {
            if(obj is AudioCueKey)
            {
                AudioCueKey other = (AudioCueKey)obj;

                return value == other.value && audioCue == other.audioCue;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode() ^ audioCue.GetHashCode();
        }

        public static bool operator ==(AudioCueKey left, AudioCueKey right)
        {
            return left.value == right.value && left.audioCue == right.audioCue;
        }

        public static bool operator !=(AudioCueKey left, AudioCueKey right)
        {
            return left.value != right.value || left.audioCue != right.audioCue;
        }
    }
}
