using CHAL.Systems.Unit;


namespace CHAL.AI
{
/// <summary>
/// Selects and manages AI targets for gameplay interactions.
/// </summary>
    public class AITargetSelector
    {

        public EffectReceiver currentTarget;
        public AITargetPrio prioMode;
        public float sightRange;

/// <summary>
/// Ensures the target is still valid and within range.
/// </summary>
        public void EnsureTarget()
        {
            //Lockin on target until dead or out of range/sight
        }

/// <summary>
/// Resets the current target if it is lost or dead.
/// </summary>
        public void InvalidateTarget()
        {
            //reset currenttarget if Lost/dead
        }

    }

    public enum AITargetPrio
    {
        Nearest,
        HighestHP,
        LowestHP
    }
}
