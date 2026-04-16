using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Digx7.Zygote
{
    [System.Serializable]
    public struct GridAxisMetadata : IEquatable<GridAxisMetadata>
    {
        #region Variables ============================
        public bool xAxisOnTop;
        public bool xAxisOnBottom;
        public bool yAxisOnLeft;
        public bool yAxisOnRight;
        public bool originInMiddle;
        #endregion

        public void SetAllToFalse()
        {
            xAxisOnTop = false;
            xAxisOnBottom = false;
            yAxisOnLeft = false;
            yAxisOnRight = false;
            originInMiddle = false;
        }

        #region Equality Methods ============================

        // Implement IEquatable<T>.Equals(T other) for type-safe, efficient comparison
        public bool Equals(GridAxisMetadata other)
        {
            return xAxisOnTop == other.xAxisOnTop && 
                    xAxisOnBottom == other.xAxisOnBottom && 
                    yAxisOnLeft == other.yAxisOnLeft && 
                    yAxisOnRight == other.yAxisOnRight && 
                    originInMiddle == other.originInMiddle;
        }

        // Override Object.Equals(object obj) to call the type-specific Equals
        public override bool Equals(object obj)
        {
            return obj is GridAxisMetadata other && Equals(other);
        }

        // Override Object.GetHashCode() so that equal objects have the same hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(xAxisOnTop, xAxisOnBottom, yAxisOnLeft, yAxisOnRight, originInMiddle);
        }

        // Overload the == and != operators for intuitive syntax
        public static bool operator ==(GridAxisMetadata left, GridAxisMetadata right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridAxisMetadata left, GridAxisMetadata right)
        {
            return !(left == right);
        }

        #endregion
    }
}