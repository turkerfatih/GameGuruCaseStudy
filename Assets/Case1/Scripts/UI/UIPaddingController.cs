using UnityEngine;
using UnityEngine.UI;

namespace Case1
{
    public class UIPaddingController : MonoBehaviour
    {
        public RectTransform Panel;
    
        private void OnEnable()
        {
            EventBus.ViewBoundsChange += ViewBoundsChange;
        }

        private void OnDisable()
        {
            EventBus.ViewBoundsChange -= ViewBoundsChange;
        }

        private void ViewBoundsChange(Bounds bounds)
        {
            var canvas = Panel.GetComponentInParent<Canvas>();
            var scalar = canvas.GetComponent<CanvasScaler>();
            if (scalar.screenMatchMode != CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
            {
                EventBus.ViewBoundsChanged?.Invoke(bounds,1);
                return;
            }
            var referenceResolution = scalar.referenceResolution;
            var panelSize = referenceResolution.y + Panel.sizeDelta.y;
            var panelRatio = panelSize / referenceResolution.y;
        
            EventBus.ViewBoundsChanged?.Invoke(bounds,(1-panelRatio));
            return;
        }
    }
}