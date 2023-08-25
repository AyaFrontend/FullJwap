using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public interface ISpecfication<T> 
    {
        public Expression<Func<T, bool>> Cretiria { get; }
        public List<Expression<Func<T, object>>> Include { get; }
        public Expression<Func<T, object>> OrderBy { get; }
    }
}
