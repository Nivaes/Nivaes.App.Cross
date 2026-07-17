using System.Linq.Expressions;
//using Android.Support.Design.Widget;
using Google.Android.Material.TextField;
using Nivaes.App.Cross;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Droid
{
    public static class FluentValidation
    {
        public static void Validate(this TextInputLayout textInputLayout, IBaseViewModel dataContext, string propertyName)
        {
            if (textInputLayout == null) throw new ArgumentNullException(nameof(textInputLayout));

            _ = new TextInputLayoutValidate(textInputLayout, dataContext, propertyName);
        }

        public static void Validate<TDataModel>(this TextInputLayout textInputLayout, TDataModel dataContext, Expression<Func<TDataModel, object>> targetPropertyPath)
            where TDataModel : IBaseViewModel
        {
            if (textInputLayout == null) throw new ArgumentNullException(nameof(textInputLayout));

            var propertyName = SourcePropertyPath(targetPropertyPath);
            _ = new TextInputLayoutValidate(textInputLayout, dataContext, propertyName);
        }

        public static void Validate(this EditText editText, IBaseViewModel dataContext, string propertyName)
        {
            if (editText == null) throw new ArgumentNullException(nameof(editText));

            _ = new EditTextValidate(editText, dataContext, propertyName);
        }

        public static void Validate<TDataModel>(this EditText editText, TDataModel dataContext, Expression<Func<TDataModel, object>> targetPropertyPath)
            where TDataModel : IBaseViewModel
        {
            if (editText == null) throw new ArgumentNullException(nameof(editText));

            var propertyName = SourcePropertyPath(targetPropertyPath);
            _ = new EditTextValidate(editText, dataContext, propertyName);
        }

        private static string SourcePropertyPath<TSource>(Expression<Func<TSource, object>> sourceProperty)
             where TSource : IBaseViewModel
        {
            var parser = Singleton<CrossBindingSingletonCache>.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();
            return sourcePropertyPath;
        }
    }
}
