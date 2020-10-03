using System;
using Xunit;
using TaxesAPI;
using TaxesAPI.Services;
using Moq;
using System.Collections.Generic;
using TaxesAPI.Models;

namespace TaxesAPI.Test
{
    public class TaxesRatioServiceTest
    {
        [Fact]
        public void DailyControl_OneTax_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow, TaxesSchedule = "daily", TaxesRatio = 1.2 });


            var result = service.DailyControl(taxesList, DateTime.UtcNow.AddHours(-1));


            Assert.Equal(1.2, result);
        }
        [Fact]
        public void WeeklyControl_OneTax_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow.AddDays(-3), TaxesSchedule = "weekly", TaxesRatio = 1.4 });


            var result = service.WeeklyControl(taxesList, DateTime.UtcNow.AddDays(-4));


            Assert.Equal(1.4, result);
        }
        [Fact]
        public void MonthlyControl_OneTax_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow, TaxesSchedule = "monthly", TaxesRatio = 1.6 });


            var result = service.MonthlyControl(taxesList, DateTime.UtcNow.AddDays(-1));


            Assert.Equal(1.6, result);
        }
        [Fact]
        public void YearlyControl_OneTax_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow, TaxesSchedule = "yearly", TaxesRatio = 1.8 });


            var result = service.YearlyControl(taxesList, DateTime.UtcNow.AddDays(-1));


            Assert.Equal(1.8, result);
        }
        [Fact]

        public void YearlyControl_TwoTax_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem {
                Municipality = "istanbul",
                Date = DateTime.UtcNow,
                TaxesSchedule = "yearly",
                TaxesRatio = 1.8
            });
            taxesList.Add(new TaxesItem
            {
                Municipality = "istanbul",
                Date = DateTime.UtcNow.AddDays(-365),
                TaxesSchedule = "yearly",
                TaxesRatio = 1.1
            });



            var result = service.YearlyControl(taxesList, DateTime.UtcNow.AddDays(-365));


            Assert.Equal(1.1, result);
        }
        
    }
}
