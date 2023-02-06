using System;
using UnityEngine;

namespace Combat.Character
{
    public class NotYourTurnException : Exception
    {
        GameObject character;
        public NotYourTurnException(GameObject character) 
        {
            this.character = character;
        }
    }

}
