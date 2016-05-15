using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Common.Utility.Spreadsheets.Writer;
using BrightLine.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BrightLine.Tests.Unit.Common.Spreadsheets
{
    //[TestFixture]
    public class SpreadsheetWriterTests
    {
        //[Test]
        public void CanWriteTable()
        {
            var props = new List<PropertyInfo>()
            {
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.Title),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.Page),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.Brand),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.ContentType),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.Theme),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.TotalViews),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.Date),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.AvgCompletionRate),
                ExpressionHelper.GetPropertyInfo<VideoDetail>( vid => vid.AvgTimeSpent)
            };

            // data 
            var videos = new List<VideoDetail>()
            {
                new VideoDetail(){ Title = "Title 1 ", Page = "page 1 ", Brand = "brand 1 ", ContentType = "content type 1 ", Theme = "theme 1 ", TotalViews = 1 , Date = DateTime.Today.AddDays(1 ), AvgCompletionRate = 10.1 , AvgTimeSpent = new TimeSpan(0, 1 , 30) },
                new VideoDetail(){ Title = "Title 2 ", Page = "page 2 ", Brand = "brand 2 ", ContentType = "content type 2 ", Theme = "theme 2 ", TotalViews = 2 , Date = DateTime.Today.AddDays(2 ), AvgCompletionRate = 10.2 , AvgTimeSpent = new TimeSpan(0, 2 , 30) },
                new VideoDetail(){ Title = "Title 3 ", Page = "page 3 ", Brand = "brand 3 ", ContentType = "content type 3 ", Theme = "theme 3 ", TotalViews = 3 , Date = DateTime.Today.AddDays(3 ), AvgCompletionRate = 10.3 , AvgTimeSpent = new TimeSpan(0, 3 , 30) },
                new VideoDetail(){ Title = "Title 4 ", Page = "page 4 ", Brand = "brand 4 ", ContentType = "content type 4 ", Theme = "theme 4 ", TotalViews = 4 , Date = DateTime.Today.AddDays(4 ), AvgCompletionRate = 10.4 , AvgTimeSpent = new TimeSpan(0, 4 , 30) },
                new VideoDetail(){ Title = "Title 5 ", Page = "page 5 ", Brand = "brand 5 ", ContentType = "content type 5 ", Theme = "theme 5 ", TotalViews = 5 , Date = DateTime.Today.AddDays(5 ), AvgCompletionRate = 10.5 , AvgTimeSpent = new TimeSpan(0, 5 , 30) },
                new VideoDetail(){ Title = "Title 6 ", Page = "page 6 ", Brand = "brand 6 ", ContentType = "content type 6 ", Theme = "theme 6 ", TotalViews = 6 , Date = DateTime.Today.AddDays(6 ), AvgCompletionRate = 10.6 , AvgTimeSpent = new TimeSpan(0, 6 , 30) },
                new VideoDetail(){ Title = "Title 7 ", Page = "page 7 ", Brand = "brand 7 ", ContentType = "content type 7 ", Theme = "theme 7 ", TotalViews = 7 , Date = DateTime.Today.AddDays(7 ), AvgCompletionRate = 10.7 , AvgTimeSpent = new TimeSpan(0, 7 , 30) },
                new VideoDetail(){ Title = "Title 8 ", Page = "page 8 ", Brand = "brand 8 ", ContentType = "content type 8 ", Theme = "theme 8 ", TotalViews = 8 , Date = DateTime.Today.AddDays(8 ), AvgCompletionRate = 10.8 , AvgTimeSpent = new TimeSpan(0, 8 , 30) },
                new VideoDetail(){ Title = "Title 9 ", Page = "page 9 ", Brand = "brand 9 ", ContentType = "content type 9 ", Theme = "theme 9 ", TotalViews = 9 , Date = DateTime.Today.AddDays(9 ), AvgCompletionRate = 10.9 , AvgTimeSpent = new TimeSpan(0, 9 , 30) },
                new VideoDetail(){ Title = "Title 10", Page = "page 10", Brand = "brand 10", ContentType = "content type 10", Theme = "theme 10", TotalViews = 10, Date = DateTime.Today.AddDays(10), AvgCompletionRate = 10.10, AvgTimeSpent = new TimeSpan(0, 10, 30) },
                new VideoDetail(){ Title = "Title 11", Page = "page 11", Brand = "brand 11", ContentType = "content type 11", Theme = "theme 11", TotalViews = 11, Date = DateTime.Today.AddDays(11), AvgCompletionRate = 10.11, AvgTimeSpent = new TimeSpan(0, 11, 30) },
                new VideoDetail(){ Title = "Title 12", Page = "page 12", Brand = "brand 12", ContentType = "content type 12", Theme = "theme 12", TotalViews = 12, Date = DateTime.Today.AddDays(12), AvgCompletionRate = 10.12, AvgTimeSpent = new TimeSpan(0, 12, 30) },
                new VideoDetail(){ Title = "Title 13", Page = "page 13", Brand = "brand 13", ContentType = "content type 13", Theme = "theme 13", TotalViews = 13, Date = DateTime.Today.AddDays(13), AvgCompletionRate = 10.13, AvgTimeSpent = new TimeSpan(0, 13, 30) },
                new VideoDetail(){ Title = "Title 14", Page = "page 14", Brand = "brand 14", ContentType = "content type 14", Theme = "theme 14", TotalViews = 14, Date = DateTime.Today.AddDays(14), AvgCompletionRate = 10.14, AvgTimeSpent = new TimeSpan(0, 14, 30) },
                new VideoDetail(){ Title = "Title 15", Page = "page 15", Brand = "brand 15", ContentType = "content type 15", Theme = "theme 15", TotalViews = 15, Date = DateTime.Today.AddDays(15), AvgCompletionRate = 10.15, AvgTimeSpent = new TimeSpan(0, 15, 30) },
                new VideoDetail(){ Title = "Title 16", Page = "page 16", Brand = "brand 16", ContentType = "content type 16", Theme = "theme 16", TotalViews = 16, Date = DateTime.Today.AddDays(16), AvgCompletionRate = 10.16, AvgTimeSpent = new TimeSpan(0, 16, 30) },
                new VideoDetail(){ Title = "Title 17", Page = "page 17", Brand = "brand 17", ContentType = "content type 17", Theme = "theme 17", TotalViews = 17, Date = DateTime.Today.AddDays(17), AvgCompletionRate = 10.17, AvgTimeSpent = new TimeSpan(0, 17, 30) },
                new VideoDetail(){ Title = "Title 18", Page = "page 18", Brand = "brand 18", ContentType = "content type 18", Theme = "theme 18", TotalViews = 18, Date = DateTime.Today.AddDays(18), AvgCompletionRate = 10.18, AvgTimeSpent = new TimeSpan(0, 18, 30) },
                new VideoDetail(){ Title = "Title 19", Page = "page 19", Brand = "brand 19", ContentType = "content type 19", Theme = "theme 19", TotalViews = 19, Date = DateTime.Today.AddDays(19), AvgCompletionRate = 10.19, AvgTimeSpent = new TimeSpan(0, 19, 30) },
                new VideoDetail(){ Title = "Title 20", Page = "page 20", Brand = "brand 20", ContentType = "content type 20", Theme = "theme 20", TotalViews = 20, Date = DateTime.Today.AddDays(20), AvgCompletionRate = 10.20, AvgTimeSpent = new TimeSpan(0, 20, 30) }
            };
            
            ISpreadSheetWriter writer = SpreadsheetHelper.GetWriter();
            writer.CreateWorkBook();
            writer.CreateSheet("Video Detail", true);
            writer.WriteTable<VideoDetail>(props, videos);
            writer.CreateSheet("test", true);
            writer.WriteList("test", 0, 0, new int[4]{1,2,3,4}, false);
            writer.WriteList("test", 1, 0, new string[4] { "a", "b", "c", "d" }, false);
            writer.WriteList("test", 2, 0, new List<string>() { "a", "b", "c", "d" }, false);
            writer.Save(@"c:\temp\brightline\components\iq_export1.xls");
        }


        class VideoDetail
        {
            public VideoDetail()
            {
            }

            public string Title { get; set; }
            public string Page { get; set; }
            public string Brand { get; set; }
            public string ContentType { get; set; }
            public string Theme { get; set; }
            public int TotalViews { get; set; }
            public double AvgCompletionRate { get; set; }
            public TimeSpan AvgTimeSpent { get; set; }

            // This is just to test writing out all types of data ( date )
            public DateTime Date { get; set; }
        }
    }
}
