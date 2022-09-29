using MapManagement;
using System;
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

        public string ConvertTextWithStyles(string text, SimpleTooltipStyle style)
        {
            // Convert all tags to TMPro markup
            var styles = style.fontStyles;
            for (int i = 0; i < styles.Length; i++)
            {
                string addTags = "</b></i></u></s>";
                addTags += "<color=#" + ColorToHex(styles[i].color) + ">";
                if (styles[i].bold) addTags += "<b>";
                if (styles[i].italic) addTags += "<i>";
                if (styles[i].underline) addTags += "<u>";
                if (styles[i].strikethrough) addTags += "<s>";
                text = text.Replace(styles[i].tag, addTags);
            }
            return text;
        }

        public string ColorToHex(Color color)
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);
            return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        /// <summary>
        /// Select n elements for the pool taking account their weigth
        /// </summary>
        /// <typeparam name="T">Type of items stored in the pool</typeparam>
        /// <param name="pool">Pool of items</param>
        /// <param name="numOfElements">Number of elements in the list to return</param>
        /// <param name="removeOnceSelected">true if once one item is took from the pool should be removed</param>
        /// <returns></returns>
        public List<T> GetListFromPool<T>(List<PoolObject<T>> pool, int n, bool removeOnceSelected = true)
        {
            int totalWeigth = 0;
            List<PoolObject<T>> copyOfPool = new List<PoolObject<T>>(pool); 
            List<T> listToRet = new List<T>(); 
            foreach (var item in pool)
            {
                totalWeigth += item.weigth;
            }
            for (int i = 0; i < pool.Count; i++)
            {
                int randomNum = UnityEngine.Random.Range(0, totalWeigth);
                int aux = 0;
                int indexItemSelected = 0;

                for (int j = 0; j < n; j++)
                {
                    if (randomNum < copyOfPool[j].weigth + aux)
                    {
                        indexItemSelected = j;
                        break;
                    }
                    else
                    {
                        aux += copyOfPool[j].weigth;
                    }
                }
                listToRet.Add(copyOfPool[indexItemSelected].gameObject);
                if (removeOnceSelected)
                {
             
                    copyOfPool.RemoveAt(indexItemSelected);
                }
            }
            return listToRet;
        }

        public string ParseParams(string text, List<string> parameters)
        {
            int count = 0;
            foreach (var item in parameters)
            {
                text = text.Replace("{" + count + "}", item);
                count++;
            }
            return text;
        }

    }

    
    public static class ShuffleClass
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    [Serializable]
    public class PoolObject<T>
    {
        public PoolObject(int weigth, T gameObject)
        {
            this.gameObject = gameObject;
            this.weigth = weigth;
        }

        public int weigth;
        public T gameObject;
    }
}