using System;

namespace BrokenEngine.Utils.Attributes
{
    /// <summary>
    /// Marks a method as bullshit
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =  false)]
    public class Bullshit : System.Attribute
    {
        public string Reason;
    }
}