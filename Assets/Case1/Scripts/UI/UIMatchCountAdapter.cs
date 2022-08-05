using UnityEngine;
using UnityEngine.UI;

namespace Case1
{
    public class UIMatchCountAdapter : UISliderValueDisplayer
    {

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.wholeNumbers = true;
            slider.minValue = Config.MinimumMatchCount;
            slider.maxValue = Config.MaximumMatchCount;
            slider.value = Mathf.Lerp(Config.MinimumMatchCount, Config.MaximumMatchCount, 0.5f);
        }
    }
}