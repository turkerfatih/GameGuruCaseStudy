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
        public static Action<Placement> OnPiecePlaced;
        public static Action<LevelParameter> OnLevelReady;
        public static Action<int> OnLevelNumberChanged;
    }
}