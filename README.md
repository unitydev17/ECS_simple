# ECS simple test game

### Тестовое приложение с использованием ECS архитектуры и фреймвока LEO-ECS

![Onion architecture](/Assets/_Project/Images/readme/ecs_game.png)

## ECSStartup.cs

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
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(sceneData.camera)
            .Inject(_systems)
            .Inject(_runtimeData)
            .Inject(sceneData.ui)
            .Init();
    }
```




## UI
