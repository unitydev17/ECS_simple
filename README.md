# ECS simple test game

### Тестовое приложение с использованием ECS архитектуры и фреймворка [LEO-ECS](https://github.com/Leopotam/ecs)

![Onion architecture](/Assets/_Project/Images/readme/ecs_game.png)

## ECSStartup.cs

Содержит список систем игры

```C#
    private void Start()
    {
        _ecsWorld = new EcsWorld();
        _systems = new EcsSystems(_ecsWorld);
        _runtimeData = new RuntimeData();

        _systems.Add(new PlayerInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new PlayerMoveSystem())
            .Add(new BallSpawnSystem())
            .Add(new BallMovePhysicsSystem())
            .Add(new BotInitSystem())
            .Add(new BotMoveSystem())
            .Add(new GoalSystem())
            .Add(new RestartSystem())
            .Add(new ViewPortSystem())
            .Add(new DelaySpawnSystem())
            .Add(new DelayActionSystem())
```
а также объектов для инъекции в эти же системы
```C#
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(sceneData.camera)
            .Inject(_systems)
            .Inject(_runtimeData)
            .Inject(sceneData.ui)
            .Init();
    }
```




## PlayerInitSystem.cs

Пример системы инициализации - инициализация игрока

```C#
public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private StaticData _staticData;
    private SceneData _sceneData;
    private RuntimeData _runtimeData;

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
```

