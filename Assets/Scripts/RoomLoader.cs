using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    private void Awake()
    {
        foreach (var wall in Map.GetWalls())
        {
            var wallInfo = wall.Value;
            var wallObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // Set Gazable layer
            wallObj.layer = 9;
            wallObj.transform.localScale = new Vector3(wallInfo.length * Map.CellSize, Map.WallHeight, 1);
            wallObj.transform.position = wallInfo.position * Map.CellSize;
            wallObj.transform.eulerAngles = wallInfo.eulerAngles;

            wallObj.transform.SetParent(transform);
        }

        foreach (var floor in Map.GetFloors())
        {
            var floorInfo = floor.Value;
            var floorObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            
            // Set Gazable layer
            floorObj.layer = 9;
            floorObj.transform.position = floorInfo.position * Map.CellSize;
            floorObj.transform.localScale = floorInfo.size;
            floorObj.transform.eulerAngles = new Vector3(0, 90, 0);
            
            var ceilingObj = Instantiate(floorObj);
            
            var ceilingPos = ceilingObj.transform.position;
            ceilingPos.y += Map.WallHeight;
            ceilingObj.transform.position = ceilingPos;
            ceilingObj.transform.Rotate(180, 0, 0);
        }

        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x, localScale.y, -1);
        var roomPos = transform.position;
        roomPos.y = Map.WallHeight / 2;

        transform.position = roomPos;
    }
}