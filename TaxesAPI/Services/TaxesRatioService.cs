using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxesAPI.Models;

namespace TaxesAPI.Services
{
    public interface ITaxesRatioService1
    {
        double DailyControl(IEnumerable<TaxesItem> list, DateTime dt);
        Task<double> GetRatio(DateTime dt, string municipality);
    }

    public class TaxesRatioService : ITaxesRatioService1
    {
        private readonly ITaxesService _taxesService;

        public TaxesRatioService(ITaxesService taxesService)
        {
            _taxesService = taxesService;
        }
        public async Task<double> GetRatio(DateTime dt, string municipality)
        {
            var taxesMunicipality = await _taxesService.ReadAsync(municipality);
            var list = taxesMunicipality.ToList();
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("daily")))
            {
                if (item.Date == dt)
                    return item.TaxesRatio;
            }
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("weekly")))
            {
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                if (item.Date.AddDays(-1 * (int)cal.GetDayOfWeek(item.Date)).Equals(dt.AddDays(-1 * (int)cal.GetDayOfWeek(dt))))
                    return item.TaxesRatio;
            }
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("monthly")))
            {
                if (item.Date.Month.Equals(dt.Month) && item.Date.Year.Equals(dt.Year))
                    return item.TaxesRatio;
            }
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("yearly")))
            {
                if (item.Date.Year.Equals(dt.Year))
                    return item.TaxesRatio;
            }
            return 0;
        }

        public double DailyControl(IEnumerable<TaxesItem> list, DateTime dt)
        {
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("daily")))
            {
                if (item.Date.Year.Equals(dt.Year) && item.Date.DayOfYear.Equals(dt.DayOfYear))
                    return item.TaxesRatio;
            }
            return 0;
        }

    }
}
