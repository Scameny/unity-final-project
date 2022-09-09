using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class GameStartMenu : MonoBehaviour
    {
        [SerializeField] string gameStartScene;

        public void StartGame()
        {
            SceneManager.LoadScene(gameStartScene);
        }
    }

}
