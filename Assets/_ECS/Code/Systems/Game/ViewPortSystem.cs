using Leopotam.Ecs;
using UnityEngine;

public class ViewPortSystem : IEcsInitSystem
{
    private SceneData _sceneData;
    private RuntimeData _runtimeData;

    public void Init()
    {
        Application.targetFrameRate = 60;

        _runtimeData.boardExtents = CountBoardExtents();
        SetBoardColliders(_runtimeData.boardExtents);
    }

    private void SetBoardColliders(Vector2 extents)
    {
        _sceneData.leftBorder.SetX(-extents.x);
        _sceneData.rightBorder.SetX(extents.x);
        _sceneData.topBorder.SetY(extents.y);
        _sceneData.bottomBorder.SetY(-extents.y);
        _sceneData.botGoal.SetY(extents.y);
        _sceneData.playerGoal.SetY(-extents.y);
    }

    private Vector2 CountBoardExtents()
    {
        var topRightPos = _sceneData.camera.ViewportToWorldPoint(new Vector2(1, 1));
        return new Vector2(topRightPos.x, topRightPos.y);
    }
}