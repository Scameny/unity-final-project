using CardSystem;

namespace Strategies.EffectStrategies
{
    public abstract class BuffEffectStrategy: EffectStrategy
    {
        public BuffEffectStrategy()
        {
            effectTypes.Add(CardEffectType.Buff);
        }
    }

}

