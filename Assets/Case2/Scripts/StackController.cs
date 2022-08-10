using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Case2
{
    public class StackController : MonoBehaviour
    {
        public AnimationCurve PieceMovementCurve;
        public Transform Stack;
        public GameObject PiecePrefab;
        public List<PairGradient> Gradients;
        
        private float spawnXDistance;
        private Transform currentPiece;
        private Transform prevPiece;
        private bool running;
        private TweenerCore<Vector3, Vector3, VectorOptions> tween;
        private LevelParameter param;
        private int pieceCount;
        private float colorCount;
        private PairedGradient targetColor;
        private PairGradient sourceColor;

        private void OnEnable()
        {
            EventBus.OnLevelReady += OnLevelReady;
            EventBus.OnGameStart += OnGameStart;
            EventBus.OnGameEnd += OnGameEnd;
        }
        private void OnDisable()
        {
            EventBus.OnGameStart-= OnGameStart;
            EventBus.OnGameEnd -= OnGameEnd;
            EventBus.OnLevelReady -= OnLevelReady;
        }
        
        private void OnLevelReady(LevelParameter levelParameter)
        {
            GetRandomStartingGradient();
            param = levelParameter;
            CreateStartingPieces();
        }
        private void OnGameStart()
        {
            running = true;
            spawnXDistance = param.Width;
            CreateNewPiece();

        }
        private void OnGameEnd()
        {
            running = false;
        }

        private void CreateStartingPieces()
        {
            currentPiece = CreateNewPiece(0);
            CreateNewPiece(-1);
        }

        private Transform CreateNewPiece(float posZ)
        {
            var go = Instantiate(PiecePrefab, Stack).transform;
            go.localPosition = new Vector3(0,  param.Height / -2f, param.Length*posZ);
            go.localScale = new Vector3(param.Width, param.Height, param.Length);
            go.GetComponent<MeshRenderer>().material.color = GetNewColor();
            return go;
        }
        
        private void CreateNewPiece()
        {
            pieceCount++;
            var piece = Instantiate(currentPiece,Stack).transform;
            piece.name = "p" + pieceCount;
            var direction = GetRandomDirection();
            var currentPos= currentPiece.localPosition;
            var currentScale = currentPiece.localScale;
            var horizontalPosition = (spawnXDistance +currentScale.x/2f)*direction;
            var startingPosition=new Vector3(horizontalPosition, currentPos.y,currentPos.z+currentScale.z);
            piece.localPosition = startingPosition;
            piece.GetComponent<MeshRenderer>().material.color = GetNewColor();
            tween = piece.DOLocalMoveX(-startingPosition.x, param.Speed)
                .SetEase(PieceMovementCurve)
                .OnComplete(OnPieceMoved);
            prevPiece = currentPiece;
            currentPiece = piece;
        }

        private float GetRandomDirection()
        {
            return Random.value > 0.5f? 1:-1;
        }
        private void OnPieceMoved()
        {
            //fail trigger
            if (pieceCount >= param.PieceCount)
            {
                return;
            }
            CreateNewPiece();
        }

        private void OnPiecePlaced()
        {
            if (pieceCount >= param.PieceCount)
            {
                return;
            }

            running = true;
            CreateNewPiece();
        }

        private void Update()
        {
            if(!running)
                return;
            if (!Input.GetMouseButtonDown(0)) 
                return;
            
            running = false;
            var remainingTime = tween.Duration() - tween.Elapsed();
            tween.Kill();
            var currentPosition= currentPiece.localPosition;
            var currentScale = currentPiece.localScale;
            var prevPosition = prevPiece.localPosition;
            var widthDifference = currentPosition.x - prevPosition.x;
            var absoluteWidthDifference = Mathf.Abs(widthDifference);
            
            
            if (absoluteWidthDifference < param.ToleranceWidth)//perfect
            {
                currentPosition.x = prevPosition.x;
                currentPiece.localPosition = currentPosition;
                DOVirtual.DelayedCall(remainingTime, OnPiecePlaced);
            }
            else if(absoluteWidthDifference>currentScale.x)//early/late tap (out of bounds)
            {
                prevPosition.z += param.Length/ 2f;
            }
            else
            {
                CutPiece(currentScale, absoluteWidthDifference, currentPosition, widthDifference, remainingTime);
            }
        }

        private void CutPiece(Vector3 currentScale, float absWidthDifference, Vector3 currentPosition,
            float widthDifference, float remainingTime)
        {
            var cutWidth = currentScale.x - absWidthDifference;
            currentPiece.localScale = new Vector3(cutWidth, currentScale.y, currentScale.z);
            var centeredPositionX = currentPosition.x - widthDifference / 2f;
            currentPiece.localPosition = new Vector3(centeredPositionX, currentPosition.y, currentPosition.z);

            var cutPiece = Instantiate(currentPiece, Stack);
            var cutScale = cutPiece.localScale;
            cutScale.x = absWidthDifference;
            var cutDirection = Mathf.Sign(widthDifference);
            cutPiece.localScale = cutScale;
            var cutCenteredPositionX = centeredPositionX + cutDirection * ((cutWidth + cutScale.x) / 2f);
            cutPiece.localPosition = new Vector3(cutCenteredPositionX, currentPosition.y, currentPosition.z);
            var body = cutPiece.GetComponent<Rigidbody>();
            body.useGravity = true;
            body.isKinematic = false;
            body.AddForce(new Vector3(cutDirection*param.Length,0,0), ForceMode.Impulse);
            DOVirtual.DelayedCall(remainingTime, OnPiecePlaced);
        }

        private Color GetNewColor()
        {
            colorCount++;
            if (colorCount >= targetColor.Length)
            {
                colorCount = 0;
                sourceColor = targetColor.Pair;
                targetColor = sourceColor.PairGradients[Random.Range(0, sourceColor.PairGradients.Count)];
            }
            return Color.Lerp(sourceColor.Color, targetColor.Pair.Color, colorCount / targetColor.Length);
        }

        private void GetRandomStartingGradient()
        {
            var index = Random.Range(0, Gradients.Count);
            sourceColor = Gradients[index];
            targetColor = sourceColor.PairGradients[Random.Range(0, sourceColor.PairGradients.Count)];
        }
    }
}