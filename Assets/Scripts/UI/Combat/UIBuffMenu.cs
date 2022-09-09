using Character.Character;
using GameManagement;
using System;
using UI.Character;
using UnityEngine;

namespace UI.Combat
{
    public class UIBuffMenu : MonoBehaviour, IObserver<SignalData>
    {
        GameObject buffElement;
        IDisposable disposable;
        DefaultCharacter character;

        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            if (GetComponentInParent<CombatCharacterUI>() != null)
            {
                character = GetComponentInParent<CombatCharacterUI>().GetCharacter();
                buffElement = Resources.Load<GameObject>("UI/EnemyBuffElement");
            }
            else
            {
                character = GameObject.FindGameObjectWithTag("Player").GetComponent<DefaultCharacter>();
                buffElement = Resources.Load<GameObject>("UI/BuffElement");
            }
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.NEW_TRAIT) && (value as TraitCombatSignalData).user.Equals(character.gameObject))
            {
                TraitCombatSignalData data = value as TraitCombatSignalData;
                GameObject newBuff = Instantiate(buffElement, transform);
                newBuff.GetComponent<UIBuff>().InitializeBuff(data.trait, data.user);
            }
        }
    }

}
