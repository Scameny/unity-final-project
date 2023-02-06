using CardSystem;

namespace Strategies.EffectStrategies
{
    public abstract class DebuffEffectStrategy : EffectStrategy
    {
        public DebuffEffectStrategy()
        {
            effectTypes.Add(CardEffectType.Debuff);
        }
    }

}
