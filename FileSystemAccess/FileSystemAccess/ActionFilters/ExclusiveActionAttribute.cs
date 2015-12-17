using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FileSystemAccess.ActionFilters
{
    public class ExclusiveActionAttribute : ActionFilterAttribute
    {
        private static int _isExecuting = 0;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Interlocked.CompareExchange(ref _isExecuting, 1, 0) == 0)
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            filterContext.Result =
                new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            Interlocked.Exchange(ref _isExecuting, 0);
        }
    }
}
