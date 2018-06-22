using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyWeb.DataLayer.Context;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.Dashboard;
using MyWeb.ViewModel.Post;
using Newtonsoft.Json;
using ActiveStatus = MyWeb.ViewModel.Comment.ActiveStatus;

namespace MyWeb.ServiceLayer.EFServices.Dashboard
{
    public class DashboardService : IDashboardService
    {
        #region Fields
        private readonly IApplicationUserManager _userService;
        private readonly ICommentService _commentService;
        private readonly IPostsReactService _postReactService;
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IWebViewService _webViewService;
        private readonly IWebPostViewService _webPostViewService;
        private readonly IDbSet<DomainClasses.Entities.WebView> _webView;

        #endregion

        #region Ctor

        public DashboardService(
          IApplicationUserManager userService,
          ICommentService commentService,
          IPostsReactService postReactService,
          IPostService postService,
          IWebViewService webViewService,
          IWebPostViewService webPostViewService,
          ICategoryService categoryService,
          IUnitOfWork unitOfWork
          )
        {
            _userService = userService;
            _commentService = commentService;
            _postReactService = postReactService;
            _postService = postService;
            _categoryService = categoryService;
            _webViewService = webViewService;
            _webPostViewService = webPostViewService;
            _webView = unitOfWork.Set<DomainClasses.Entities.WebView>();
        }
        #endregion

        #region CountOfWebsiteViewForToDayInHours

        public async Task<string> CountOfWebsiteViewForToDayInHours()
        {
            var websiteViewIn24HoursAgoList = new List<WebsiteViewIn24HoursAgo>();
            var dateTimeNow = DateTime.Now;
            var dateTime24HoursAgo = dateTimeNow.AddHours(-23);
            var dateTime24HoursAgoClockClear = new DateTime(dateTime24HoursAgo.Year, dateTime24HoursAgo.Month, dateTime24HoursAgo.Day, dateTime24HoursAgo.Hour, 0, 0);
            var dateTimeNowClockClear = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, 0, 0).AddHours(1);

            var websiteViews = await
              _webView
                .AsNoTracking()
                .Where(q => q.CreatedOn <= dateTimeNowClockClear & q.CreatedOn >= dateTime24HoursAgoClockClear)
                .AsQueryable()
                .ToListAsync();

            //var selectedMessages = await outcomingMessages.ProjectTo<OutgoingMessageDetailViewModel>(_mappingEngine).ToListAsync();

            var next = new DateTime(dateTime24HoursAgoClockClear.Year, dateTime24HoursAgoClockClear.Month, dateTime24HoursAgoClockClear.Day,
              dateTime24HoursAgoClockClear.Hour, 0, 0);

            while (next < dateTimeNowClockClear)
            {
                var nexthour = next.AddHours(1);
                var hourlyWebsiteView = websiteViews.Where(q => q.CreatedOn <= nexthour & q.CreatedOn >= next).ToList();

                websiteViewIn24HoursAgoList.Add(new WebsiteViewIn24HoursAgo
                {
                    Hour = next.Hour + ":00",
                    Count = hourlyWebsiteView.Count
                });

                next = next.AddHours(1);
            }

            return JsonConvert.SerializeObject(websiteViewIn24HoursAgoList, Formatting.None);
        }

        #endregion

        #region CountOfWebsiteViewIn30DaysAgo

        public async Task<string> CountOfWebsiteViewIn30DaysAgo()
        {
            var websiteViewIn30DaysAgoList = new List<WebsiteViewIn30DaysAgo>();
            var dateTimeNow = DateTime.Now;
            //var cultureInfo = new CultureInfo("fa-IR");
            var dateTime30DaysAgo = dateTimeNow.AddDays(-30);
            var dateTime30DaysAgoClockClear = new DateTime(dateTime30DaysAgo.Year, dateTime30DaysAgo.Month, dateTime30DaysAgo.Day, 0, 0, 0);
            var dateTimeNowClockClear = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0).AddDays(1);

            var websiteViews = await
              _webView
                .AsNoTracking()
                .Where(q => q.CreatedOn <= dateTimeNowClockClear & q.CreatedOn >= dateTime30DaysAgoClockClear)
                .AsQueryable()
                .ToListAsync();

            var next = new DateTime(dateTime30DaysAgoClockClear.Year, dateTime30DaysAgoClockClear.Month, dateTime30DaysAgoClockClear.Day,
              0, 0, 0);

            while (next < dateTimeNowClockClear)
            {
                var nextDay = next.AddDays(1);
                var dailyWebView = websiteViews.Where(q => q.CreatedOn <= nextDay & q.CreatedOn >= next).ToList();
                websiteViewIn30DaysAgoList.Add(new WebsiteViewIn30DaysAgo
                {
                    Day = next.ToPersianString("yyyy-MM-dd"),
                    Count = dailyWebView.Count
                });

                next = next.AddDays(1);
            }

