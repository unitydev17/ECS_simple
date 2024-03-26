using Leopotam.Ecs;
using UnityEngine;

public class PlayerMoveByPhysicsSystem : IEcsRunSystem
{
    private EcsFilter<Player, PlayerInputData> _filter;
    private RuntimeData _runtimeData;
    private Camera _camera;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var player = ref _filter.Get1(i);
            ref var input = ref _filter.Get2(i);

            PuddleMove(player, input);
        }
    }

    private void PuddleMove(Player player, PlayerInputData input)
    {
        var (tap, movePos) = input.moveInput;
        if (!tap) return;

        var targetPos = _camera.ScreenToWorldPoint(movePos);
        targetPos.z = 0;

        var currPos = player.transform.position;


        var nextPos = Vector3.Lerp(currPos, targetPos, Time.deltaTime * 100);
        
        player.collider.GetComponent<Rigidbody2D>().MovePosition(nextPos);

    }

    private Vector3 BoundPosition(Vector3 currPos, Vector3 nextPos)
    {
        if (nextPos.x < -_runtimeData.boardExtents.x || nextPos.x > _runtimeData.boardExtents.x)
        {
            nextPos.x = currPos.x;
        }

        if (nextPos.y < -_runtimeData.boardExtents.y || nextPos.y > _runtimeData.boardExtents.y)
        {
            nextPos.y = currPos.y;
        }

        return nextPos;
    }
}