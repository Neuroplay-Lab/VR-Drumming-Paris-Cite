using System;

namespace DrumRhythmGame.Data
{
    [Serializable]
    public class PreferenceData
    {
        public bool displayVisualCue = true;
        public bool enableRecording = true;
        public bool recordPerUnit = true;
        public bool muteAgentDrumSounds = true;
        public bool muteParticipcantDrumSounds = false;
        public bool muteMusicSounds = false;
        public bool enableLogging = true;

        public float hideCueAfterTime = 20.0F;
        public float musicVolume = 1;
    }
}