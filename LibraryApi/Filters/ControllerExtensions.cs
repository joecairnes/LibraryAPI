using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Filters
{
    public static class ControllerExtensions
    {
        public static ActionResult Maybe<T>(this ControllerBase c, T response)
        {
            if (response == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(response);
            }
        }
    }
}
