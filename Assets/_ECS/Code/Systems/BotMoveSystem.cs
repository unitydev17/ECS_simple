using Leopotam.Ecs;

public class BotMoveSystem : IEcsRunSystem
{
    private EcsFilter<Bot> _filterBot;
    private EcsFilter<Ball> _filterBall;

    public void Run()
    {
        ref var bot = ref _filterBot.Get1(0);
        ref var ball = ref _filterBall.Get1(0);

        MoveBot(ref bot, ref ball);
    }

    private static void MoveBot(ref Bot bot, ref Ball ball)
    {
        bot.transform.SetX(ball.transform.position.x);
    }
}