using System;

namespace BrightLine.OLAP
{
    public sealed class OLAPContextSingleton
    {
        private static readonly Lazy<OLAPContext> lazy = new Lazy<OLAPContext>(() => new OLAPContext());
        public static OLAPContext Instance { get { return lazy.Value; } }
        private OLAPContextSingleton() { }
    }
}