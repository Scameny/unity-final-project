using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Validator
    {
        public bool MustNotBeNull(Object parameter)
        {
            return parameter != null;
        }
    }

}