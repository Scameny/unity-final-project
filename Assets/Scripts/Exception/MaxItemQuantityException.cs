using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxItemQuantityException : Exception
{
    public MaxItemQuantityException(string itemName)
    {
        this.itemName = itemName;
    }

    public string itemName;
}