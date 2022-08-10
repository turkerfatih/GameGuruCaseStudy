using System;
using UnityEngine;

namespace Case2
{
    public static class EventBus
    {
        public static Action OnGameStart;
        public static Action OnGameFail;
        public static Action OnGameReplay;
        public static Action OnGameSuccess;
        public static Action OnContinue;
        public static Action OnGameEnd;
        public static Action<LevelParameter> OnLevelReady;
        public static Action OnStopPlayer;
        public static Action<int> OnLevelNumberChanged;
    }
}