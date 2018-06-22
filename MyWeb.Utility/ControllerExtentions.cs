using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWeb.Utility
{
    public static class ControllerExtentions
    {
        #region GetUserManagerErros

        public static string GetUserManagerErros(this IEnumerable<string> errors)
        {
            return errors.Aggregate(string.Empty, (current, error) => current + string.Format("{0} \n", error));
        }

        #endregion

        #region GetListOfErrors
        public static string GetListOfErrors(this ModelStateDictionary modelState)
        {
            var list = modelState.ToList();
            return
                list.Select(keyValuePair => keyValuePair.Value.Errors.Select(a => a.ErrorMessage))
                    .Aggregate(string.Empty,
                        (current1, errors) =>
                            errors.Aggregate(current1, (current, error) => current + string.Format("{0}\n", error)));
        }
        #endregion


    }
}
