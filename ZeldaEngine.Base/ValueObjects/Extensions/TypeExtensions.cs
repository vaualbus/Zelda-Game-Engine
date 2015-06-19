using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ZeldaEngine.Base.ValueObjects.Extensions
{
    public static class TypeExtensions
    {
        public static bool MatchTypes(this MethodInfo methodInfo, object[] @params)
        {
            var paramsTypes = @params.Select(t => t.GetType()).ToArray();
            var providedMethodTypes = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();

            Debug.Assert(paramsTypes.Length == providedMethodTypes.Length);

            var results = new List<bool>();
            for (var i = 0; i < @params.Length; i++)
            {
                if (paramsTypes[i] == providedMethodTypes[i])
                    results.Add(true);
            }

            return results.Count == paramsTypes.Length;
        }
    }
}