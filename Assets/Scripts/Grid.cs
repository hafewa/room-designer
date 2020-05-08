using UnityEngine;

public class Grid : MonoBehaviour
{
    public float size = 1f;
    public bool enableGrid = false;
    

    public Vector3 GetNearestPoint(Vector3 position)
    {
        var transformPosition = transform.position;
        position -= transformPosition;

        var xCount = Mathf.RoundToInt(position.x / size);
        var yCount = Mathf.RoundToInt(position.y / size);
        var zCount = Mathf.RoundToInt(position.z / size);

        var result = new Vector3(
            xCount * size,
            yCount * size,
            zCount * size
        );

        result += transformPosition;

        return result;
    }

    private void OnDrawGizmos()
    {
        if (!enableGrid) return;
        Gizmos.color = Color.white;
        var rect = transform.GetComponent<RectTransform>().rect;

        var transformPosition = transform.position;
        var rightSide = transformPosition.x + rect.width;
        var topSide = transformPosition.y + rect.height;
        for (var x = transformPosition.x + size; x < rightSide; x += size)
        {
            for (var y = transformPosition.y + size; y < topSide; y += size)
            {
                var point = GetNearestPoint(new Vector3(x, y, transformPosition.z));
                Gizmos.DrawSphere(point, 5f);
            }
        }
    }
}