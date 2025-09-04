namespace CyberSpeed.CardsMatchGame
{
    public interface IScoreData
    {
        int Turns { get; }
        int Matches { get; }
        int Combo { get; }
        int Score { get; }
    }
}