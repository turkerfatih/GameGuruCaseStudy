using System;
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
        public bool AdaptChanges;
        public float GroundCheckInterval;
        private LevelParameter param;
        private float playerSpeed;
        private WaitForSeconds waitForGroundCheckInterval;
        private bool running;
        private Rigidbody body;

        private void Awake()
        {
            body = Container.GetComponent<Rigidbody>();
            waitForGroundCheckInterval = new WaitForSeconds(GroundCheckInterval);
        }

        private void OnEnable()
        {
            EventBus.OnGameStart += OnGameStart;
            EventBus.OnLevelReady+= OnLevelReady;
            EventBus.OnContinue += OnNewLevelRequest;
            EventBus.OnGameReplay += OnGameReplay;
            EventBus.OnPathChange += OnPathChange;
        }
        
        private void OnDisable()
        {
            EventBus.OnGameStart -= OnGameStart;
            EventBus.OnLevelReady -= OnLevelReady;
            EventBus.OnContinue -= OnNewLevelRequest;
            EventBus.OnGameReplay -= OnGameReplay;
            EventBus.OnPathChange -= OnPathChange;
        }
        private void ResetModel()
        {
            Mover.localPosition = Vector3.zero;
            body.isKinematic = true;
            body.useGravity = false;
            body.transform.localPosition = Vector3.zero;
            Model.Idle();
            Mover.DOKill();
        }

        private void OnLevelReady(LevelParameter levelParameter)
        {
            param = levelParameter;
            if (param.Speed < GroundCheckInterval)
            {
                GroundCheckInterval = param.Speed / 2f;
            }
            ResetModel();
        }
        
        private void OnPathChange(float xPosition)
        {
            if(!AdaptChanges)
                return;
            Mover.DOLocalMoveX(xPosition, param.Speed / 2f);
        }

        private void OnGameReplay()
        {
            ResetModel();
        }

        private void OnNewLevelRequest()
        {
            ResetModel();
        }

        private void OnGameStart()
        {
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
            running = false;
            Model.Dance();
            EventBus.OnGameSuccess?.Invoke();
        }

        private void FailJump()
        {
            if(!running)
                return;
            StopAllCoroutines();
            running = false;
            body.isKinematic = false;
            body.useGravity = true;
            Model.Fail();
            DOVirtual.Float(1, 7.5f,1.5f ,SlowDown)
                .SetDelay(1).OnComplete(TriggerFailEvent);
        }
        
        private void SlowDown(float val)
        {
            body.drag = val;
        }

        private void TriggerFailEvent()
        {
            EventBus.OnGameFail?.Invoke();
        }

        IEnumerator GroundCheck()
        {
            while (running)
            {
                if (!Physics.Raycast(Container.position+Vector3.up*param.Height/2f, Vector3.down, param.Height))
                {
                    Mover.DOKill();
                    FailJump();
                }
                yield return waitForGroundCheckInterval;
            }
        }

    }
}