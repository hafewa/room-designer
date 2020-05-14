using System.Linq;
using Boo.Lang;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Material material;

    private void Awake()
    {
        GenerateWalls();

        GenerateFloors();

        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x, localScale.y, -1);
        var roomPos = transform.position;
        roomPos.y = Map.WallHeight / 2;

        transform.position = roomPos;
    }

    private void GenerateWalls()
    {
        foreach (var wall in Map.GetWalls())
        {
            var wallInfo = wall.Value;
            var wallObj = new GameObject();
            var mF = wallObj.AddComponent<MeshFilter>();
            var render = wallObj.AddComponent<MeshRenderer>();
            render.material = material;
            var msh = new Mesh();

            var rightPoint = wallInfo.length / 2;
            var topPoint = Map.WallHeight / 2;
            var wallVertices = new List<Vector3>
            {
                new Vector3(-rightPoint, topPoint), // top left vert
                new Vector3(-rightPoint, -topPoint), // bottom left vert
                new Vector3(rightPoint, topPoint), // top right vert
                new Vector3(rightPoint, -topPoint) // bottom right vert
            };

            const int topWindowPoint = 8;
            const int bottomWindowPoint = -5;
            var tris = new List<int>();
            if (wallInfo.windows.Count == 0)
            {
                tris.Add(0).Add(1).Add(2).Add(2).Add(1).Add(3);
            }
            else
            {
                var prevTopExtraInd = 0;
                var prevTopInd = 0;
                var prevBottomInd = 0;
                var prevBottomExtraInd = 1;
                for (var i = 0; i < wallInfo.windows.Count; i++)
                {
                    var window = wallInfo.windows[i];
                    wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                        topWindowPoint)); // left top window vert
                    var leftTopVertInd = wallVertices.Count - 1;
                    wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                        bottomWindowPoint)); // left bottom window vert
                    var leftBottomVertInd = leftTopVertInd + 1;
                    wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                        topWindowPoint)); // right top window vert
                    var rightTopVertInd = leftTopVertInd + 2;
                    wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                        bottomWindowPoint)); // right bottom window vert
                    var rightBottomVertInd = leftTopVertInd + 3;

                    var rightTopExtraVertInd = 0;
                    var rightBottomExtraVertInd = 0;
                    if (i + 1 < wallInfo.windows.Count)
                    {
                        wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                            topPoint)); // right top extra window vert
                        rightTopExtraVertInd = leftTopVertInd + 4;
                        wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                            -topPoint)); // right bottom extra window vert
                        rightBottomExtraVertInd = leftTopVertInd + 5;
                    }

                    if (i == 0)
                    {
                        tris.Add(0).Add(1).Add(leftBottomVertInd);
                        tris.Add(leftTopVertInd).Add(0).Add(leftBottomVertInd);
                        tris.Add(leftBottomVertInd).Add(1).Add(rightBottomVertInd);

                        if (i + 1 == wallInfo.windows.Count)
                        {
                            tris.Add(prevTopExtraInd).Add(leftTopVertInd).Add(2);

                            tris.Add(2).Add(leftTopVertInd).Add(rightTopVertInd);
                            tris.Add(rightBottomVertInd).Add(1).Add(3);

                            tris.Add(2).Add(rightTopVertInd).Add(3);
                            tris.Add(rightTopVertInd).Add(rightBottomVertInd).Add(3);
                        }
                        else
                        {
                            tris.Add(0).Add(leftTopVertInd).Add(rightTopExtraVertInd);
                            tris.Add(rightTopExtraVertInd).Add(leftTopVertInd).Add(rightTopVertInd);
                            tris.Add(rightBottomVertInd).Add(1).Add(rightBottomExtraVertInd);
                        }
                    }
                    else
                    {
                        tris.Add(prevTopExtraInd).Add(prevTopInd).Add(leftTopVertInd);
                        tris.Add(leftTopVertInd).Add(prevTopInd).Add(leftBottomVertInd);
                        tris.Add(leftBottomVertInd).Add(prevTopInd).Add(prevBottomInd);
                        tris.Add(prevBottomInd).Add(prevBottomExtraInd).Add(leftBottomVertInd);
                        if (i + 1 < wallInfo.windows.Count)
                        {
                            tris.Add(prevTopExtraInd).Add(leftTopVertInd).Add(rightTopExtraVertInd);
                            tris.Add(leftBottomVertInd).Add(prevBottomExtraInd).Add(rightBottomExtraVertInd);

                            tris.Add(rightTopExtraVertInd).Add(leftTopVertInd).Add(rightTopVertInd);
                            tris.Add(rightBottomVertInd).Add(leftBottomVertInd).Add(rightBottomExtraVertInd);
                        }
                        else
                        {
                            tris.Add(prevTopExtraInd).Add(leftTopVertInd).Add(2);
                            tris.Add(leftBottomVertInd).Add(prevBottomExtraInd).Add(3);

                            tris.Add(2).Add(leftTopVertInd).Add(rightTopVertInd);
                            tris.Add(rightBottomVertInd).Add(leftBottomVertInd).Add(3);

                            tris.Add(2).Add(rightTopVertInd).Add(3);
                            tris.Add(rightTopVertInd).Add(rightBottomVertInd).Add(3);
                        }
                    }

                    prevTopExtraInd = rightTopExtraVertInd;
                    prevTopInd = rightTopVertInd;
                    prevBottomInd = rightBottomVertInd;
                    prevBottomExtraInd = rightBottomExtraVertInd;

                    // wall
                    wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                        topWindowPoint, -5)); // left top back vert
                    var leftTopBackInd = wallVertices.Count - 1;
                    wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                        bottomWindowPoint, -5)); // left bottom back vert
                    var leftBottomBackInd = leftTopBackInd + 1;
                    wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                        topWindowPoint, -5)); // right top back vert
                    var rightTopBackInd = leftTopBackInd + 2;
                    wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                        bottomWindowPoint, -5)); // right bottom back vert
                    var rightBottomBackInd = leftTopBackInd + 3;

                    // left wall
                    tris.Add(leftTopVertInd).Add(leftBottomVertInd).Add(leftBottomBackInd);
                    tris.Add(leftBottomBackInd).Add(leftTopBackInd).Add(leftTopVertInd);
                    // top wall
                    tris.Add(leftTopVertInd).Add(leftTopBackInd).Add(rightTopVertInd);
                    tris.Add(rightTopVertInd).Add(leftTopBackInd).Add(rightTopBackInd);
                    // right wall
                    tris.Add(rightTopVertInd).Add(rightTopBackInd).Add(rightBottomBackInd);
                    tris.Add(rightBottomBackInd).Add(rightBottomVertInd).Add(rightTopVertInd);
                    // bottom wall
                    tris.Add(rightBottomVertInd).Add(rightBottomBackInd).Add(leftBottomVertInd);
                    tris.Add(leftBottomVertInd).Add(rightBottomBackInd).Add(leftBottomBackInd);
                }
            }

            msh.vertices = wallVertices.ToArray();
            msh.triangles = tris.ToArray();
            msh.RecalculateNormals();
            mF.mesh = msh;
            
            // Set Gazable layer
            wallObj.layer = 9;
            wallObj.transform.localScale = new Vector3(Map.CellSize, 1, -1);
            wallObj.transform.position = wallInfo.position * Map.CellSize;
            wallObj.transform.eulerAngles = wallInfo.eulerAngles;

            wallObj.transform.SetParent(transform);

            wallObj.AddComponent<MeshCollider>();
        }
    }

    private void GenerateFloors()
    {
        foreach (var floor in Map.GetFloors())
        {
            var floorInfo = floor.Value;
            var floorObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            // Set Gazable layer
            floorObj.layer = 9;
            floorObj.transform.position = floorInfo.position * Map.CellSize;
            floorObj.transform.localScale = new Vector3(floorInfo.size.x * Map.CellSize, floorInfo.size.y, floorInfo.size.z * Map.CellSize);
            floorObj.transform.eulerAngles = new Vector3(0, 90, 0);

            var ceilingObj = Instantiate(floorObj);

            var ceilingPos = ceilingObj.transform.position;
            ceilingPos.y += Map.WallHeight;
            ceilingObj.transform.position = ceilingPos;
            ceilingObj.transform.Rotate(180, 0, 0);
        }
    }
}