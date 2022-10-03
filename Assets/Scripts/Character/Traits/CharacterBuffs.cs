using Abilities.Passive;
using CardSystem;
using Character.Stats;
using GameManagement;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Buff
{
    [Serializable]
    public class CharacterBuffs : MonoBehaviour, ICardGiver, IPassiveProvider
    {
        [Serializable]
        public class BuffsDictionary : SerializableDictionaryBase<string, BuffInfo> { }
        
        [TabGroup("Traits")]
        public BuffsDictionary currentBuffs;


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

        public List<SignalData> RemoveBuffs(GameObject user, IEnumerable<GameObject> charactersInCombat)
        {
            List<string> keys = new List<string>(currentBuffs.Keys);
            List<SignalData> toRet = new List<SignalData>();
            foreach (var key in keys)
            {
                BaseBuff buff = currentBuffs[key].buff;
                if (!buff.IsTrait() && RemoveBuff(key))
                {
                    toRet.Add(new TraitCombatSignalData(GameSignal.REMOVE_TRAIT, user, charactersInCombat, buff));
                }
            }
            return toRet;
        }

        public List<SignalData> ReduceTurnInTemporaryBuffs(GameObject user, IEnumerable<GameObject> charactersInCombat)
        {
            List<SignalData> toRet = new List<SignalData>();
            foreach (var key in currentBuffs.Keys.ToList())
            {
                if (currentBuffs[key].buff.IsTemporary())
                {
                    BuffInfo buffInfo = currentBuffs[key];
                    buffInfo.remainingTurns -= 1;
                    toRet.Add(new TraitCombatSignalData(GameSignal.TRAIT_MODIFIED, user, charactersInCombat, buffInfo.buff));
                    if (buffInfo.remainingTurns == 0)
                    {
                        if (RemoveBuff(key))
                            toRet.Add(new TraitCombatSignalData(GameSignal.REMOVE_TRAIT, gameObject, charactersInCombat, buffInfo.buff));
                    }
                }
            }
            return toRet;
        }

        public List<BaseBuff> GetTraits()
        {
            List<BaseBuff> toRet = new List<BaseBuff>();
            foreach (var key in currentBuffs.Keys.ToList())
            {
                toRet.Add(currentBuffs[key].buff);
            }
            return toRet;
        }

        public GameSignal NewBuff(BaseBuff buff)
        {
            if (currentBuffs.ContainsKey(buff.GetName()))
            {
                if (buff.IsTemporary())
                {
                    currentBuffs[buff.GetName()].remainingTurns = buff.GetTurns();
                    return GameSignal.TRAIT_RENEWED;
                }
                else if (buff.GetMaxStacks() > currentBuffs[buff.GetName()].stacks)
                {
                    currentBuffs[buff.GetName()].stacks += 1;
                    return GameSignal.TRAIT_MODIFIED;
                }
                return GameSignal.NONE;
            }
            else
            {
                currentBuffs[buff.GetName()] = new BuffInfo(buff, buff.GetTurns());
                return GameSignal.NEW_TRAIT;
            }
        }

        public bool RemoveBuff(string buff)
        {
            if (!currentBuffs.ContainsKey(buff))
                return false;
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

    }

    [Serializable]
    public class BuffInfo
    {
        public BuffInfo(BaseBuff buff, int remainingTurns, int stacks = 1)
        {
            this.buff = buff;
            this.remainingTurns = remainingTurns;
            this.stacks = stacks;
        }

        public BuffInfo()
        {
        }

        public BaseBuff buff;
        public int remainingTurns;
        public int stacks;

        public void SetTooltipText(SimpleTooltip tooltip)
        {
            tooltip.infoRight = buff.IsTemporary() ? remainingTurns.ToString() + " remaining turns" : "Permanent";
            buff.SetTooltipText(tooltip);
        }
    }
}
