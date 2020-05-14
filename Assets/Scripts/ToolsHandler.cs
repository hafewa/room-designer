using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ToolsHandler : MonoBehaviour
{
    public GameObject drawingBoard;
    
    public enum Tool
    {
        None,
        DrawFloor,
        DrawWall,
        DrawWindow,
        Eraser
    }

    public static Tool CurrentTool { get; private set; } = Tool.None;

    #region ToolScripts

    private LinePlacer _linePlacer;
    private FloorPlacer _floorPlacer;
    private WindowPlacer _windowPlacer;
    private Eraser _eraser;

    #endregion

    #region ToolBtns

    public Image drawWallBtn;
    public Image drawFloorBtn;
    public Image eraserBtn;

    #endregion


    private void Awake()
    {
        _eraser = drawingBoard.GetComponent<Eraser>();
        _linePlacer = drawingBoard.GetComponent<LinePlacer>();
        _windowPlacer = drawingBoard.GetComponent<WindowPlacer>();
        _floorPlacer = drawingBoard.GetComponent<FloorPlacer>();
        
        ResetAll();
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
                _linePlacer.enabled = true;
                SetSelected(drawWallBtn);

                break;
            case Tool.DrawWindow:
                _windowPlacer.enabled = true;
                SetSelected(drawWallBtn);

                break;
            case Tool.DrawFloor:
                _floorPlacer.enabled = true;
                SetSelected(drawFloorBtn);

                break;
            case Tool.Eraser:
                _eraser.enabled = true;
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

        _linePlacer.enabled = false;
        _floorPlacer.enabled = false;
        _eraser.enabled = false;
        _windowPlacer.enabled = false;
    }
}