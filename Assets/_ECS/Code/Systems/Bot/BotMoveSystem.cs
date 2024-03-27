using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotMoveSystem : IEcsRunSystem
{
    private enum State
    {
        Follow,
        Attack
    }

    private EcsFilter<Bot> _filterBot;
    private EcsFilter<Ball> _filterBall;
    private SceneData _sceneData;
    private RuntimeData _runtimeData;

    private State _state = State.Follow;
    private Vector2 _targetBall;
    private float _deltaTime;
    private float _deltaTimeTarget;
    private float _startPunchY;


    public void Run()
    {
        if (_runtimeData.goal) return;

        ref var bot = ref _filterBot.Get1(0);
        ref var ball = ref _filterBall.Get1(0);


        CheckBallState(ref ball, ref bot);
        ProcessAttack(ref ball);
        ChangeDirection(ref ball);
        MoveBot(ref bot, ref ball);
    }

    private void ProcessAttack(ref Ball ball)
    {
        if (_state == State.Attack)
        {
            _targetBall.y = ball.transform.position.y + ball.collider.bounds.extents.y * 0.5f;
        }
    }


    private void CheckBallState(ref Ball ball, ref Bot bot)
    {
        var ballPos = ball.transform.position;

        var isBallOnBotSide = ballPos.y > 0;
        var isBallSlowsDown = ball.velocity.magnitude < 1;
        var isBallCloseToBot = Vector2.Distance(ballPos, bot.transform.position) < 10f;

        if (isBallOnBotSide && (isBallSlowsDown || isBallCloseToBot))
        {
            _state = State.Attack;
            _startPunchY = bot.transform.position.y;
        }
        else
        {
            _state = State.Follow;
            _targetBall.y = _sceneData.botSpawnPoint.transform.position.y;
        }
    }

    private void ChangeDirection(ref Ball ball)
    {
        _deltaTime += Time.deltaTime;
        if (_deltaTime < _deltaTimeTarget) return;

        _deltaTimeTarget = Random.value * 0.1f;
        _deltaTime = 0;

        _targetBall.x = ball.transform.position.x;
    }

    private void MoveBot(ref Bot bot, ref Ball ball)
    {
        var isBallOnBotSide = ball.transform.position.y > 0;

        var currPos = bot.transform.position;
        var nextPosX = isBallOnBotSide ? GetNextPosX(currPos) : currPos.x;
        var nextPosY = GetNextPosY(currPos);

        bot.transform.Set(nextPosX, nextPosY);
    }

    private float GetNextPosY(Vector3 currPos)
    {
        var fromY = currPos.y;
        if (_state == State.Attack) fromY = _startPunchY;

        var nextPosY = Mathf.Lerp(fromY, _targetBall.y, Time.deltaTime * 10);
        return nextPosY;
    }

    private float GetNextPosX(Vector3 currPos)
    {
        var toX = _targetBall.x;
        if (_state == State.Attack) toX += Random.value - 1f;

        var nextPosX = Mathf.Lerp(currPos.x, toX, Time.deltaTime * 5);
        return nextPosX;
    }
}