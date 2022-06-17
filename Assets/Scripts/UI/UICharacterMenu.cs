using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UICharacterMenu : MonoBehaviour
    {
        [SerializeField] GameObject gearWindow, deckWindow;
        public void SetActiveGearWindow(bool enable)
        {
            gearWindow.SetActive(enable);
        }

        public void SetActiveDeckWindow(bool enable)
        {
            deckWindow.SetActive(enable);
        }
    }
}
