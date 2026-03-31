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

            public GridTypes gridType = GridTypes.Coordinate;

            public Vector2Int origin;

            public Grid(int newX_Length, int newY_Length, GridTypes newGridType = GridTypes.Coordinate)
            {
                SetEmptyGrid(newX_Length, newY_Length, newGridType);
                origin = new Vector2Int(newX_Length - 1, newY_Length -1);
            }

            public Grid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length, GridTypes newGridType = GridTypes.Coordinate)
            {
                SetGrid(newData, newX_Length, newY_Length, newGridType);
                origin = new Vector2Int(newX_Length - 1, newY_Length -1);
            }

            public void SetEmptyGrid(int newX_Length, int newY_Length, GridTypes newGridType = GridTypes.Coordinate)
            {
                data = new List<CoordinateFlagPair>();

                x_Length = newX_Length;
                y_Length = newY_Length;
                gridType = newGridType;

                for (int x = 0; x < x_Length; x++)
                {
                    for (int y = 0; y < y_Length; y++)
                    {
                        CoordinateFlagPair coordinateFlagPair = new CoordinateFlagPair();
                        coordinateFlagPair.coordinate = new Vector2Int(x,y);
                        coordinateFlagPair.flag = "0";

                        data.Add(coordinateFlagPair);
                    }
                }

                PrintGrid();
            }

            public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length, GridTypes newGridType = GridTypes.Coordinate)
            {
                data = newData;

                x_Length = newX_Length;
                y_Length = newY_Length;
                gridType = newGridType;

                PrintGrid();
            }

            public void UpdateCoordinateFlag(Vector2Int coordinate, string newFlag)
            {
                int i = GridUtils.CoordinateToIndex(coordinate, x_Length, y_Length);
                CoordinateFlagPair coordinateFlagPair = data[i];
                coordinateFlagPair.flag = newFlag;
                data[i] = coordinateFlagPair;
            }

            public string GetFlagofGridSpace(Vector2Int coordinate)
            {
                if(data.Count == 0) return "-1";

                int i = GridUtils.CoordinateToIndex(coordinate, x_Length, y_Length);

                return data[i].flag;
            }

            public string GetFlagofGridSpace(int x, int y)
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

            private Vector2Int IndexToCoordinate(int index)
            {
                return data[index].coordinate;
            }
        }

        public static class GridUtils
        {
            public static int CoordinateToIndex(Vector2Int coordinate, int x_Length, int y_Length)
            {
                return (coordinate.y + (y_Length * coordinate.x));
            }

            public static Vector2Int SpreadSheetCoordinateToGameCoordinate(Vector2Int spreadsheedCoordinate, Vector2Int spreadSheedOriginCoordinate)
            {
                Vector2Int output = spreadsheedCoordinate - spreadSheedOriginCoordinate;
                output.y *= -1;

                return output;
            }

            public static Vector2Int GameCoordinateToSpreadSheetCoordinate(Vector2Int gameCoordinate, Vector2Int topLeftCornerGameCoordinate)
            {
                Vector2Int output = gameCoordinate - topLeftCornerGameCoordinate;
                output.y *= -1;

                return output;
            }
               
        }

        [System.Serializable]
        public struct CoordinateFlagPair
        {
            public Vector2Int coordinate;
            public string flag;
        }

        [System.Serializable]
        public enum GridTypes
        {
            A4,
            Coordinate
        }
    }
}

