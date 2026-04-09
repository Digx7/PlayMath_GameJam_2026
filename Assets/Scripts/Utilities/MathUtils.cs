using UnityEngine;

namespace Digx7.Utils
{
    public static class MathUtils
    {
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            if (Mathf.Approximately(fromMax - fromMin, 0))
            {
                Debug.LogWarning("MathUtils.Map: fromMax and fromMin are too close. Returning toMin.");
                return toMin; // Avoid division by zero
            }

            float normalizedValue = (value - fromMin) / (fromMax - fromMin);
            return toMin + normalizedValue * (toMax - toMin);
        }

        public static Vector2 GetRatio(int a, int b)
        {
            int gcd = CalculateGCD(a, b);
            return new Vector2(a / gcd, b / gcd);
        }

        public static Vector2 GetRatio(float a, float b)
        {
            if (Mathf.Approximately(a, 0) || Mathf.Approximately(b, 0))
            {
                Debug.LogWarning("MathUtils.GetRatio: One of the inputs is approximately zero. Returning (0, 0).");
                return Vector2.zero; // Avoid division by zero
            }

            float ratioA = a / b;
            float ratioB = b / a;
            return new Vector2(ratioA, ratioB);
        }

        public static Vector2 GetRatio(Vector2Int input)
        {
            return GetRatio(input.x, input.y);
        }
        
        public static int CalculateGCD(int a, int b)
        {
            // Ensure absolute values to handle negative inputs
            a = Mathf.Abs(a);
            b = Mathf.Abs(b);

            while (b != 0)
            {
                int remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }

        public static int CalculateLCM(int a, int b)
        {
            if (a == 0 || b == 0)
                return 0; // LCM is zero if either number is zero

            int gcd = CalculateGCD(a, b);
            return Mathf.Abs(a * b) / gcd;
        }

    } 
}
    