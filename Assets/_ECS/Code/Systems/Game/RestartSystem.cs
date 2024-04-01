using Leopotam.Ecs;
using UnityEngine;

public class RestartSystem : IEcsRunSystem
{
    private EcsFilter<Player> _filterPlayer;
    private EcsFilter<Bot> _filterBot;
    private EcsFilter<Ball> _filterBall;

    private RuntimeData _runtimeData;
    private SceneData _sceneData;

    public void Run()
    {
        if (!_runtimeData.restartRequired) return;

        RestartPlayer();
        RestartBot();
        RestartBall();


        _runtimeData.restartRequired = false;
        _runtimeData.goal = false;
    }

    private void RestartPlayer()
    {
        foreach (var i in _filterPlayer)
        {
            ref var player = ref _filterPlayer.Get1(i);
            player.transform.position = _sceneData.puddleSpawnPoint.position;
        }
    }

    private void RestartBot()
    {
        foreach (var i in _filterBot)
        {
            ref var bot = ref _filterBot.Get1(i);
            bot.transform.position = _sceneData.botSpawnPoint.position;
        }
    }

    private void RestartBall()
    {
        foreach (var i in _filterBall)
        {
            ref var ball = ref _filterBall.Get1(i);
            ball.trailRenderer.emitting = false;
            ball.rigidBody.position = _sceneData.ballSpawnPoint.position;
            var trail = ball.trailRenderer;

            _runtimeData.DelayedCall(0.1f, () => trail.emitting = true);
        }
    }
}