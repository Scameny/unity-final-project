using Sirenix.OdinInspector;

namespace CardSystem
{
    public class PlayerDeck : Deck
    {

        [Button]
        public void ShufflePlayerDeck()
        {
            base.ShuffleDeck();
        }

    }

}
