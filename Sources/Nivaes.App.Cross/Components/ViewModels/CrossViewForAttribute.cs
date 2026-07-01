namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxViewForAttribute : Attribute
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
        public Type ViewModel { get; set; }

        public MvxViewForAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type viewModel)
        {
            ViewModel = viewModel;
        }
    }
}