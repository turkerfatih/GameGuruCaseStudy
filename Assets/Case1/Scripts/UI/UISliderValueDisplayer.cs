using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Case1
{
    public class UISliderValueDisplayer : MonoBehaviour
    {
        public Case1Config Config;
        public TextMeshProUGUI Text;
        internal Slider slider;
        public void ValueChanged(Single value)
        {
            Text.SetText(value.ToString());
        }
    }
}