using Leopotam.Ecs;
using Object = UnityEngine.Object;

public class BotInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private StaticData _staticData;
    private SceneData _sceneData;
    private RuntimeData _runtimeData;

    public void Init()
    {
        var botEntity = _ecsWorld.NewEntity();
        ref var bot = ref botEntity.Get<Bot>();

        var botGo = Object.Instantiate(_staticData.puddlePrefab);
        botGo.transform.position = _sceneData.botSpawnPoint.position;

        bot.transform = botGo.transform;
    }
}