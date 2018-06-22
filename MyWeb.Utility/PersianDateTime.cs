using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Utility
{
  public static class PersianDateExtensionMethods
  {
    private static CultureInfo _culture;
    public static CultureInfo GetPersianCulture()
    {
      if (_culture != null)
        return _culture;
      _culture = new CultureInfo("fa-IR");
      var formatInfo = _culture.DateTimeFormat;
      formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
      formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
      var monthNames = new[]
      {
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
        "اسفند",
        ""
      };
      formatInfo.AbbreviatedMonthNames =
        formatInfo.MonthNames =
          formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
      formatInfo.AMDesignator = "ق.ظ";
      formatInfo.PMDesignator = "ب.ظ";
      formatInfo.ShortDatePattern = "yyyy/MM/dd";
      formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
      formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
      Calendar cal = new PersianCalendar();

      var fieldInfo = _culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
      if (fieldInfo != null)
        fieldInfo.SetValue(_culture, cal);

      var info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
      if (info != null)
        info.SetValue(formatInfo, cal);

      _culture.NumberFormat.NumberDecimalSeparator = "/";
      _culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
      _culture.NumberFormat.NumberNegativePattern = 0;
      return _culture;
    }

    public static string ToPersianString(this DateTime date, string format = "yyyy/MM/dd")
    {
      return date.ToString(format, GetPersianCulture());
    }
  }
}
