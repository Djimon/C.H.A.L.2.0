using CHAL.Systems.Unit;


namespace CHAL.AI
{
    public class AITargetSelector
    {

        public EffectReceiver currentTarget;
        public AITargetPrio prioMode;
        public float sightRange;

        public void EnsureTarget()
        {
            //Lockin on target until dead or out of range/sight
        }

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
