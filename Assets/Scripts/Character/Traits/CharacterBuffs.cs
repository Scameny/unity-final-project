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

namespace Character.Buff
{
    [Serializable]
    public class CharacterBuffs : MonoBehaviour, ICardGiver, IPassiveProvider
    {
        [Serializable]
        public class BuffsDictionary : SerializableDictionaryBase<string, BuffInfo> { }
        
        [TabGroup("Traits")]
        public BuffsDictionary currentBuffs;

        [TabGroup("UI")]
        public GameObject traitMenu, traitUI;

        SimpleTooltipStyle tooltipStyle;


        public int GetAdditiveModifier(DamageTypeStat stat)
        {
            int value = 0;
            foreach (var trait in currentBuffs.Values)
            {
                foreach (var givenStat in trait.buff.GetAdditiveModifier(stat))
                {
                    for (int i = 0; i < trait.stacks; i++)
                    {
                        value += givenStat;
                    }
                }
            }
            return value;
        }

        public int GetAdditiveModifier(StatType stat)
        {
            int value = 0;
            foreach (var buff in currentBuffs.Values)
            {
                foreach (var givenStat in buff.buff.GetAdditiveModifier(stat))
                {
                    for (int i = 0; i < buff.stacks; i++)
                    {
                        value += givenStat;
                    }
                }
            }
            return value;
        }

        public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var buff in currentBuffs.Values)
            {
                foreach (var usable in buff.buff.GetUsableCards())
                {
                    yield return usable;
                }
            }
        }

        public IEnumerable<Passive> GetPasiveAbilities()
        {
            foreach (var item in currentBuffs.Values)
            {
                foreach (var passive in item.buff.GetPasiveAbilities())
                {
                    yield return passive;
                }
            }
        }

        public void RemoveBuffs()
        {
            List<string> keys = new List<string>(currentBuffs.Keys);
            foreach (var key in keys)
            {
                RemoveBuff(key);
            }
        }

        public void ReduceTurnInTemporaryBuffs()
        {
            List<string> keys = new List<string>(currentBuffs.Keys);
            foreach (var key in keys)
            {
                if (currentBuffs[key].buff.IsTemporary())
                {
                    currentBuffs[key].remainingTurns -= 1;
                    if (currentBuffs[key].remainingTurns == 0)
                        RemoveBuff(key);
                    else
                        UpdateTraitElements(key, currentBuffs[key]);
                }
            }
        }

        public bool NewBuff(BaseBuff buff)
        {
            if (currentBuffs.ContainsKey(buff.GetName()))
            {
                if (buff.IsTemporary())
                {
                    currentBuffs[buff.GetName()].remainingTurns = buff.GetTurns();
                } 
                else if (buff.GetMaxStacks() > currentBuffs[buff.GetName()].stacks)
                {
                    currentBuffs[buff.GetName()].stacks += 1;
                }
                return false;
            }
            else
            {
                currentBuffs[buff.GetName()] = new BuffInfo(buff, buff.GetTurns(), NewTraitUIElement(buff));
                return true;
            }
        }

        public bool RemoveBuff(string buff)
        {
            if (!currentBuffs.ContainsKey(buff))
                return false;
            DeleteTraitUIElement(currentBuffs[buff].uiElement);
            foreach(var item in currentBuffs[buff].buff.GetPasiveAbilities())
            {
                item.OnCompleted();
            }
            currentBuffs.Remove(buff);
            return true;
        }

        public BuffInfo GetBuff(string buffName)
        {
            if (!currentBuffs.ContainsKey(buffName))
                throw new NotSpecificTraitException();
            return currentBuffs[buffName];
        }

        #region UI Management
        public GameObject NewTraitUIElement(BaseBuff trait)
        {
            GameObject element = Instantiate(traitUI, traitMenu.transform);
            if (tooltipStyle == null)
                tooltipStyle = UIManager.manager.tooltipStyle;
            element.transform.Find("Icon").GetComponent<Image>().sprite = trait.GetIcon();
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

        public void UpdateTraitElements(string trait, BuffInfo info)
        {
            if (info.buff.IsTemporary())
                info.uiElement.GetComponent<SimpleTooltip>().infoRight= trait + " " + info.remainingTurns + " turns";
    
        }
        #endregion

    }

    [Serializable]
    public class BuffInfo
    {
        public BuffInfo(BaseBuff buff, int remainingTurns, GameObject uiElement, int stacks = 1)
        {
            this.buff = buff;
            this.remainingTurns = remainingTurns;
            this.uiElement = uiElement;
            this.stacks = stacks;
        }

        public BuffInfo()
        {
        }

        public BaseBuff buff;
        public int remainingTurns;
        public int stacks;
        public GameObject uiElement;
    }
}
