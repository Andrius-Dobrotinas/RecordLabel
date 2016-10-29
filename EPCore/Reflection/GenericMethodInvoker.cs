using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AndrewD.EntityPlus.Reflection
{
    // TODO: right now, it's tailored to specific cases. It would be a good idea to make this more "generic" and flexible
    public class GenericMethodInvoker
    {
        public static BindingFlags DefaultPublicInstanceBindingFlags => BindingFlags.Public | BindingFlags.Instance;

        public static object InvokeGenericMethod(Type classType, Type[] classGenericTypeArguments, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            // TODO: I could probably implement a cache so as not to invoke this every time

            var genericType = classType.MakeGenericType(classGenericTypeArguments);

            return InvokeGenericMethod(genericType, methodName, methodGenericTypeArguments, methodBindingFlags, methodArguments, targetObject);
        }

        public static object InvokeGenericMethod(Type classType, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            var method = classType.GetMethod(methodName, methodBindingFlags);
            if (methodGenericTypeArguments?.Length > 0)
                method = method.MakeGenericMethod(methodGenericTypeArguments);
            return method.Invoke(targetObject, methodArguments);
        }
    }
}
