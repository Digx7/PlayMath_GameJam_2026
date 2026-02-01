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

            public Grid(int newX_Length, int newY_Length)
            {
                SetEmptyGrid(newX_Length, newY_Length);
            }

            public Grid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
            {
                SetGrid(newData, newX_Length, newY_Length);
            }

            public void SetEmptyGrid(int newX_Length, int newY_Length)
            {
                data = new List<CoordinateFlagPair>();

                x_Length = newX_Length;
                y_Length = newY_Length;

                for (int x = 0; x < x_Length; x++)
                {
                    for (int y = 0; y < y_Length; y++)
                    {
                        CoordinateFlagPair coordinateFlagPair = new CoordinateFlagPair();
                        coordinateFlagPair.coordinate = new Vector2Int(x,y);
                        coordinateFlagPair.flag = 0;

                        data.Add(coordinateFlagPair);
                    }
                }

                PrintGrid();
            }

            public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
            {
                data = newData;

                x_Length = newX_Length;
                y_Length = newY_Length;

                PrintGrid();
            }

            public void UpdateCoordinateFlag(Vector2Int coordinate, int newFlag)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if(data[i].coordinate == coordinate) 
                    {
                        CoordinateFlagPair coordinateFlagPair = data[i];
                        coordinateFlagPair.flag = newFlag;
                        data[i] = coordinateFlagPair;
                    }
                }
            }

            public int GetFlagofGridSpace(Vector2Int coordinate)
            {
                if(data.Count == 0) return -1;
                
                for (int i = 0; i < data.Count; i++)
                {
                    if(data[i].coordinate == coordinate) return data[i].flag;
                }

                return -1;
            }

            public int GetFlagofGridSpace(int x, int y)
            {
                Vector2Int coordinate = new Vector2Int(x,y);
                return GetFlagofGridSpace(coordinate);
            }

            public bool IsCoordinateInGrid(Vector2Int coordinate)
            {
                return IsCoordinateInGrid(coordinate.x, coordinate.y);
            }

            public bool IsCoordinateInGrid(int x, int y)
            {
                if(x >= 0 && x < x_Length && y >= 0 && y < y_Length) return true;
                else return false;
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

