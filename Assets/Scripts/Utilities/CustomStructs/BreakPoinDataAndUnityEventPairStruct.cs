using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Digx7.Zygote
{
    [System.Serializable]
    public struct BreakPoinDataAndUnityEventPair : IEquatable<BreakPoinDataAndUnityEventPair>
    {
        #region Variables ============================
        public ScreenBreakPointData screenBreakPointData;
        public UnityEvent onBreakPointApplied;
        #endregion

        #region Equality Methods ============================

        // Implement IEquatable<T>.Equals(T other) for type-safe, efficient comparison
        public bool Equals(BreakPoinDataAndUnityEventPair other)
        {
             return screenBreakPointData == other.screenBreakPointData && onBreakPointApplied == other.onBreakPointApplied;
        }

        // Override Object.Equals(object obj) to call the type-specific Equals
        public override bool Equals(object obj)
        {
            return obj is BreakPoinDataAndUnityEventPair other && Equals(other);
        }

        // Override Object.GetHashCode() so that equal objects have the same hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(screenBreakPointData, onBreakPointApplied);
        }

        // Overload the == and != operators for intuitive syntax
        public static bool operator ==(BreakPoinDataAndUnityEventPair left, BreakPoinDataAndUnityEventPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BreakPoinDataAndUnityEventPair left, BreakPoinDataAndUnityEventPair right)
        {
            return !(left == right);
        }

        #endregion
    }
}