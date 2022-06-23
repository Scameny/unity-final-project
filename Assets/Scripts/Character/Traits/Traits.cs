using Abilities.Passive;
using CardSystem;
using Character.Stats;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Character.Trait
{
    [Serializable]
    public class Traits : MonoBehaviour, ICardGiver, IPassiveProvider
    {
        [Serializable]
        public class TraitsDictionary : SerializableDictionaryBase<string, TraitInfo> { }
        
        [TabGroup("Traits")]
        public TraitsDictionary currentTraits;

        [TabGroup("UI")]
        public GameObject traitMenu, traitUI;

        public int GetAdditiveModifier(DamageTypeStat stat)
        {
            int value = 0;
            foreach (var trait in currentTraits.Values)
            {
                foreach (var givenStat in trait.trait.GetAdditiveModifier(stat))
                {
                    value += givenStat;    
                }
            }
            return value;
        }

        public int GetAdditiveModifier(StatType stat)
        {
            int value = 0;
            foreach (var trait in currentTraits.Values)
            {
                foreach (var givenStat in trait.trait.GetAdditiveModifier(stat))
                {
                    value += givenStat;
                }
            }
            return value;
        }

        public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var trait in currentTraits.Values)
            {
                foreach (var usable in trait.trait.GetUsableCards())
                {
                    yield return usable;
                }
            }
        }

        public IEnumerable<Passive> GetPasiveAbilities()
        {
            foreach (var item in currentTraits.Values)
            {
                foreach (var passive in item.trait.GetPasiveAbilities())
                {
                    yield return passive;
                }
            }
        }

        public void ReduceTurnInTemporaryTraits()
        {
            foreach (var pair in currentTraits)
            {
                if (pair.Value.trait.IsTemporary)
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
            if (currentTraits.ContainsKey(trait.name))
            {
                if (trait.IsTemporary)
                {
                    currentTraits[trait.name].remainingTurns = trait.turns;
                }
            }
            else
            {
                currentTraits[trait.name] = new TraitInfo(trait, trait.turns, NewTraitUIElement(trait));
            }
        }

        public void RemoveTrait(string trait)
        {
            DeleteTraitUIElement(currentTraits[trait].uiElement);
            foreach(var item in currentTraits[trait].trait.GetPasiveAbilities())
            {
                item.OnCompleted();
            }
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

        public void UpdateTraitElements(string trait, TraitInfo info)
        {
            if (info.trait.IsTemporary)
                info.uiElement.GetComponent<Text>().text = trait + " " + info.remainingTurns + " secs";
            else
                info.uiElement.GetComponent<Text>().text = trait;
        }
        #endregion

    }

    [System.Serializable]
    public class TraitInfo
    {
        public TraitInfo(BaseTrait trait, int remainingTurns, GameObject uiElement)
        {
            this.trait = trait;
            this.remainingTurns = remainingTurns;
            this.uiElement = uiElement;
        }

        public TraitInfo()
        {

        }

        public BaseTrait trait;
        public int remainingTurns;
        public GameObject uiElement;
    }
}
