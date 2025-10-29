namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;

    [Register("nivaes.app.cross.CrossActivity")]
    public abstract class CrossActivity<TViewModel>
        : Activity, ICrossView, ICrossBindingView
        where TViewModel : class, ICrossViewModel
    {

        protected const int NoContent = 0;

        private readonly int _resourceId;
        private Android.OS.Bundle? _bundle;

        #region Constructors
        protected CrossActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected CrossActivity(IntPtr javaReference, JniHandleOwnership transfer)
           : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Android.OS.Bundle? savedInstanceState)
        {
            _bundle = Intent?.Extras;

            if (_bundle != null)
            {
                var key = _bundle.GetInt("viewModelKey");
                if (Singleton<TemporaryStore<ICrossViewModel>>.Instance.TryGetAndRemove(key, out var viewModel))
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
        private string GetSourcePropertyName(LambdaExpression lambdaExpression)
        {
            var sourcePropertyName = string.Empty;
            if (lambdaExpression.Body is MemberExpression member)
                sourcePropertyName = member.Member.Name;

            if (lambdaExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                sourcePropertyName = memberOperand.Member.Name;

            return sourcePropertyName;
        }

        public void Binding<TSource>(TextView? textView, Expression<Func<TSource, string?>> sourceProperty)
            where TSource : TViewModel
        {
            if (textView != null && ViewModel != null)
            {
                var propertyFunc = sourceProperty.Compile();
                textView.Text = propertyFunc?.Invoke((TSource)ViewModel);

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    var sourcePropertyName = GetSourcePropertyName(sourceProperty);

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
                var propertyFunc = sourceProperty.Compile();
                textView.Text = propertyFunc?.Invoke((TSource)ViewModel).ToString();

                textView.EditorAction += (sender, e) =>
                {
                };

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    var sourcePropertyName = GetSourcePropertyName(sourceProperty);

                    if (!string.IsNullOrEmpty(sourcePropertyName))
                    {
                        if (e.PropertyName == sourcePropertyName)
                        {
                            var value = propertyFunc?.Invoke((TSource)ViewModel).ToString();
                            if (textView.Text != value)
                            {
                                textView.Text = value;
                            }
                        }
                    }
                };
            }
        }

        public void Binding<TSource>(EditText? editText, Expression<Func<TSource, string?>> sourceProperty)
            where TSource : TViewModel
        {
            if (editText != null && ViewModel != null)
            {
                var propertyFunc = sourceProperty.Compile();
                editText.Text = propertyFunc?.Invoke((TSource)ViewModel);

                editText.EditorAction += (sender, e) =>
                {
                    if (sourceProperty.Body is not MemberExpression member)
                    {
                        if (sourceProperty.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                            member = memberOperand;
                        else
                            return;
                    }

                    var parameter = Expression.Parameter(typeof(string), "value");
                    var assign = Expression.Assign(member, parameter);
                    var lambda = Expression.Lambda<Action<TSource, string?>>(assign, sourceProperty.Parameters[0], parameter);
                    var action = lambda.Compile();
                    action((TSource)ViewModel, editText.Text);
                };

                editText.TextChanged += (sender, e) =>
                {
                    if (sourceProperty.Body is not MemberExpression member)
                    {
                        if (sourceProperty.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                            member = memberOperand;
                        else
                            return;
                    }

                    var parameter = Expression.Parameter(typeof(string), "value");
                    var assign = Expression.Assign(member, parameter);
                    var lambda = Expression.Lambda<Action<TSource, string?>>(assign, sourceProperty.Parameters[0], parameter);
                    var action = lambda.Compile();
                    action((TSource)ViewModel, editText.Text);
                };

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    var sourcePropertyName = GetSourcePropertyName(sourceProperty);

                    if (!string.IsNullOrEmpty(sourcePropertyName))
                    {
                        if (e.PropertyName == sourcePropertyName)
                        {
                            var value = propertyFunc?.Invoke((TSource)ViewModel)?.ToString();
                            if (editText.Text != value)
                            {
                                editText.Text = value;
                            }
                        }
                    }
                };
            }
        }

        public void Binding<TSource>(EditText? editText, Expression<Func<TSource, int?>> sourceProperty)
          where TSource : TViewModel
        {
            if (editText != null && ViewModel != null)
            {
                var propertyFunc = sourceProperty.Compile();
                editText.Text = propertyFunc?.Invoke((TSource)ViewModel).ToString();

                editText.EditorAction += (sender, e) =>
                {
                    if (sourceProperty.Body is not MemberExpression member)
                    {
                        if (sourceProperty.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                            member = memberOperand;
                        else
                            return;
                    }

                    var parameter = Expression.Parameter(typeof(int?), "value");
                    var assign = Expression.Assign(member, parameter);
                    var lambda = Expression.Lambda<Action<TSource, int?>>(assign, sourceProperty.Parameters[0], parameter);
                    var action = lambda.Compile();
                    if (int.TryParse(editText.Text, out int result))
                    {
                        action((TSource)ViewModel, result);
                    }
                };

                editText.TextChanged += (sender, e) =>
                {
                    if (sourceProperty.Body is not MemberExpression member)
                    {
                        if (sourceProperty.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                            member = memberOperand;
                        else
                            return;
                    }

                    var parameter = Expression.Parameter(typeof(int?), "value");
                    var assign = Expression.Assign(member, parameter);
                    var lambda = Expression.Lambda<Action<TSource, int?>>(assign, sourceProperty.Parameters[0], parameter);
                    var action = lambda.Compile();
                    if (int.TryParse(editText.Text, out int result))
                    {
                        action((TSource)ViewModel, result);
                    }
                };

                ViewModel.PropertyChanged += (sender, e) =>
                {
                    var sourcePropertyName = GetSourcePropertyName(sourceProperty);

                    if (!string.IsNullOrEmpty(sourcePropertyName))
                    {
                        if (e.PropertyName == sourcePropertyName)
                        {
                            var value = propertyFunc?.Invoke((TSource)ViewModel)?.ToString();
                            if (editText.Text != value)
                            {
                                editText.Text = value;
                            }
                        }
                    }
                };
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
