using UnityEngine;

namespace Case2
{
    public class LevelController : MonoBehaviour
    {
        public Transform Stack;
        public float PieceWidth;
        public float PieceHeight;
        public float PieceLength;
        public float ToleranceWidth;
        public float SpeedMin;
        public float SpeedMax;
        public AnimationCurve SpeedCurve;
        public int PieceCountMin;
        public int PieceCountMax;
        public AnimationCurve PieceCountCurve;
        public int MaxLevel;
        public GameObject FinishLinePrefab;
        private int level;
        private float finishLength;

        private void Awake()
        {
            finishLength = FinishLinePrefab.GetComponent<MeshRenderer>().bounds.extents.z;
        }

        private void OnEnable()
        {
            EventBus.OnContinue += OnContinueNewLevel;
        }

        private void OnDisable()
        {
            EventBus.OnContinue -= OnContinueNewLevel;
        }
        private void Start()
        {
            LoadLevelNumber();
            LoadLevel();
        }

        private void LoadLevelNumber()
        {
            level = PlayerPrefs.GetInt("Level", 1);
        }

        private void SaveLevelNumber()
        {
            PlayerPrefs.SetInt("Level", level);
        }

        private void OnContinueNewLevel()
        {
            level++;
            SaveLevelNumber();
            LoadLevel();
        }

        private void LoadLevel()
        {
            LevelParameter levelParameter = GetDifficulty();
            var finalPosition = new Vector3(0, 0, (PieceLength * levelParameter.PieceCount)+PieceLength/2f+finishLength);
            levelParameter.FinalPosition = finalPosition.z;
            var finish= Instantiate(FinishLinePrefab, finalPosition, Quaternion.identity,Stack);
            
            EventBus.OnLevelNumberChanged?.Invoke(level);
            EventBus.OnLevelReady?.Invoke(levelParameter);
        }

        private LevelParameter GetDifficulty()
        {
            var t = Mathf.InverseLerp(0, MaxLevel, level);
            var pieceCount = Mathf.Lerp(PieceCountMin, PieceCountMax, PieceCountCurve.Evaluate(t));
            var speed = Mathf.Lerp(SpeedMin, SpeedMax, SpeedCurve.Evaluate(t));
            LevelParameter levelParameter = new LevelParameter()
            {
                Width = PieceWidth,
                Height = PieceHeight,
                Length = PieceLength,
                Speed = speed,
                ToleranceWidth =ToleranceWidth,
                PieceCount = Mathf.CeilToInt(pieceCount),
            };
            return levelParameter;
        }
    }
}