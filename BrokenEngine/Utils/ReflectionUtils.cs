using System.Reflection;
using System.Text;

namespace BrokenEngine.Utils
{
    public class ReflectionUtils
    {

        public static string ListFields(object instance)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Configuration:");
            foreach (FieldInfo field in instance.GetType().GetFields())
            {
                builder.Append(field.Name);
                builder.Append(": ");
                builder.AppendLine(field.GetValue(instance).ToString());
            }
            return builder.ToString();
        }
         
    }
}