using UnityEngine;

namespace Case2
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int RunTrigger = Animator.StringToHash("Run");
        private static readonly int DanceTrigger = Animator.StringToHash("Dance");
        private static readonly int FailTrigger = Animator.StringToHash("Fail");
        private static readonly int IdleTrigger = Animator.StringToHash("Idle");

        public void Run()
        {
            animator.ResetTrigger(IdleTrigger);
            animator.SetTrigger(RunTrigger);
        }

        public void Dance()
        {
            animator.SetTrigger(DanceTrigger);
            animator.ResetTrigger(RunTrigger);
        }
        
        public void Fail()
        {
            animator.SetTrigger(FailTrigger);
        }

        public void Idle()
        {
            animator.SetTrigger(IdleTrigger);
            animator.ResetTrigger(DanceTrigger);
            animator.ResetTrigger(FailTrigger);
        }
    }
}