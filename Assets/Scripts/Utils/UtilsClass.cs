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
    }

}