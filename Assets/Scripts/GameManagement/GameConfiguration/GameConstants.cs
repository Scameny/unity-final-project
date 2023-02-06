using UnityEngine;


namespace GameManagemenet.GameConfiguration
{
    [CreateAssetMenu(fileName = "GameConstants", menuName = "GameData/GameConstants")]
    public class GameConstants : ScriptableObject
    {
        [SerializeField] int maxNumberOfBuffs;

       
        public int GetMaxNumberOfBuffs()
        {
            return maxNumberOfBuffs;
        }
    }

}
