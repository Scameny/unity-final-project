using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class EmptyCardContainerException : Exception
    {

        public EmptyCardContainerException( string className)
        {
            this.className = className;
        }

        public string className;
    }
}
