using System;
using UnityEngine;

namespace IGS.Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
