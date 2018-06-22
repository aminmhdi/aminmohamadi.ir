using System;
using System.Globalization;
using System.Web.Mvc;

namespace MyWeb.Utility
{
  public class PersianDateModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
      var modelState = new ModelState { Value = valueResult };
      object actualValue = new DateTime(1900, 1, 1); //todo: توصيه شده تاريخ تولد خودتان را در اينجا قرار دهيد
      try
      {
        var englishNumbers = valueResult.AttemptedValue.GetEnglishNumber();

        var dateparts = englishNumbers.Split('/'); //ex. 1391/1/19

        if (dateparts.Length != 3) return actualValue;

        var year = int.Parse(dateparts[0]);
        var month = int.Parse(dateparts[1]);

        var timepart = dateparts[2].Split(' ');

        var day = int.Parse(timepart[0]);

        if (timepart.Length != 2)
        {
          actualValue = new DateTime(year, month, day, 0, 0, 0, new PersianCalendar());
        }

        else
        {
          var timeparts = timepart[1].Split(':');

          if (timeparts.Length != 3) return actualValue;

          var hour = int.Parse(timeparts[0]);
          var min = int.Parse(timeparts[1]);
          var sec = int.Parse(timeparts[2]);

          actualValue = new DateTime(year, month, day, hour, min, sec, new PersianCalendar());
        }


      }
      catch (FormatException)
      {
        modelState.Errors.Add("تاریخ را به شکل صحیح [ به عنوان مثال 1371/9/28] وارد کنید");
      }

      bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
      return actualValue;
    }
  }
}
