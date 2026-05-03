using UnityEngine;
using UnityEngine.Events;
using System;

namespace Digx7.Zygote
{
    [System.Serializable]
    public struct UIResponsiveBreakPoint : IEquatable<UIResponsiveBreakPoint>
    {
        public ScreenBreakPointData screenBreakPointData;

        [Header("Anchor Points")]
        public Vector2 AnchorMinPoints;
        public Vector2 AnchorMaxPoints;

        [Header("Events")]
        public UnityEvent onBreakPointApplied;

        // Implement IEquatable<T>.Equals(T other) for type-safe, efficient comparison
        public bool Equals(UIResponsiveBreakPoint other)
        {
            return AnchorMinPoints.Equals(other.AnchorMinPoints) &&
                   AnchorMaxPoints.Equals(other.AnchorMaxPoints) &&
                   screenBreakPointData.Equals(other.screenBreakPointData);
        }

        // Override Object.Equals(object obj) to call the type-specific Equals
        public override bool Equals(object obj)
        {
            return obj is UIResponsiveBreakPoint other && Equals(other);
        }

        // Override Object.GetHashCode() so that equal objects have the same hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(AnchorMinPoints, AnchorMaxPoints, screenBreakPointData);
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