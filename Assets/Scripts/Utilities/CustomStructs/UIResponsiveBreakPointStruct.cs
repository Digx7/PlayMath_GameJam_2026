using UnityEngine;
using UnityEngine.Events;
using System;

namespace Digx7.Zygote
{
    [System.Serializable]
    public struct UIResponsiveBreakPoint : IEquatable<UIResponsiveBreakPoint>
    {
        public string breakPointName;
        
        [Header("Screen Width")]
        public float minScreenWidth;
        public float maxScreenWidth;

        [Header("Anchor Points")]
        public Vector2 AnchorMinPoints;
        public Vector2 AnchorMaxPoints;

        [Header("Events")]
        public UnityEvent onBreakPointApplied;

        // Implement IEquatable<T>.Equals(T other) for type-safe, efficient comparison
        public bool Equals(UIResponsiveBreakPoint other)
        {
            return breakPointName == other.breakPointName &&
                   minScreenWidth.Equals(other.minScreenWidth) &&
                   maxScreenWidth.Equals(other.maxScreenWidth) &&
                   AnchorMinPoints.Equals(other.AnchorMinPoints) &&
                   AnchorMaxPoints.Equals(other.AnchorMaxPoints);
        }

        // Override Object.Equals(object obj) to call the type-specific Equals
        public override bool Equals(object obj)
        {
            return obj is UIResponsiveBreakPoint other && Equals(other);
        }

        // Override Object.GetHashCode() so that equal objects have the same hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(breakPointName, minScreenWidth, maxScreenWidth, AnchorMinPoints, AnchorMaxPoints);
        }

        // Overload the == and != operators for intuitive syntax
        public static bool operator ==(UIResponsiveBreakPoint left, UIResponsiveBreakPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UIResponsiveBreakPoint left, UIResponsiveBreakPoint right)
        {
            return !(left == right);
        }
    }
}