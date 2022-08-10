using UnityEngine;

namespace Case2
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int RunTrigger = Animator.StringToHash("Run");
        private static readonly int DanceTrigger = Animator.StringToHash("Dance");
        private static readonly int PrepareRunTrigger = Animator.StringToHash("PrepareRun");
        private static readonly int FailTrigger = Animator.StringToHash("Fail");

        public void Run()
        {
            animator.SetTrigger(RunTrigger);
        }

        public void Dance()
        {
            animator.SetTrigger(DanceTrigger);
        }

        public void PrepareRun()
        {
            animator.SetTrigger(PrepareRunTrigger);
        }
        public void Fail()
        {
            animator.SetTrigger(FailTrigger);
        }


    }
}