using BotApplication.Cards.Interfaces;
using BotApplication.Players.Interfaces;

namespace BotApplication.Strategies
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
    }
}