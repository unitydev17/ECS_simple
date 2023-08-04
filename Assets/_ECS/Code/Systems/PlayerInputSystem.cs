using Leopotam.Ecs;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{
     private RuntimeData _runtimeData;
     private EcsFilter<PlayerInputData> _filter;
     private EcsWorld _ecsWorld;
 
     public void Run()
     {
         foreach (var i in _filter)
         {
             ref var input = ref _filter.Get1(i);
             input.moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
             
             _runtimeData.spawnBall = Input.GetKeyDown(KeyCode.Space);
             _runtimeData.runBall = Input.GetKeyDown(KeyCode.KeypadEnter);
             
         }
     }
 }