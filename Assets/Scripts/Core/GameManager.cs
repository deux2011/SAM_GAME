using UnityEngine;

namespace SamGame.Core
{
    /// <summary>
    /// 게임 전체 흐름을 관리하는 싱글턴 매니저
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Title;
        public int CurrentTurn { get; private set; } = 1;
        public Season CurrentSeason { get; private set; } = Season.Spring;
        public int CurrentYear { get; private set; } = 190;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartNewGame()
        {
            CurrentState = GameState.Playing;
            CurrentTurn = 1;
            CurrentYear = 190;
            CurrentSeason = Season.Spring;
        }

        public void AdvanceTurn()
        {
            CurrentTurn++;
            AdvanceSeason();
            OnTurnAdvanced();
        }

        private void AdvanceSeason()
        {
            if (CurrentSeason == Season.Winter)
            {
                CurrentSeason = Season.Spring;
                CurrentYear++;
            }
            else
            {
                CurrentSeason++;
            }
        }

        private void OnTurnAdvanced()
        {
            // 각 시스템에 턴 진행 알림
            Debug.Log($"Turn {CurrentTurn}: {CurrentYear}년 {CurrentSeason}");
        }
    }

    public enum GameState
    {
        Title,
        Playing,
        Paused,
        GameOver
    }

    public enum Season
    {
        Spring,  // 봄
        Summer,  // 여름
        Autumn,  // 가을
        Winter   // 겨울
    }
}
