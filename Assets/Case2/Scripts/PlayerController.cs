using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Case2
{
    public class PlayerController : MonoBehaviour
    {
        public Transform Mover;
        public Transform Container;
        public Model Model;

        private LevelParameter param;
        private float playerSpeed;
        private WaitForSeconds waitForAWhile = new WaitForSeconds(0.2f);
        private bool running;
        private void OnEnable()
        {
            EventBus.OnGameStart += OnGameStart;
            EventBus.OnLevelReady+= OnLevelReady;
        }
        

        private void OnDisable()
        {
            EventBus.OnGameStart -= OnGameStart;
            EventBus.OnLevelReady -= OnLevelReady;
        }

        private void OnLevelReady(LevelParameter levelParameter)
        {
            param = levelParameter;
        }
        private void OnGameStart()
        {
            Model.PrepareRun();
            DOVirtual.DelayedCall(param.Speed/2f, StartMoving);
        }

        private void StartMoving()
        {
            var finalPosition =  param.FinalPosition;
            var pieceSpeed= param.Length / param.Speed;
            playerSpeed = finalPosition / pieceSpeed;
            Mover.DOLocalMoveZ(finalPosition,playerSpeed )
                .SetEase(Ease.Linear).OnComplete(MoveComplete);
            Model.Run();
            running = true;
            StartCoroutine(GroundCheck());
        }

        private void MoveComplete()
        {
            Model.Dance();
            EventBus.OnGameSuccess?.Invoke();
        }

        private void FailJump()
        {
            var body = Container.GetComponent<Rigidbody>();
            body.isKinematic = false;
            body.useGravity = true;
            Model.Fail();
            DOVirtual.DelayedCall(2, SlowDown);
        }

        private void SlowDown()
        {
            var body = Container.GetComponent<Rigidbody>();
            body.drag = 7.5f;
        }

        IEnumerator GroundCheck()
        {
            while (running)
            {
                Debug.DrawRay(Container.position+Vector3.up*param.Height/2f,Vector3.down*param.Height,Color.magenta,10);
                if (!Physics.Raycast(Container.position+Vector3.up*param.Height/2f, Vector3.down, param.Height))
                {
                    running = false;
                    Mover.DOKill();
                    FailJump();
                }
                yield return waitForAWhile;
            }
        }

    }
}