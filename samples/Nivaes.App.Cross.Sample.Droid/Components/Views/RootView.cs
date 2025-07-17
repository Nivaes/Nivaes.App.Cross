namespace Nivaes.App.Cross.Sample.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Windows.Input;
    using Android.Widget;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.Sample;
    using static Android.Renderscripts.ScriptGroup;

    [Activity(Label = "@string/app_name")]
    public partial class RootView
        : CrossActivity<RootViewModel>,
        IBindingView
    {
        public RootView()
            : base(Resource.Layout.RootView)
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void Binding()
        {
            var nameTextView = base.FindViewById<TextView>(Resource.Id.nameTextView);
            Binding<RootViewModel>(nameTextView, vm => vm.Name);

            var ageTextView = base.FindViewById<TextView>(Resource.Id.ageTextView);
            Binding<RootViewModel>(ageTextView, vm => vm.Age);

            var nameEditText = base.FindViewById<EditText>(Resource.Id.nameEditText);
            Binding<RootViewModel>(nameEditText, vm => vm.Name);

            var ageEditText = base.FindViewById<EditText>(Resource.Id.ageEditText);
            Binding<RootViewModel>(ageEditText, vm => vm.Age);

            var button1 = base.FindViewById<Button>(Resource.Id.button1);
            Binding(button1, base.ViewModel?.Command);
        }
    }
}