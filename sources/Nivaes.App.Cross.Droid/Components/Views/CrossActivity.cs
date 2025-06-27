namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;

    [Register("nivaes.app.cross.CrossActivity")]
    public abstract class CrossActivity<TViewModel>
        : Activity, IView, IBindingView
        where TViewModel : class, IViewModel
    {

        protected const int NoContent = 0;

        private readonly int _resourceId;
        private Bundle? _bundle;

        #region Constructors
        protected CrossActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected CrossActivity(IntPtr javaReference, JniHandleOwnership transfer)
           : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            _bundle = Intent?.Extras;

            if (_bundle != null)
            {
                var key = _bundle.GetInt("viewModelKey");
                if (Singleton<TemporaryStore<IViewModel>>.Instance.TryGetAndRemove(key, out var viewModel))
                {
                    ViewModel = (TViewModel?)viewModel;
                }
            }

            base.OnCreate(savedInstanceState);

            if (_resourceId != NoContent)
            {
                try
                {
                    var content = LayoutInflater.Inflate(_resourceId, null);
                    base.SetContentView(content);
                }
                catch (Exception ex)
                {
                    throw new CrossException("Error al generar el recurso", ex);
                }
            }

            Binding();
        }

        //protected override void AttachBaseContext(Context? @base)
        //{
        //    base.AttachBaseContext(new CrossContextWrapper(@base));
        //}

        protected abstract void Binding();


        //public override void SetContentView(int layoutResID)
        //{
        //    if (BaseContextToAttach(this) is MvxContextWrapper)
        //    {
        //        var view = this.BindingInflate(layoutResID, null);
        //        base.SetContentView(view);
        //        return;
        //    }

        //    base.SetContentView(layoutResID);
        //}

        //public override View? OnCreateView(string name, Context context, IAttributeSet attrs)
        //{
        //    var aa = base.OnCreateView(name, context, attrs);
        //    return aa;
        //}

        //public override View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
        //{
        //    var aa = base.OnCreateView(parent, name, context, attrs);
        //    return aa;
        //}
        #endregion

        #region Properties
        private TViewModel? mViewModel;

        public TViewModel? ViewModel
        {
            [System.Diagnostics.DebuggerStepThrough]
            get => mViewModel;
            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                mViewModel = value;
            }
        }
        #endregion

        #region Binding
        public void Binding<TSource>(TextView? textView, Expression<Func<TSource, string?>> sourceProperty)
            where TSource : TViewModel
        {
            if (textView != null && ViewModel != null)
            {
                var propertyFunc = sourceProperty.Compile();

                textView.Text = propertyFunc?.Invoke((TSource)ViewModel);

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    var sourcePropertyName = string.Empty;
                    if (sourceProperty.Body is MemberExpression member)
                        sourcePropertyName = member.Member.Name;

                    if (sourceProperty.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                        sourcePropertyName = memberOperand.Member.Name;

                    var aa = sourceProperty;
                    if (!string.IsNullOrEmpty(sourcePropertyName))
                    {
                        if (e.PropertyName == sourcePropertyName)
                        {
                            var value = propertyFunc?.Invoke((TSource)ViewModel);
                            if (textView.Text != value)
                            {
                                textView.Text = value;
                            }
                        }
                    }
                };
            }
        }

        public void Binding<TSource>(TextView? textView, Expression<Func<TSource, int?>> sourceProperty)
            where TSource : TViewModel
        {
            if (textView != null && ViewModel != null)
            {
                //textView.Text = ViewModel.Age.ToString();
                var propertyFunc = sourceProperty.Compile();

                textView.Text = propertyFunc?.Invoke((TSource)ViewModel).ToString();

                textView.EditorAction += (sender, e) =>
                {
                };

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    //if (e.PropertyName == nameof(ViewModel.Age) && textView.Text != ViewModel.Age.ToString())
                    //{
                    //    textView.Text = ViewModel.Age.ToString();
                    //}
                };
            }
        }

        public void Binding<TSource>(EditText? editText, Expression<Func<TSource, string?>> sourceProperty)
            where TSource : TViewModel
        {
            if (editText != null && ViewModel != null)
            {
                //editText.Text = ViewModel.Name;
                //editText.EditorAction += (sender, e) =>
                //{
                //    if (ViewModel.Name != editText.Text)
                //    {
                //        ViewModel.Name = editText.Text;
                //    }
                //};

                //editText.TextChanged += (sender, e) =>
                //{
                //    if (ViewModel.Name != editText.Text)
                //    {
                //        ViewModel.Name = editText.Text;
                //    }
                //};

                //ViewModel.PropertyChanged += (sender, e) =>
                //{
                //    if (e.PropertyName == nameof(base.ViewModel.Name) && editText.Text != base.ViewModel.Name)
                //    {
                //        editText.Text = base.ViewModel.Name;
                //    }
                //};
            }
        }

        public void Binding<TSource>(EditText? editText, Expression<Func<TSource, int?>> sourceProperty)
          where TSource : TViewModel
        {
            if (editText != null && ViewModel != null)
            {
                //editText.Text = ViewModel.Age.ToString();
                //editText.EditorAction += (sender, e) =>
                //{
                //    if (int.TryParse(editText.Text, out int result))
                //    {
                //        if (ViewModel.Age != result)
                //        {
                //            ViewModel.Age = result;
                //        }
                //    }
                //};
                //editText.TextChanged += (sender, e) =>
                //{
                //    if (int.TryParse(editText.Text, out int result))
                //    {
                //        if (ViewModel.Age != result)
                //        {
                //            ViewModel.Age = result;
                //        }
                //    }
                //};

                //ViewModel.PropertyChanged += (sender, e) =>
                //{
                //    if (e.PropertyName == nameof(base.ViewModel.Age) &&
                //        editText.Text != base.ViewModel.Age.ToString())
                //    {
                //        editText.Text = base.ViewModel.Age.ToString();
                //    }
                //};
            }
        }

        public void Binding(Button? button, ICommand? command)
        {
            if (button != null)
            {
                button.Enabled = command?.CanExecute(null) ?? true;
                button.Click += (ssend, e) =>
                {
                    if (command?.CanExecute(null) ?? false)
                    {
                        command?.Execute(null);
                    }
                };

                if (command != null)
                {
                    command.CanExecuteChanged += (sender, e) =>
                    {
                        button.Enabled = command?.CanExecute(null) ?? true;
                    };
                }
            }
        }
        #endregion
    }
}
