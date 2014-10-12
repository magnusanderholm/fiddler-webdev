namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Linq.Expressions;

    public class Property
    {
        public static string Name<T>(Expression<Func<T>> selector)
        {
            return ((MemberExpression)selector.Body).Member.Name;
        }
    }
}
