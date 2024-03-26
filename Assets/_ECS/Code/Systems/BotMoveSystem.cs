using Leopotam.Ecs;
using UnityEngine;

public class BotMoveSystem : IEcsRunSystem
{
    private EcsFilter<Bot> _filterBot;
    private EcsFilter<Ball> _filterBall;
    private float _targetBallX;
    private float _deltaTime;
    private float _deltaTimeTarget;

    public void Run()
    {
        ref var bot = ref _filterBot.Get1(0);
        ref var ball = ref _filterBall.Get1(0);

        MoveBot(ref bot);
        FollowByRandom(ball);
    }

    private void FollowByRandom(Ball ball)
    {
        _deltaTime += Time.deltaTime;
        if (_deltaTime < _deltaTimeTarget) return;

        _deltaTimeTarget = Random.value * 0.1f;
        _deltaTime = 0;
        _targetBallX = ball.transform.position.x;
    }

    private void MoveBot(ref Bot bot)
    {
        var currPosX = bot.transform.position.x;
        var targetPosX = _targetBallX;
        var nextPosX = Mathf.Lerp(currPosX, targetPosX, Time.deltaTime * 5);
        bot.transform.SetX(nextPosX);
    }
}