using Combat;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl 
{
    public class PlayerControllerPC : MonoBehaviour
    {

        public GameObject SelectCharacter()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D col = Physics2D.OverlapPoint(pos);
                if (col != null && col.GetComponentInParent<TurnCombat>() != null)
                {
                    return col.GetComponentInParent<TurnCombat>().gameObject;
                }
            }
            return null;
        }

        public bool CancelAction()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

}