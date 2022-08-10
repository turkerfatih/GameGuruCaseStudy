using DG.Tweening;
using UnityEngine;

namespace Case2
{
    public class ScaleAnimator : MonoBehaviour
    {
        public float Scale;
        public float Duration;
        public float Delay;

        private void OnEnable()
        {
            transform.DOScale(Vector3.one * Scale, Duration).SetLoops(-1, LoopType.Yoyo).SetDelay(Delay);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }
    }
}