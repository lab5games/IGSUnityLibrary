using System;
using UnityEngine;

namespace IGS.Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxAttribute : PropertyAttribute
    {
        public readonly float max;

        public MaxAttribute(float max)
        {
            this.max = max;
        }
    }
}
