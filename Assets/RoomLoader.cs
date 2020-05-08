using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    private void Awake()
    {
        foreach (var wall in Map.GetWalls())
        {
            var wallInfo = wall.Value;
            var wallObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallObj.transform.localScale = new Vector3(wallInfo.Length, wallInfo.Height, 1);
            wallObj.transform.position = wallInfo.Position;
            wallObj.transform.eulerAngles = wallInfo.EulerAngles;

            wallObj.transform.SetParent(transform);
        }

        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x, localScale.y, -1);
    }
}