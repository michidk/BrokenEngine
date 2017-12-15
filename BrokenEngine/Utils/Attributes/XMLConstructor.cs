using System;

namespace BrokenEngine.Utils.Attributes
{
    /// <summary>
    /// Marks a constructor as just necessary for being able to serialize its class
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class XmlConstructor : Attribute
    {
    }
}