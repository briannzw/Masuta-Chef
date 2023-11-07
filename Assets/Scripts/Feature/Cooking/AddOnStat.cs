namespace Character.StatEffect
{
    using MyBox;
    [System.Serializable]
    public class AddOnStat
    {
        [Tag]
        public string AffectedCharacter;
        public Effect Stat;
    }
}