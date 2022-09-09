using Character.Buff;
using Character.Character;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class UIBuff : MonoBehaviour, IObserver<SignalData>
    {

        IDisposable disposable;
        BuffInfo buffInfo;
        GameObject user;

        public void InitializeBuff(BaseBuff baseBuff, GameObject user)
        {
            buffInfo = user.GetComponent<DefaultCharacter>().GetTrait(baseBuff.GetName());
            this.user = user;
            transform.Find("Icon").GetComponent<Image>().sprite = buffInfo.buff.GetIcon();
            GetComponentInChildren<TextMeshProUGUI>().text = buffInfo.stacks.ToString();
            SimpleTooltip tooltip = GetComponent<SimpleTooltip>();
            tooltip.simpleTooltipStyle = UIManager.manager.GetTooltipStyle();
            buffInfo.SetTooltipText(tooltip);
            disposable = UIManager.manager.Subscribe(this);
        }


        private void ReloadBuff()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = buffInfo.stacks.ToString();
            SimpleTooltip tooltip = GetComponent<SimpleTooltip>();
            buffInfo.SetTooltipText(tooltip);
        }

        public void OnCompleted()
        {
            disposable.Dispose();
            Destroy(gameObject);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if ((value.signal.Equals(GameSignal.TRAIT_RENEWED) || value.signal.Equals(GameSignal.TRAIT_MODIFIED)) && (value as TraitCombatSignalData).trait.GetName().Equals(buffInfo.buff.GetName()) && (value as TraitCombatSignalData).user.Equals(user))
            {
                ReloadBuff();
            } 
            else if ((value.signal.Equals(GameSignal.TRAIT_EXPIRED) || value.signal.Equals(GameSignal.REMOVE_TRAIT)) && (value as TraitCombatSignalData).trait.GetName().Equals(buffInfo.buff.GetName()) && (value as TraitCombatSignalData).user.Equals(user))
            {
                OnCompleted();
            }
        }
    }

}
