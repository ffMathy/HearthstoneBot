using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Strategies.Interfaces
{
    public interface IPlayStrategy
    {
        double CalculateAttackScore(
            ILocalPlayer localPlayer,
            IPlayer enemyPlayer,
            ICard target);

        double CalculatePlayScore(
            ILocalPlayer localPlayer,
            IPlayer enemyPlayer);

        bool FitsCard(
            ICard card);
    }
}