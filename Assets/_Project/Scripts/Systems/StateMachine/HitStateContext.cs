using RootMotion.FinalIK;

public class HitStateContext
{
    private IKEffector _handEffector;

    public HitStateContext(IKEffector iKEffector)
    {
        _handEffector = iKEffector;
    }

    public IKEffector handEffector => _handEffector;
}