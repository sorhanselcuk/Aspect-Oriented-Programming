using System;
using System.Collections.Generic;
using System.Text;

namespace AOP
{
    public class AspectContext
    {
        private readonly static Lazy<AspectContext> _Instance = new Lazy<AspectContext>(() => new AspectContext());

        private AspectContext()
        {
        }

        public static AspectContext Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        public string MethodName { get; set; }
        public object[] Arguments { get; set; }
    }
}
