using System;
using Leopotam.Ecs;
using UnityEngine;

public class GoalSystem : IEcsRunSystem
{
    private EcsFilter<Ball> _filter;
    private RuntimeData _runtimeData;
    private UI _ui;

    private readonly Collider2D[] _colliders = new Collider2D[3];

    private readonly int BotGoalLayer = LayerMask.GetMask("BotGoal");
    private readonly int PlayerGoalLayer = LayerMask.GetMask("PlayerGoal");

    public void Run()
    {
        if (_runtimeData.goal) return;

        for (var i = 0; i < _filter.GetEntitiesCount(); i++)
        {
            ref var ball = ref _filter.Get1(i);

            Array.Clear(_colliders, 0, _colliders.Length);
            Physics2D.OverlapCircleNonAlloc(ball.transform.position, ball.collider.radius, _colliders);

            foreach (var collider in _colliders)
            {
                if (collider == null) continue;


                if (collider.IsTouchingLayers(BotGoalLayer))
                {
                    _runtimeData.score.x++;

                    OpenUI();
                    break;
                }

                if (collider.IsTouchingLayers(PlayerGoalLayer))
                {
                    _runtimeData.score.y++;

                    OpenUI();
                    break;
                }
            }
        }
    }

    private void OpenUI()
    {
        _runtimeData.goal = true;
        _ui.OpenScore(_runtimeData.score, CloseCallback);
    }

    private void CloseCallback()
    {
        _runtimeData.restartRequired = true;
    }
}