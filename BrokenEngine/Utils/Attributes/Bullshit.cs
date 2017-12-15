using System;

namespace BrokenEngine.Utils.Attributes
{
    /// <summary>
    /// Marks a method as bullshit
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited =  false)]
    public class Bullshit : Attribute
    {
        public string Reason;
    }
}