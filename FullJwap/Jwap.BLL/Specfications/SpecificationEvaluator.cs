using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public  class SpecificationEvaluator<T> where T: class
    {
       public static IQueryable<T> GetQuery(IQueryable<T> query , ISpecfication<T> spec)
       {
            var Query = query;

            if (spec.Cretiria != null)
                Query = Query.Where(spec.Cretiria);

            if (spec.OrderBy != null)
                Query = Query.OrderBy(spec.OrderBy);

            if (spec.Include.Count != 0)
                Query = spec.Include.Aggregate(Query, (currentQuery, include) => currentQuery.Include(include));
           
            return Query;
       }
    }
}
