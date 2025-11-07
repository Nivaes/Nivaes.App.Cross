namespace Nivaes.App.Cross
{
    using System;
    using System.Linq.Expressions;

    public interface ICrossPropertyExpressionParser
    {
        ICrossParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath);

        ICrossParsedExpression Parse(LambdaExpression propertyPath);
    }
}
