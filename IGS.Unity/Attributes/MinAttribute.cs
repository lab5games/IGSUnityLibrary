using System;
using UnityEngine;

namespace IGS.Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinAttribute : PropertyAttribute
    {
        public readonly float min;

        public MinAttribute(float min)
        {
            this.min = min;
        }
    }
}
