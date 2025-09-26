namespace CHAL.Data
{
    public enum HeroAIPrio
    {
        RandomAttack,
        AttackHighestHP,
        AttackLowestHP,
        AttackNearest,
        FocusFirstInRange,
        BuffAllies,
        HealAllies,
        DebuffTarget,
        MaintainMinions,
        SpreadDoTs,
        CCFirstThreat,
        // TODO: ggf. erweitern für Spezialverhalten
    }

}