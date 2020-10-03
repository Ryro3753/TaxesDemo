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
        public void DailyControl_OneText_OneRatio()
        {
            var taxesServiceMock = new Mock<ITaxesService>();
            var service = new TaxesRatioService(taxesServiceMock.Object);
            var taxesList = new List<TaxesItem>();
            taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow, TaxesSchedule = "daily", TaxesRatio = 1.2 });


            var result = service.DailyControl(taxesList, DateTime.UtcNow.AddHours(-1));


            Assert.Equal(1.2, result);
        }

        [Fact]
        public void Test1()
        {
            //var moqDatabase = new Mock<TaxesAPI.Models.TaxesContext>();

            //var service = new TaxesRatioService(moqDatabase.Object);
            //var taxesServiceMock = new Mock<ITaxesService>();

            //taxesServiceMock.Setup(i => i.Read("istanbul")).ReturnsAsync(taxesList);



            ////var service = new TaxesRatioService();


            //var taxesList = new List<TaxesItem>();
            //taxesList.Add(new TaxesItem { Municipality = "istanbul", Date = DateTime.UtcNow, TaxesSchedule = "daily", TaxesRatio = 1.2 });
            //var result = service.DailyControl(taxesList, DateTime.UtcNow.AddHours(-1));
            //Assert.Equal(1.2, result);
        }
    }
}
