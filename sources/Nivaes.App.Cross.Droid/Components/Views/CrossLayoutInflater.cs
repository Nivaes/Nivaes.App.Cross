namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;

    [Register("nivaes.app.cross.CrossLayoutInflater")]
    internal class CrossLayoutInflater 
        : LayoutInflater
    {
        //private readonly MvxBindingVisitor _bindingVisitor;

        public CrossLayoutInflater(Context? context)
            : base(context)
        {
        }

        public CrossLayoutInflater(LayoutInflater? original, Context? newContext, /*MvxBindingVisitor? bindingVisitor,*/ bool cloned = false)
            : base(original, newContext)
        {
        }

        // ToDo: el atributo DynamicDependencyAttribute a de indicarse sobre el metodo que llama al metodo que no ha de ser desartado.
        [DynamicDependencyAttribute(".ctor", typeof(CrossLayoutInflater))]
        public CrossLayoutInflater(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override LayoutInflater CloneInContext(Context? newContext)
        {
            return new CrossLayoutInflater(this, newContext, /*_bindingVisitor,*/ true);
        }
    }
}
