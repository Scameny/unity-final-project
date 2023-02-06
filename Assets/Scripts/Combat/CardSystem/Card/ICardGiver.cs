using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public interface ICardGiver
    {
        IEnumerable<Usable> GetUsableCards();

    }
}
