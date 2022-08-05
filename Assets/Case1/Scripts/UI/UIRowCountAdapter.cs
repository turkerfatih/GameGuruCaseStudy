using UnityEngine;
using UnityEngine.UI;

public class UIRowCountAdapter : UISliderValueDisplayer
{

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.wholeNumbers = true;
        slider.minValue = Config.MinimumGridSize;
        slider.maxValue = Config.MaximumGridSize;
        slider.value = Mathf.Lerp(Config.MinimumGridSize, Config.MaximumGridSize, 0.5f);
    }
}
