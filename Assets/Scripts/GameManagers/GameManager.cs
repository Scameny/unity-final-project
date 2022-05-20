using UnityEngine;

namespace Character
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gm;

        private void Awake()
        {
            gm = this;
        }

        private void Start()
        {

        }
    }
}


