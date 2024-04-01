using Leopotam.Ecs;
using UnityEngine;

public class DelayActionSystem : IEcsRunSystem
{
    private EcsFilter<Delay> _filter;
    private RuntimeData _runtimeData;

    public void Run()
    {
        for (var i = 0; i < _filter.GetEntitiesCount(); i++)
        {
            ref var delay = ref _filter.Get1(i);

            if (!delay.active) continue;

            delay.count += Time.deltaTime;
            if (delay.count < delay.delay) continue;

            delay.active = false;
            delay.delayedAction?.Invoke();
        }
    }
}