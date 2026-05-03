using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewScreenBreakPointData", menuName = "ScriptableObjects/Data/ScreenBreakPointData", order = 1)]
    public class ScreenBreakPointData: ScriptableObject
    {
        
        public bool checkScreenHeight;
        public float minScreenHeight;
        public float maxScreenHeight;

        public bool checkScreenWidth;
        public float minScreenWidth;
        public float maxScreenWidth;

        public bool mobileOnly;
        public bool portraitOnly;
        public bool landscapeOnly;

        public bool IsWithinBreakPoint(ScreenInfo screenInfo) 
        {
            if (mobileOnly && !screenInfo.isMobile)
            {
                return false;
            }

            if (portraitOnly && !screenInfo.IsPortrait)
            {
                return false;
            }

            if (landscapeOnly && !screenInfo.IsLandscape)
            {
                return false;
            }

            if (checkScreenWidth && (screenInfo.width < minScreenWidth || screenInfo.width > maxScreenWidth))
            {
                return false;
            }

            if (checkScreenHeight && (screenInfo.height < minScreenHeight || screenInfo.height > maxScreenHeight))
            {
                return false;
            }

            return true;
        }
    }
}