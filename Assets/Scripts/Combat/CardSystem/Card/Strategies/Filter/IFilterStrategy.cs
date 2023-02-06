using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.FilterStrategies
{
    public interface IFilterStrategy
    {
        public IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter);
    }
}
