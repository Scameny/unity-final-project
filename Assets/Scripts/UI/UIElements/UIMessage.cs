using GameManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.UIElements
{
    public class UIMessage : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] float displayTime;
        [SerializeField] List<GameError> errorList;

        IDisposable disposable;
        Coroutine coroutine;
        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);       
        }

        public void DisplayError(GameSignal gameSignal, List<string> parameters) 
        {
            GameError err = errorList.Find(r => r.errorSignal.Equals(gameSignal));
            if (err != null)
                SetErrorText(err, parameters);

        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on UIMessage " + error.Message);
        }

        public void OnNext(SignalData value)
        {
            try
            {
                if (value.GetType().Equals(typeof(ErrorSignalData)))
                    DisplayError(value.signal, (value as ErrorSignalData).parameters);
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        private void SetErrorText(GameError e, List<string> parameters)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(SetErrorTextCoroutine(e,parameters));
        }

        private IEnumerator SetErrorTextCoroutine(GameError e, List<string> parameters)
        {
            TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
            textMesh.enabled = true;
            textMesh.text = UtilsClass.instance.ParseParams(e.message, parameters);
            textMesh.color = e.textColor;
            textMesh.outlineColor = e.outlineColor;
            textMesh.outlineWidth = e.outlineThickness;
            yield return new WaitForSeconds(displayTime);
            textMesh.enabled = false;
            coroutine = null;
        }
    }

    [Serializable]
    public class GameError
    {
        public string message;
        public GameSignal errorSignal;
        public Color textColor;
        [ColorUsage(true, true)]
        public Color32 outlineColor;
        public float outlineThickness;
    }
}
