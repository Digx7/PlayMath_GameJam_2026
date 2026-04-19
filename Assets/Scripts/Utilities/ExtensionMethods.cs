using UnityEngine;
using System;

public static class ExtensionMethods
{
    public static float Remap (this float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;       
       
        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;
       
        return to;
    }

    public static int CompareStoryModeLevelNames(LevelData x, LevelData y)
    {
        if (x == null)
        {
            if (y == null)
            {
                // If x is null and y is null, they're
                // equal.
                return 0;
            }
            else
            {
                // If x is null and y is not null, y
                // is greater.
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (y == null)
                // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                // ...and y is not null, compare the names

                string[] xNameParts = x.name.Split('-', System.StringSplitOptions.RemoveEmptyEntries);
                int xName_World = int.Parse(xNameParts[0]);
                int xName_Level = int.Parse(xNameParts[1]);

                string[] yNameParts = y.name.Split('-', System.StringSplitOptions.RemoveEmptyEntries);
                int yName_World = int.Parse(yNameParts[0]);
                int yName_Level = int.Parse(yNameParts[1]);

                if (xName_World == yName_World)
                {
                    // Same world
                    if( xName_Level == yName_Level)
                    {
                        // same level
                        return 0;
                    }
                    else if (xName_Level > yName_Level )
                    {
                        // x level is greater
                        return 1;
                    }
                    else 
                    {
                        // y level is greater
                        return -1;
                    }
                }
                else if ( xName_World > yName_World )
                {
                    // x world is greater
                    return 1;
                }
                else
                {
                    // y world is greater
                    return -1;
                }
                
            }
        }
    }
}