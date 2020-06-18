using UnityEngine;

public class Eraser : MonoBehaviour
{
    private void Awake()
    {
        Wall.OnWallClicked += DeleteWall;
        Floor.OnFloorClicked += DeleteFloor;
    }
    
    private void OnDestroy()
    {
        Wall.OnWallClicked -= DeleteWall;
        Floor.OnFloorClicked -= DeleteFloor;
    }

    private void DeleteWall(Wall wall)
    {
        if (ToolsHandler.CurrentTool != ToolsHandler.Tool.Eraser) return;
        
        Map.DeleteWall(wall.wallId);
        Destroy(wall.gameObject);
    }
    
    private void DeleteFloor(Floor floor)
    {
        if (ToolsHandler.CurrentTool != ToolsHandler.Tool.Eraser) return;
        
        Map.DeleteFloor(floor.floorId);
        Destroy(floor.gameObject);
    }
}