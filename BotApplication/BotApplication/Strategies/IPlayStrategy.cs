using BotApplication.Cards.Interfaces;
using BotApplication.Players.Interfaces;

namespace BotApplication.Strategies
{
    public interface IPlayStrategy<in TCard>
        where TCard: ICard<TCard>
    {
        double CalculateAttackScore(
            ILocalPlayer localPlayer,
            IPlayer enemyPlayer,
            TCard target);

        double CalculatePlayScore(
            ILocalPlayer localPlayer,
            IPlayer enemyPlayer);
    }
}