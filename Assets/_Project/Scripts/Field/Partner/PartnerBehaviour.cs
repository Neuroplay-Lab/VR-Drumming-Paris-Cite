using DrumRhythmGame.Data;
using UnityEngine.XR;

namespace DrumRhythmGame.Field
{
    public interface IPartnerBehaviour
    {
        bool Enabled { get; }
        
        void Enable();
        void Disable();
    }
}