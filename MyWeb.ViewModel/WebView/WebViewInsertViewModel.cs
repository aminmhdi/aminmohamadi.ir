using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.WebView
{
  public class WebViewInsertViewModel
  {
    public WebViewInsertViewModel()
    {
      CreatedOn = DateTime.Now;
    }

    public DateTime CreatedOn { get; set; }
    public string Ip { get; set; }
    public string Session { get; set; }
    public string BrowserName { get; set; }
    public string BrowserVersion { get; set; }
    public string Platfrom { get; set; }
    public string Device { get; set; }
    public string MoreInfo { get; set; }
    public long? UserId { get; set; }
  }
}