            return JsonConvert.SerializeObject(websiteViewIn30DaysAgoList, Formatting.None);
        }

        #endregion

        #region CountOfWebsiteViewIn12MonthsAgo

        public async Task<string> CountOfWebsiteViewIn12MonthsAgo()
        {
            var websiteViewIn12MonthsAgoList = new List<WebsiteViewIn12MonthsAgo>();
            var dateTimeNow = DateTime.Now;
            var dateTime12MonthsAgo = dateTimeNow.AddMonths(-12);

            var persianCalendar = new PersianCalendar();
            var year12Persian = persianCalendar.GetYear(dateTime12MonthsAgo);
            var month12Persian = persianCalendar.GetMonth(dateTime12MonthsAgo);
            var dateTime12MonthsClockClear = persianCalendar.ToDateTime(year12Persian, month12Persian, 1, 0, 0, 0, 0);

            var yearNowPersian = persianCalendar.GetYear(dateTimeNow);
            var monthNowPersian = persianCalendar.GetMonth(dateTimeNow);
            var dateTimeNowClockClear = persianCalendar.AddMonths(persianCalendar.ToDateTime(yearNowPersian, monthNowPersian, 1, 0, 0, 0, 0), 1);

            var websiteViews =
              _webView
                .AsNoTracking()
                .Where(q => q.CreatedOn <= dateTimeNowClockClear & q.CreatedOn >= dateTime12MonthsClockClear)
                .AsQueryable();

            var next = persianCalendar.ToDateTime(year12Persian, month12Persian, 1, 0, 0, 0, 0);

            while (next < dateTimeNowClockClear)
            {
                var nextmonth = persianCalendar.AddMonths(next, 1);
                var next1 = next;
                var dailyWebView = await websiteViews.Where(q => q.CreatedOn <= nextmonth & q.CreatedOn >= next1).CountAsync();

                websiteViewIn12MonthsAgoList.Add(new WebsiteViewIn12MonthsAgo
                {
                    Month = next.ToPersianString("yyyy-MM"),
                    Count = dailyWebView
                });

                next = persianCalendar.AddMonths(next, 1);
            }

            return JsonConvert.SerializeObject(websiteViewIn12MonthsAgoList, Formatting.None);
        }

        #endregion

        #region CountOfWebsiteViewIn7YearsAgo

        public async Task<string> CountOfWebsiteViewIn7YearsAgo()
        {
            var websiteViewIn7YearsAgoList = new List<WebsiteViewIn7YearsAgo>();
            var dateTimeNow = DateTime.Now;
            var dateTime7YearsAgo = dateTimeNow.AddYears(-7);

            var persianCalendar = new PersianCalendar();
            var year7Persian = persianCalendar.GetYear(dateTime7YearsAgo);
            var dateTime7YearsAgoClockClear = persianCalendar.ToDateTime(year7Persian, 1, 1, 0, 0, 0, 0);

            var yearNowPersian = persianCalendar.GetYear(dateTimeNow);
            var dateTimeNowClockClear = persianCalendar.AddYears(persianCalendar.ToDateTime(yearNowPersian, 1, 1, 0, 0, 0, 0), 1);

            var websiteViews =
              _webView
                .AsNoTracking()
                .Where(q => q.CreatedOn <= dateTimeNowClockClear & q.CreatedOn >= dateTime7YearsAgoClockClear)
                .AsQueryable();

            var next = persianCalendar.ToDateTime(year7Persian, 1, 1, 0, 0, 0, 0);

            while (next < dateTimeNowClockClear)
            {
                var nextyear = persianCalendar.AddYears(next, 1);
                var next1 = next;
                var dailyWebView = await websiteViews.Where(q => q.CreatedOn <= nextyear & q.CreatedOn >= next1).CountAsync();

                websiteViewIn7YearsAgoList.Add(new WebsiteViewIn7YearsAgo
                {
                    Year = next.ToPersianString("yyyy"),
                    Count = dailyWebView
                });

                next = persianCalendar.AddYears(next, 1);
            }

            return JsonConvert.SerializeObject(websiteViewIn7YearsAgoList, Formatting.None);
        }

        #endregion

        #region LastComments

        public async Task<CommentMainListViewModel> LastComments()
        {
            return await _commentService.GetCommentForMainPagedListAsync(new CommentSearchRequest
            {
                PageSize = 6,
                IsActive = ActiveStatus.All
            });
        }

        #endregion

        #region MostViewedPosts

        public async Task<List<MostViewedPostDashboardViewModel>> MostViewedPosts()
        {
            return await _webPostViewService.GetMostViewPosts();
        }

        #endregion

        #region ViewersDevice

        public async Task<string> ViewersDevice()
        {
            var viewersDevice = await
                _webView
                .GroupBy(q => q.Device)
                .OrderByDescending(id => id.Count())
                  .Select(s => new
                  {
                      Device = s.FirstOrDefault(),
                      Count = s.Count()
                  })
                  .Select(x => new ViewersPlatform
                  {
                      label = x.Device.Device,
                      value = x.Count
                  })
                .AsQueryable()
                .ToListAsync();

            var countAll = viewersDevice.Sum(item => item.value);

            var percent = viewersDevice.Select(item => new ViewersPlatform
            {
                label = item.label,
                value = (int)Math.Round(item.value / (double)countAll * 100)
            })
            .ToList();

            return JsonConvert.SerializeObject(percent, Formatting.None);
        }

        #endregion

        #region WebsiteStatistics

        public async Task<WebsiteStatistics> WebsiteStatistics()
        {
            return new WebsiteStatistics
            {
                PostCount = await _postService.CountOfAll(),
                CategoryCount = await _categoryService.CountOfAll(),
                UnconfirmedCommentCount = await _commentService.CountOfUnconfirmedComments(),
                UserCount = await _userService.CountOfAll()
            };
        }

        #endregion
    }
}
