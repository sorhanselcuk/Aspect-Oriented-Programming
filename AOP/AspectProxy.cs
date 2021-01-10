using ImpromptuInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace AOP
{
    public class AspectProxy<T> : DynamicObject where T : class, new()
    {
        private readonly T Subject;
        private AspectProxy(T context)
        {
            Subject = context;
        }
        public static IT As<IT>() where IT : class
        {
            if (!typeof(IT).IsInterface)
                throw new ArgumentException("IT must be an abstract type!");

            return new AspectProxy<T>(new T()).ActLike<IT>();

        }
        public static IT As<IT>(object[] args) where IT : class
        {
            if (!typeof(IT).IsInterface)
                throw new ArgumentException("IT must be an abstract type!");
            try
            {
                var instance = (T)Activator.CreateInstance(typeof(T), args);
                return new AspectProxy<T>(instance).ActLike<IT>();
            }
            catch (Exception)
            {

                throw new ArgumentException("Arguments not match!");
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                var methodInfo = Subject.GetType().GetMethod(binder.Name);
                var aspects = methodInfo.GetCustomAttributes(typeof(Aspect), true);

                FillAspectContext(methodInfo, args);

                var response = CheckBeforeAspect(aspects, args);

                if (response != null)
                {
                    result = response;
                    return true;
                }

                result = methodInfo.Invoke(Subject, args);

                CheckAfterAspect(result, aspects);
                return true;

            }
            catch (Exception exception)
            {
                result = exception.Message;
                return false;
            }
        }

        private void CheckAfterAspect(object result, object[] aspects)
        {
            foreach (IAspect aspect in aspects)
            {
                var type = aspect.GetType();
                var onAfterVoid = type.GetMethod("OnAfterVoid");
                var onAfter = type.GetMethod("OnAfter");

                if (onAfterVoid.DeclaringType.Name != "Aspect")
                {
                    aspect.OnAfterVoid(result);
                }
                if (onAfter.DeclaringType.Name != "Aspect")
                {
                    aspect.OnAfter(result);

                }
            }
        }

        private object CheckBeforeAspect(object[] aspects, object[] args)
        {
            object result = null;

            foreach (IAspect aspect in aspects)
            {
                var type = aspect.GetType();
                var onBeforeVoid = type.GetMethod("OnBeforeVoid");
                var onbefore = type.GetMethod("OnBefore");


                if (onbefore.DeclaringType.Name != "Aspect")
                {
                    result = aspect.OnBefore(args);
                }
                if (onBeforeVoid.DeclaringType.Name != "Aspect")
                {
                    aspect.OnBeforeVoid(args);
                }
            }
            return result;
        }

        private void FillAspectContext(MethodInfo methodInfo, object[] args)
        {
            AspectContext.Instance.MethodName = methodInfo.Name;
            AspectContext.Instance.Arguments = args;
        }
    }
}
