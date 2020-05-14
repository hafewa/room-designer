using UnityEngine;

public class Eraser : MonoBehaviour
{
    private void Awake()
    {
        Wall.OnWallClicked += DeleteWall;
    }
    
    private void OnDestroy()
    {
        Wall.OnWallClicked -= DeleteWall;
    }

    private void DeleteWall(Wall wall)
    {
        if (ToolsHandler.CurrentTool != ToolsHandler.Tool.Eraser) return;
        
        Map.DeleteWall(wall.wallId);
        Destroy(wall.gameObject);
    }
}