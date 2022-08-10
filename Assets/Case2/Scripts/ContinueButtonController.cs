using UnityEngine;

namespace Case2
{
    public class ContinueButtonController : MonoBehaviour
    {
        public void OnClick()
        {
            EventBus.OnContinue?.Invoke();
        }
    }
}