using Abilities.Passive;
using System;
using System.Collections.Generic;

namespace Character.Buff
{
    public interface IPassiveProvider
    {
        IEnumerable<Passive> GetPasiveAbilities();
    }
}