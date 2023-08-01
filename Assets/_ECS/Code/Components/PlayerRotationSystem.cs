using Leopotam.Ecs;
using UnityEngine;

public class PlayerRotationSystem : IEcsRunSystem
{
    private EcsFilter<Player> _filter;
    private SceneData _sceneData;
    
    public void Run()
    {
        return;
        foreach (var i in _filter)
        {
            ref var player = ref _filter.Get1(i);
            var playerPlane = new Plane(Vector3.up, player.transform.position);
            var ray = _sceneData.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!playerPlane.Raycast(ray, out var hitDistance)) continue;

            player.transform.forward = ray.GetPoint(hitDistance) - player.transform.position;
        }
        
      
    }
}