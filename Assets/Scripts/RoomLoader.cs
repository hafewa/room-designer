using System.Collections.Generic;
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
                new Vector3(rightPoint, -topPoint), // bottom right vert
                
                new Vector3(-rightPoint, topPoint, -5), // top left back vert
                new Vector3(-rightPoint, -topPoint, -5), // bottom left back vert
                new Vector3(rightPoint, topPoint, -5), // top right back vert
                new Vector3(rightPoint, -topPoint, -5) // bottom right back vert
            };

            const int topWindowPoint = 8;
            const int bottomWindowPoint = -5;
            var tris = new List<int>();
            if (wallInfo.holes.Count == 0)
            {
                tris.Add(0); tris.Add(1); tris.Add(2);
                tris.Add(2); tris.Add(1); tris.Add(3);
                
                tris.Add(6); tris.Add(5); tris.Add(4);
                tris.Add(7); tris.Add(5); tris.Add(6);
            }
            else
            {
                var prevTopExtraInd = 0;
                var prevTopInd = 0;
                var prevBottomInd = 0;
                var prevBottomExtraInd = 1;
                
                var prevTopExtraBackInd = 0;
                var prevTopBackInd = 0;
                var prevBottomBackInd = 0;
                var prevBottomExtraBackInd = 5;
                for (var i = 0; i < wallInfo.holes.Count; i++)
                {
                    if (wallInfo.holes[i].GetType().ToString() == "Window")
                    {
                        var window = wallInfo.holes[i];
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
                        
                        // Back of wall
                        wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                            topWindowPoint, -5)); // left top back window vert
                        var leftTopBackVertInd = leftTopVertInd + 4;
                        wallVertices.Add(new Vector3(-rightPoint + window.startDistance,
                            bottomWindowPoint, -5)); // left bottom window vert
                        var leftBottomBackVertInd = leftTopVertInd + 5;
                        wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                            topWindowPoint, -5)); // right top window vert
                        var rightTopBackVertInd = leftTopVertInd + 6;
                        wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                            bottomWindowPoint, -5)); // right bottom window vert
                        var rightBottomBackVertInd = leftTopVertInd + 7;

                        var rightTopExtraVertInd = 0;
                        var rightBottomExtraVertInd = 0;
                        var rightTopExtraBackVertInd = 0;
                        var rightBottomExtraBackVertInd = 0;
                        if (i + 1 < wallInfo.holes.Count)
                        {
                            wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                                topPoint)); // right top extra window vert
                            rightTopExtraVertInd = leftTopVertInd + 8;
                            wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                                -topPoint)); // right bottom extra window vert
                            rightBottomExtraVertInd = leftTopVertInd + 9;
                            
                            wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                                topPoint, -5)); // right top extra window vert
                            rightTopExtraBackVertInd = leftTopVertInd + 10;
                            wallVertices.Add(new Vector3(-rightPoint + window.endDistance,
                                -topPoint, -5)); // right bottom extra window vert
                            rightBottomExtraBackVertInd = leftTopVertInd + 11;
                        }

                        if (i == 0)
                        {
                            tris.Add(0); tris.Add(1); tris.Add(leftBottomVertInd);
                            tris.Add(leftTopVertInd); tris.Add(0); tris.Add(leftBottomVertInd);
                            tris.Add(leftBottomVertInd); tris.Add(1); tris.Add(rightBottomVertInd);
                            
                            tris.Add(leftBottomBackVertInd); tris.Add(5); tris.Add(4);  
                            tris.Add(leftBottomBackVertInd); tris.Add(4); tris.Add(leftTopBackVertInd);
                            tris.Add(rightBottomBackVertInd); tris.Add(5); tris.Add(leftBottomBackVertInd);

                            if (i + 1 == wallInfo.holes.Count)
                            {
                                tris.Add(0); tris.Add(leftTopVertInd); tris.Add(2);

                                tris.Add(2); tris.Add(leftTopVertInd); tris.Add(rightTopVertInd);
                                tris.Add(rightBottomVertInd); tris.Add(1); tris.Add(3);

                                tris.Add(2); tris.Add(rightTopVertInd); tris.Add(3);
                                tris.Add(rightTopVertInd); tris.Add(rightBottomVertInd); tris.Add(3);
                                
                                tris.Add(6); tris.Add(leftTopBackVertInd); tris.Add(4); 
                                
                                tris.Add(rightTopBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(6); 
                                tris.Add(7); tris.Add(5); tris.Add(rightBottomBackVertInd);

                                tris.Add(7); tris.Add(rightTopBackVertInd); tris.Add(6);
                                tris.Add(7); tris.Add(rightBottomBackVertInd); tris.Add(rightTopBackVertInd);
                            }
                            else
                            {
                                tris.Add(0); tris.Add(leftTopVertInd); tris.Add(rightTopExtraVertInd);
                                tris.Add(rightTopExtraVertInd); tris.Add(leftTopVertInd); tris.Add(rightTopVertInd);
                                tris.Add(rightBottomVertInd); tris.Add(1); tris.Add(rightBottomExtraVertInd);
                                
                                tris.Add(rightTopExtraBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(4);
                                tris.Add(rightTopBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(rightTopExtraBackVertInd);
                                tris.Add(rightBottomExtraBackVertInd); tris.Add(5); tris.Add(rightBottomBackVertInd);
                            }
                        }
                        else
                        {
                            tris.Add(prevTopExtraInd); tris.Add(prevTopInd); tris.Add(leftTopVertInd); 
                            tris.Add(leftTopVertInd); tris.Add(prevTopInd); tris.Add(leftBottomVertInd);
                            tris.Add(leftBottomVertInd); tris.Add(prevTopInd); tris.Add(prevBottomInd);
                            tris.Add(prevBottomInd); tris.Add(prevBottomExtraInd); tris.Add(leftBottomVertInd);
                            
                            tris.Add(leftTopBackVertInd); tris.Add(prevTopBackInd); tris.Add(prevTopExtraBackInd); 
                            tris.Add(leftBottomBackVertInd); tris.Add(prevTopBackInd); tris.Add(leftTopBackVertInd);
                            tris.Add(prevBottomBackInd); tris.Add(prevTopBackInd); tris.Add(leftBottomBackVertInd);
                            tris.Add(leftBottomBackVertInd); tris.Add(prevBottomExtraBackInd); tris.Add(prevBottomBackInd);
                            if (i + 1 < wallInfo.holes.Count)
                            {
                                tris.Add(prevTopExtraInd); tris.Add(leftTopVertInd); tris.Add(rightTopExtraVertInd);
                                tris.Add(leftBottomVertInd); tris.Add(prevBottomExtraInd); tris.Add(rightBottomExtraVertInd);

                                tris.Add(rightTopExtraVertInd); tris.Add(leftTopVertInd); tris.Add(rightTopVertInd);
                                tris.Add(rightBottomVertInd); tris.Add(leftBottomVertInd); tris.Add(rightBottomExtraVertInd);
                                
                                tris.Add(rightTopExtraBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(prevTopExtraBackInd);
                                tris.Add(rightBottomExtraBackVertInd); tris.Add(prevBottomExtraBackInd); tris.Add(leftBottomBackVertInd);

                                tris.Add(rightTopBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(rightTopExtraBackVertInd);
                                tris.Add(rightBottomExtraBackVertInd); tris.Add(leftBottomBackVertInd); tris.Add(rightBottomBackVertInd);
                            }
                            else
                            {
                                tris.Add(prevTopExtraInd); tris.Add(leftTopVertInd); tris.Add(2);
                                tris.Add(leftBottomVertInd); tris.Add(prevBottomExtraInd); tris.Add(3);
                                tris.Add(2); tris.Add(leftTopVertInd); tris.Add(rightTopVertInd);
                                tris.Add(rightBottomVertInd); tris.Add(leftBottomVertInd); tris.Add(3);

                                tris.Add(2); tris.Add(rightTopVertInd); tris.Add(3);
                                tris.Add(rightTopVertInd); tris.Add(rightBottomVertInd); tris.Add(3);
                                
                                tris.Add(6); tris.Add(leftTopBackVertInd); tris.Add(prevTopExtraBackInd);
                                tris.Add(7); tris.Add(prevBottomExtraBackInd); tris.Add(leftBottomBackVertInd);
                                tris.Add(rightTopBackVertInd); tris.Add(leftTopBackVertInd); tris.Add(6);
                                tris.Add(7); tris.Add(leftBottomBackVertInd); tris.Add(rightBottomBackVertInd);

                                tris.Add(7); tris.Add(rightTopBackVertInd); tris.Add(6);
                                tris.Add(7); tris.Add(rightBottomBackVertInd); tris.Add(rightTopBackVertInd);
                            }
                        }

                        prevTopExtraInd = rightTopExtraVertInd;
                        prevTopInd = rightTopVertInd;
                        prevBottomInd = rightBottomVertInd;
                        prevBottomExtraInd = rightBottomExtraVertInd;
                        
                        prevTopExtraBackInd = rightTopExtraBackVertInd;
                        prevTopBackInd = rightTopBackVertInd;
                        prevBottomBackInd = rightBottomBackVertInd;
                        prevBottomExtraBackInd = rightBottomExtraBackVertInd;

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
                        tris.Add(leftTopVertInd); tris.Add(leftBottomVertInd); tris.Add(leftBottomBackInd);
                        tris.Add(leftBottomBackInd); tris.Add(leftTopBackInd); tris.Add(leftTopVertInd);
                        // top wall
                        tris.Add(leftTopVertInd); tris.Add(leftTopBackInd); tris.Add(rightTopVertInd);
                        tris.Add(rightTopVertInd); tris.Add(leftTopBackInd); tris.Add(rightTopBackInd);
                        // right wall
                        tris.Add(rightTopVertInd); tris.Add(rightTopBackInd); tris.Add(rightBottomBackInd);
                        tris.Add(rightBottomBackInd); tris.Add(rightBottomVertInd); tris.Add(rightTopVertInd);
                        // bottom wall
                        tris.Add(rightBottomVertInd); tris.Add(rightBottomBackInd); tris.Add(leftBottomVertInd);
                        tris.Add(leftBottomVertInd); tris.Add(rightBottomBackInd); tris.Add(leftBottomBackInd);
                    }
                    else
                    {
                        
                    }
                }
            }
            
            //left wall side
            tris.Add(5); tris.Add(1); tris.Add(0);
            tris.Add(0); tris.Add(4); tris.Add(5);
            // right wall side
            tris.Add(2); tris.Add(3); tris.Add(7);
            tris.Add(7); tris.Add(6); tris.Add(2);

            msh.vertices = wallVertices.ToArray();
            msh.triangles = tris.ToArray();
            msh.RecalculateNormals();
            mF.mesh = msh;
            
            // Set Gazable layer
            wallObj.layer = 9;
            wallObj.tag = "Wall";
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
            floorObj.tag = "Wall";
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