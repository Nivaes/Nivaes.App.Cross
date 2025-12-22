namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Linq;

    public class CrossCompositeBinding : CrossBinding
    {
        private readonly List<ICrossBinding> _bindings;

        public CrossCompositeBinding(params ICrossBinding[] args)
        {
            _bindings = args.ToList();
        }

        public void Add(params ICrossBinding[] args)
        {
            _bindings.AddRange(args);
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var mvxBinding in _bindings)
                {
                    mvxBinding.Dispose();
                }
                _bindings.Clear();
            }
            base.Dispose(isDisposing);
        }
    }
}
