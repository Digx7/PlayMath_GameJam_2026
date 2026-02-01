using UnityEngine;
using System;
using System.Collections.Generic;

namespace Digx7
{
    namespace Grids
    {
        [System.Serializable]
        public class Grid
        {
            public List<CoordinateFlagPair> data;
            public int x_Length = 0;
            public int y_Length = 0;

            public Grid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
            {
                SetGrid(newData, newX_Length, newY_Length);
            }

            public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
            {
                data = newData;

                x_Length = newX_Length;
                y_Length = newY_Length;

                PrintGrid();
            }

            public int GetIDofGridSpace(Vector2Int coordinate)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if(data[i].coordinate == coordinate) return data[i].flag;
                }

                return -1;
            }

            public int GetIDofGridSpace(int x, int y)
            {
                Vector2Int coordinate = new Vector2Int(x,y);
                return GetIDofGridSpace(coordinate);
            }

            public bool IsCoordinateInGrid(Vector2Int coordinate)
            {
                return IsCoordinateInGrid(coordinate.x, coordinate.y);
            }

            public bool IsCoordinateInGrid(int x, int y)
            {
                // TODO

                return true;
            }

            public void PrintGrid()
            {
                Debug.Log($"Printing Grid");
                if(data == null)
                {
                    Debug.LogWarning($"Grid is UNDEFINED");
                }
                Debug.Log($"Size {x_Length},{y_Length}");
                
                for (int i = 0; i < data.Count; i++)
                {
                    Debug.Log($"{data[i].flag}");
                }
            }
        }

        [System.Serializable]
        public struct CoordinateFlagPair
        {
            public Vector2Int coordinate;
            public int flag;
        }
    }
}

