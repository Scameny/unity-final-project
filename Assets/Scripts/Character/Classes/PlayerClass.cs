using UnityEngine;

namespace Character.Classes

{
    [CreateAssetMenu(fileName = "PlayerClass", menuName = "Character/Class/Player class")]
    public class PlayerClass : CharacterClass
    {
        public const bool npc = false;
        public int GetExpForNextLevel(int level)
        {
            return progression.GetExpNeeded(level);
        }

        public int GetMaxLevel()
        {
            return progression.GetMaxLevel();
        }
        // talents
    }
}

