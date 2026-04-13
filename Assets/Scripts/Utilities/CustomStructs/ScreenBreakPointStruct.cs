using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Digx7.Zygote
{
    [System.Serializable]
    public struct ScreenBreakPoint : IEquatable<ScreenBreakPoint>
    {
        #region Variables ============================
        public string breakPointName;
        
        [Header("Screen Width")]
        public float minScreenWidth;
        public float maxScreenWidth;

        [Header("Events")]
        public UnityEvent onBreakPointApplied;
        #endregion

        #region Equality Methods ============================

        // Implement IEquatable<T>.Equals(T other) for type-safe, efficient comparison
        public bool Equals(ScreenBreakPoint other)
        {
            return breakPointName == other.breakPointName &&
                   minScreenWidth == other.minScreenWidth &&
                   maxScreenWidth == other.maxScreenWidth;
        }

        // Override Object.Equals(object obj) to call the type-specific Equals
        public override bool Equals(object obj)
        {
            return obj is ScreenBreakPoint other && Equals(other);
        }

        // Override Object.GetHashCode() so that equal objects have the same hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(breakPointName, minScreenWidth, maxScreenWidth);
        }

        // Overload the == and != operators for intuitive syntax
        public static bool operator ==(ScreenBreakPoint left, ScreenBreakPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScreenBreakPoint left, ScreenBreakPoint right)
        {
            return !(left == right);
        }

        #endregion
    }
}