using System.Collections.Generic;

namespace Character.StatEffect
{
    public interface IApplyEffect
    {
        public List<Effect> Effects { get; set; }
        public void ApplyEffect(Character character);
    }
}
