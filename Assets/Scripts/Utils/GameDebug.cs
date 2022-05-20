using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils 
{ 
    public class GameDebug
    {
        private readonly static GameDebug _instance = new GameDebug();

        private GameDebug()
        {

        }

        public static GameDebug Instance
        {
            get
            {
                return _instance;
            }
        }

        public void Log(Color color, string msg)
        {
            Debug.Log("<color=" + ColorUtility.ToHtmlStringRGB(color) + ">" + msg + "</color>");
        }

        public void LogError(string msg)
        {
            Debug.LogError(msg);
        }

    }
}
