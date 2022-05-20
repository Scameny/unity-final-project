using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.FilterStrategies
{
    [CreateAssetMenu(fileName = "Filter", menuName = "Strategy/TargetingFilter/TagFilter", order = 1)]
    public class TagFilter : FilterStrategy
    {
        public string tagToFilter;

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(tagToFilter))
                {
                    yield return gameObject;
                }
            }
        }
    }
}