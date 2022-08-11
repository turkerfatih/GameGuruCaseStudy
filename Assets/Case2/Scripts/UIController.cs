using System;
using TMPro;
using UnityEngine;

namespace Case2
{
    public class UIController : MonoBehaviour
    {
        public GameObject LoadingPanel;
        public GameObject StartButton;
        public GameObject ContinueButton;
        public TextMeshProUGUI LevelNumber;
        public GameObject FailPanel;

        private void Awake()
        {
            LoadingPanel.SetActive(true);
        }

        private void OnEnable()
        {
            EventBus.OnLevelReady += OnLevelGenerated;
            EventBus.OnGameFail += OnFail;
            EventBus.OnLevelNumberChanged += ShowLevel;
            EventBus.OnGameStart += OnGameStart;
            EventBus.OnGameSuccess += OnSuccess;
            EventBus.OnContinue += OnContinueNewLevel;
            EventBus.OnGameReplay += OnGameReplay;
        }
        private void OnDisable()
        {
            EventBus.OnLevelReady -= OnLevelGenerated;
            EventBus.OnGameFail -= OnFail;
            EventBus.OnLevelNumberChanged -= ShowLevel;
            EventBus.OnGameStart -= OnGameStart;
            EventBus.OnGameSuccess -= OnSuccess;
            EventBus.OnContinue -= OnContinueNewLevel;
            EventBus.OnGameReplay -= OnGameReplay;
        }
        private void OnGameStart()
        {
            StartButton.SetActive(false);
        }
        private void OnGameReplay()
        {
            FailPanel.SetActive(false);
            StartButton.SetActive(true);
        }

        private void OnLevelGenerated(LevelParameter levelParameter)
        {
            LoadingPanel.SetActive(false);
            StartButton.SetActive(true);
        }

        private void OnFail()
        {
            FailPanel.SetActive(true);
        }
        
        private void ShowLevel(int levelNumber)
        {
            LevelNumber.text = "Level "+levelNumber;
        }

        private void OnSuccess()
        {
            ContinueButton.SetActive(true);
        }

        private void OnContinueNewLevel()
        {
            LoadingPanel.SetActive(true);
            ContinueButton.SetActive(false);
        }

    }
}