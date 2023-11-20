namespace Cooking.Recipe.Stat
{
    using Character.StatEffect;
    using MyBox;
    [System.Serializable]
    public class RecipeStat
    {
        [Tag]
        public string AffectedCharacter;
        public Effect Stat;
    }
}