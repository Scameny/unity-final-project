using GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIElements
{
    public class UIResourceFrame : UIFrame
    {
        public override void OnNext(SignalData value)
        {
            base.OnNext(value);
            if (GetActiveSignals().Exists(s => s.Equals(value.signal)))
            {
                FitResourceBars();
            }
        }

        private void FitResourceBars()
        {
            VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();
            float width = GetComponent<RectTransform>().rect.width - (layout.padding.left + layout.padding.right);
            float height = GetComponent<RectTransform>().rect.height - (layout.padding.bottom + layout.padding.top);
            foreach(var child in transform)
            {
                RectTransform childTransform = (child as Transform).GetComponent<RectTransform>();
                childTransform.sizeDelta = new Vector2(width, height/transform.childCount);
            }
        }
    }

}