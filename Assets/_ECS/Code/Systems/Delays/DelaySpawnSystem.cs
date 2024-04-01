using Leopotam.Ecs;

public class DelaySpawnSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;
    private EcsWorld _ecsWorld;

    public void Run()
    {
        if (_runtimeData.delays.Count == 0) return;

        var delayEntity = _ecsWorld.NewEntity();
        ref var delay = ref delayEntity.Get<Delay>();

        var incomeDelay = _runtimeData.delays.Dequeue();
        delay.delay = incomeDelay.delay;
        delay.delayedAction = incomeDelay.delayedAction;
        delay.active = true;
    }
}