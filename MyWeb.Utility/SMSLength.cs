using System;
using System.Text.RegularExpressions;

namespace MyWeb.Utility
{
    public class SmsLength
    {
        private static SmsLength _instance;
        private static readonly object SyncRoot = new Object();

        public static SmsLength Instance 
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new SmsLength();
                    }
                }

                return _instance;
            }
        }

        public int CalculateSmsLength(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {

                if (IsEnglishText(text))
                {
                    return text.Length <= 160 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 153));
                }

                return text.Length <= 70 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));

            }

            return 1;
        }

        public bool IsEnglishText(string text)
        {
            return string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"^[\u0000-\u007F]+$");
        }
    }
}
