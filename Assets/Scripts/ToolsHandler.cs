using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ToolsHandler : MonoBehaviour
{
    public enum Tool
    {
        None,
        DrawFloor,
        DrawWall,
        Eraser
    }

    public static Tool CurrentTool { get; private set; } = Tool.None;
    public LinePlacer linePlacer;
    public FloorPlacer floorPlacer;
    
    public Image drawWallBtn;
    public Image drawFloorBtn;
    public Image eraserBtn;


    private void Awake()
    {
        SetTool(2);
    }

    public void SetTool(int toolInd)
    {
        var tool = (Tool) toolInd;
        if (tool == CurrentTool) return;

        ResetAll();
        CurrentTool = tool;

        switch (CurrentTool)
        {
            case Tool.DrawWall:
                linePlacer.enabled = true;
                SetSelected(drawWallBtn);

                break;
            case Tool.DrawFloor:
                floorPlacer.enabled = true;
                SetSelected(drawFloorBtn);

                break;
            case Tool.Eraser:
                SetSelected(eraserBtn);

                break;
        }
    }
    
    public void SetCellSize(string sizeStr)
    {
        if (sizeStr.Length == 0)
        {
            Map.CellSize = 1;
            return;
        }

        Map.CellSize = float.Parse(sizeStr, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void SetWallHeight(string heightStr)
    {
        if (heightStr.Length == 0)
        {
            Map.WallHeight = 10;
            return;
        }

        Map.WallHeight = float.Parse(heightStr, CultureInfo.InvariantCulture.NumberFormat) * 10;
    }

    private void SetSelected(Graphic btn)
    {
        var color = btn.color;
        color.a = 1f;

        btn.color = color;
    }

    private void ResetAll()
    {
        var color = drawWallBtn.color;
        color.a = 0f;
        drawWallBtn.color = color;
        drawFloorBtn.color = color;
        eraserBtn.color = color;

        linePlacer.enabled = false;
        floorPlacer.enabled = false;
    }
}