using Abilities.Passive;
using System;
using System.Collections.Generic;

namespace Character.Trait
{
    public interface IPassiveProvider
    {
        IEnumerable<Passive> GetPasiveAbilities();
    }
}