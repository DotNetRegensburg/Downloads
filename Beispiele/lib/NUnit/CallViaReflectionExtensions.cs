using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnitTest.Helpers
{
    public static class CallViaReflectionExtensions
    {
        public static void CallPrivateSub(this object obj, string methodName, params object[] parameters)
        {
            CallPrivateFunction(obj, methodName, parameters);
        }

        public static TReturn CallPrivateFunction<TReturn>(this object obj, string methodName, params object[] parameters)
        {
            return (TReturn)CallPrivateFunction(obj, methodName, parameters);
        }

        public static object CallPrivateFunction(this object obj, string methodName, params object[] parameters)
        {
            List<Type> parameterTypes = new List<Type>();
            foreach (object p in parameters)
                parameterTypes.Add(p.GetType());

            MethodInfo mi = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes.ToArray(), null);

            return mi.Invoke(obj, parameters);
        }
    }
}
