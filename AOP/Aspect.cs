using System;
using System.Collections.Generic;
using System.Text;

namespace AOP
{
    public abstract class Aspect : Attribute, IAspect
    {
        public readonly AspectContext AspectContext = AspectContext.Instance;
        public virtual object OnAfter(object arg)
        {
            return null;
        }

        public virtual void OnAfterVoid(object arg)
        {
            return;
        }

        public virtual object OnBefore(object[] args)
        {
            return null;
        }

        public virtual void OnBeforeVoid(object[] args)
        {
            return;
        }
    }
}
