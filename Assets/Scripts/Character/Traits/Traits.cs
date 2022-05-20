using Character.Stats;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Character.Trait
{
    public class Traits : MonoBehaviour
    {
        [System.Serializable]
        public class TraitsDictionary : SerializableDictionaryBase<BaseTrait, TraitInfo> { }

        public TraitsDictionary currentTraits;

        public GameObject traitMenu, traitUI;

        public float GetAdditiveModifier(DamageTypeStat stat)
        {
            float value = 0;
            foreach (var trait in currentTraits.Keys)
            {
                foreach (var givenStat in trait.GetAdditiveModifier(stat))
                {
                    value += givenStat;    
                }
            }
            return value;
        }

        public float GetAdditiveModifier(StatType stat)
        {
            float value = 0;
            foreach (var trait in currentTraits.Keys)
            {
                foreach (var givenStat in trait.GetAdditiveModifier(stat))
                {
                    value += givenStat;
                }
            }
            return value;
        }


        public void ReduceTurnInTemporaryTraits()
        {
            foreach (var pair in currentTraits)
            {
                if (pair.Key.IsTemporary)
                {
                    currentTraits[pair.Key].remainingTurns -= 1;
                    if (currentTraits[pair.Key].remainingTurns == 0)
                        RemoveTrait(pair.Key);
                    else
                        UpdateTraitElements(pair.Key, pair.Value);
                }
            }
        }

        public void NewTrait(BaseTrait trait)
        {
            if (currentTraits.ContainsKey(trait))
            {
                if (trait.IsTemporary)
                {
                    currentTraits[trait].remainingTurns = trait.turns;
                }
            }
            else
            {
                currentTraits[trait] = new TraitInfo(trait.turns, NewTraitUIElement(trait));
            }
        }

        public void RemoveTrait(BaseTrait trait)
        {
            DeleteTraitUIElement(currentTraits[trait].uiElement);
            currentTraits.Remove(trait);
            GameDebug.Instance.Log(Color.blue, "Modifier expired");

        }

        #region UI Management
        public GameObject NewTraitUIElement(BaseTrait trait)
        {
            GameObject element = Instantiate(traitUI, traitMenu.transform);
            element.GetComponent<Text>().text = trait.name;
            return element;
        }

        public void DeleteTraitUIElement(GameObject uiElement)
        {
            Destroy(uiElement);
        }

        public void UpdateTraitElements(BaseTrait trait, TraitInfo info)
        {
            if (trait.IsTemporary)
                info.uiElement.GetComponent<Text>().text = trait.name + " " + info.remainingTurns + " secs";
            else
                info.uiElement.GetComponent<Text>().text = trait.name;
        }



        #endregion

    }

    [System.Serializable]
    public class TraitInfo
    {
        public TraitInfo(int remainingTurns, GameObject uiElement)
        {
            this.remainingTurns = remainingTurns;
            this.uiElement = uiElement;
        }

        public int remainingTurns;
        public GameObject uiElement;
    }
}
