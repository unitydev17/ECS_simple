using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private StaticData _staticData;
    private SceneData _sceneData;

    public void Init()
    {
        var playerEntity = _ecsWorld.NewEntity();
        ref var player = ref playerEntity.Get<Player>();
        playerEntity.Get<PlayerInputData>();

        var playerGo = Object.Instantiate(_staticData.puddlePrefab);
        playerGo.transform.position = _sceneData.puddleSpawnPoint.position;

        player.transform = playerGo.transform;
        player.rigidBody = playerGo.GetComponent<Rigidbody2D>();

    }
}