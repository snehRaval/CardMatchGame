using System;

namespace CyberSpeed.CardsMatchGame
{
    public interface IGameEvents
    {
        event Action<int, int> OnGameStarted;
        event Action<IScoreData> OnGameOver;
        event Action OnGamePaused;
        event Action OnGameResumed;
        event Action OnGameSave;
    }
}


