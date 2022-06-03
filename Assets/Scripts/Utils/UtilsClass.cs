using FloorManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UtilsClass
    {
        public readonly static UtilsClass instance = new UtilsClass();


        public void GetDirection(Direction dir, ref int x, ref int y)
        {
            if (dir.Equals(Direction.Up))
            {
                x = 0;
                y = 1;
            }
            if (dir.Equals(Direction.Right))
            {
                x = 1;
                y = 0;
            }
            if (dir.Equals(Direction.Left))
            {
                x = -1;
                y = 0;
            }
            if (dir.Equals(Direction.Down))
            {
                x = 0;
                y = -1;
            }

        }
    }

}