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

            if (base.ViewModel != null)
            {

                //// TextView
                //if (nameTextView != null)
                //{
                //    nameTextView.Text = base.ViewModel.Name;

                //    base.ViewModel.PropertyChanged += (sender, e) =>
                //    {
                //        if (e.PropertyName == nameof(base.ViewModel.Name) && nameTextView.Text != base.ViewModel.Name)
                //        {
                //            nameTextView.Text = base.ViewModel.Name;
                //        }
                //    };
                //}

                // TextView
                //if (ageTextView != null)
                //{
                //    ageTextView.Text = base.ViewModel.Age.ToString();
                //    ageTextView.EditorAction += (sender, e) =>
                //    {
                //    };

                //    base.ViewModel.PropertyChanged += (sender, e) =>
                //    {
                //        if (e.PropertyName == nameof(base.ViewModel.Age) &&
                //            ageTextView.Text != base.ViewModel.Age.ToString())
                //        {
                //            ageTextView.Text = base.ViewModel.Age.ToString();
                //        }
                //    };
                //}

                // EditText
                //if (nameEditText != null)
                //{
                //    nameEditText.Text = base.ViewModel.Name;
                //    nameEditText.EditorAction += (sender, e) =>
                //    {
                //        if (base.ViewModel.Name != nameEditText.Text)
                //        {
                //            base.ViewModel.Name = nameEditText.Text;
                //        }
                //    };

                //    nameEditText.TextChanged += (sender, e) =>
                //    {
                //        if (base.ViewModel.Name != nameEditText.Text)
                //        {
                //            base.ViewModel.Name = nameEditText.Text;
                //        }
                //    };

                //    base.ViewModel.PropertyChanged += (sender, e) =>
                //    {
                //        if (e.PropertyName == nameof(base.ViewModel.Name) && nameEditText.Text != base.ViewModel.Name)
                //        {
                //            nameEditText.Text = base.ViewModel.Name;
                //        }
                //    };
                //}

                // EditText
                //if (ageEditText != null)
                //{
                //    ageEditText.Text = base.ViewModel.Age.ToString();
                //    ageEditText.EditorAction += (sender, e) =>
                //    {
                //        if (int.TryParse(ageEditText.Text, out int result))
                //        {
                //            if (base.ViewModel.Age != result)
                //            {
                //                base.ViewModel.Age = result;
                //            }
                //        }
                //    };
                //    ageEditText.TextChanged += (sender, e) =>
                //    {
                //        if (int.TryParse(ageEditText.Text, out int result))
                //        {
                //            if (base.ViewModel.Age != result)
                //            {
                //                base.ViewModel.Age = result;
                //            }
                //        }
                //    };

                //    base.ViewModel.PropertyChanged += (sender, e) =>
                //    {
                //        if (e.PropertyName == nameof(base.ViewModel.Age) &&
                //            ageEditText.Text != base.ViewModel.Age.ToString())
                //        {
                //            ageEditText.Text = base.ViewModel.Age.ToString();
                //        }
                //    };
                //}
            }

        }

        //public void Binding<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>
        //   (int id, TextView control)
        //   where T : TextView
        //{
        //}

        //public void Binding<TSource>(TextView? textView, Expression<Func<TSource, string?>> sourceProperty)
        //    where TSource : RootViewModel
        //{
        //    if (textView != null && base.ViewModel != null)
        //    {
        //        var propertyFunc = sourceProperty.Compile();

        //        textView.Text = propertyFunc?.Invoke((TSource)base.ViewModel);

        //        base.ViewModel.PropertyChanged += (sender, e) =>
        //        {
        //            if (e.PropertyName == nameof(base.ViewModel.Name) && textView.Text != base.ViewModel.Name)
        //            {
        //                textView.Text = base.ViewModel.Name;
        //            }
        //        };
        //    }
        //}

    }
}