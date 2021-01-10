using System;
using System.Collections.Generic;
using System.Text;

namespace AOP
{
    interface IAspect
    {
        void OnAfterVoid(object arg);

        object OnAfter(object arg);

        void OnBeforeVoid(object[] args);

        object OnBefore(object[] args);
    }
}
