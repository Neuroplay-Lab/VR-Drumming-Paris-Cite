namespace DrumRhythmGame.Data
{
    /// <summary>
    ///     Contains all relevant info on each drum hit
    /// </summary>
    public struct DrumHitInfo
    {
        public readonly ActorType actor;
        public readonly InstrumentType instrumentType;
        public readonly float time;
        public static DrumHitInfo empty= new DrumHitInfo(ActorType.None, InstrumentType.None, 0);

        public DrumHitInfo(ActorType actor, InstrumentType instrumentType, float time)
        {
            this.actor = actor;
            this.instrumentType = instrumentType;
            this.time = time;
        }
    }
}