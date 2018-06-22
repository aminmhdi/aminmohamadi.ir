using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Utility
{
    public class AllowUploadSpecialFilesOnly
    {
        public class AllowUploadSpecialFilesOnlyAttribute : Attribute
        {
            public AllowUploadSpecialFilesOnlyAttribute(string jpgPngGif, bool b)
            {
                throw new NotImplementedException();
            }
        }
    }
}
