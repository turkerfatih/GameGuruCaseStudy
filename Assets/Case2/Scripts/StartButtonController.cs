using UnityEngine;

namespace Case2
{
    public class StartButtonController : MonoBehaviour
    {
        public void OnClick()
        {
            EventBus.OnGameStart?.Invoke();
        }
    }
}