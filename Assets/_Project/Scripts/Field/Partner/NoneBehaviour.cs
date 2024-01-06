namespace DrumRhythmGame.Field
{
    /// <summary>
    /// SEEMINGLY REDUNDANT.
    /// </summary>
    public class NoneBehaviour : IPartnerBehaviour
    {
        public bool Enabled { get; private set; } = false;

        public void Enable()
        {
            // Nothing to do
        }

        public void Disable()
        {
            // Nothing to do
        }
    }
}