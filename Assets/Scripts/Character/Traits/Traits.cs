using Abilities.Passive;
using CardSystem;
using Character.Stats;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UI;
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

        SimpleTooltipStyle tooltipStyle;


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

        public void RemoveTemporaryTraits()
        {
            List<string> keys = new List<string>(currentTraits.Keys);
            foreach (var key in keys)
            {
                if (currentTraits[key].trait.IsTemporary())
                {
                    RemoveTrait(key);
                }
            }
        }

        public void ReduceTurnInTemporaryTraits()
        {
            List<string> keys = new List<string>(currentTraits.Keys);
            foreach (var key in keys)
            {
                if (currentTraits[key].trait.IsTemporary())
                {
                    currentTraits[key].remainingTurns -= 1;
                    if (currentTraits[key].remainingTurns == 0)
                        RemoveTrait(key);
                    else
                        UpdateTraitElements(key, currentTraits[key]);
                }
            }
        }

        public bool NewTrait(BaseTrait trait)
        {
            if (currentTraits.ContainsKey(trait.GetName()))
            {
                if (trait.IsTemporary())
                {
                    currentTraits[trait.GetName()].remainingTurns = trait.GetTurns();
                }
                return false;
            }
            else
            {
                currentTraits[trait.GetName()] = new TraitInfo(trait, trait.GetTurns(), NewTraitUIElement(trait));
                return true;
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
            if (tooltipStyle == null)
                tooltipStyle = UIManager.manager.tooltipStyle;
            element.transform.Find("Icon").GetComponent<Image>().sprite = trait.icon;
            SimpleTooltip tooltip = element.GetComponent<SimpleTooltip>(); 
            tooltip.simpleTooltipStyle = tooltipStyle;
            tooltip.infoLeft = trait.GetName() + "\n";
            foreach (var passive in trait.GetPasiveAbilities()) {
                tooltip.infoLeft += "\n@passiveeffect@Passiveeffect@break@: " + passive.passiveAbility.GetDescription();
            }
            foreach (var item in trait.GetCards())
            {
                tooltip.infoLeft += "\n@cards@Card@break@: (" + item.quantity + ") " + item.usable.GetName();
            }
            tooltip.infoLeft += "@statistic@";
            foreach (var item in trait.GetStatistic())
            {
                tooltip.infoLeft += (item.amount>0 ?"+" :"") + item.amount + " " + item.statType.ToString() + "\n";
            }
            foreach (var item in trait.GetSecondaryStatistic())
            {
                tooltip.infoLeft += "+ " + item.amount + " " + item.statType.ToString() + "\n";
            }
            if (trait.IsTemporary())
                tooltip.infoRight = trait.GetTurns() + " remaining turns";
            else
                tooltip.infoRight = "Permanent";
            return element;
        }

        public void DeleteTraitUIElement(GameObject uiElement)
        {
            Destroy(uiElement);
        }

        public void UpdateTraitElements(string trait, TraitInfo info)
        {
            if (info.trait.IsTemporary())
                info.uiElement.GetComponent<SimpleTooltip>().infoRight= trait + " " + info.remainingTurns + " turns";
    
        }
        #endregion

    }

    [Serializable]
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
