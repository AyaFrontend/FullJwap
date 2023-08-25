using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public class BaseSpecfication<T> : ISpecfication<T> where T : class
    {
        public Expression<Func<T, bool>> Cretiria { get; }

        public List<Expression<Func<T, object>>> Include { set; get; } =
            new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { set; get; }
        public BaseSpecfication(Expression<Func<T, bool>> cretiria )
        {
            this.Cretiria = cretiria;
        }

        public void ApplyInclude (Expression<Func<T, object>> include)
        {
            Include.Add(include);
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }
    }
}
