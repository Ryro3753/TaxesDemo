using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxesAPI.Models;

namespace TaxesAPI.Services
{
    public interface ITaxesRatioService
    {
        double DailyControl(IEnumerable<TaxesItem> list, DateTime dt);
        Task<double> GetRatio(DateTime dt, string municipality);
    }

    public class TaxesRatioService : ITaxesRatioService
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

            var dailyControl = DailyControl(list, dt);
            if (dailyControl != 0)
                return dailyControl;

            var weeklyControl = WeeklyControl(list, dt);
            if (weeklyControl != 0)
                return weeklyControl;

            var monthlyControl = MonthlyControl(list, dt);
            if (monthlyControl != 0)
                return monthlyControl;

            var yearlyControl = YearlyControl(list, dt);
            if (yearlyControl != 0)
                return yearlyControl;

            return 0;
        }

        public double DailyControl(IEnumerable<TaxesItem> list, DateTime dt)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("daily")))
            {
                if (item.Date.Year.Equals(dt.Year) && item.Date.DayOfYear.Equals(dt.DayOfYear))
                    return item.TaxesRatio;
            }
            return 0;
        }
        public double WeeklyControl(IEnumerable<TaxesItem> list, DateTime dt)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("weekly")))
            {
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                if (item.Date.AddDays(-1 * (int)cal.GetDayOfWeek(item.Date)).DayOfYear.Equals(dt.AddDays(-1 * (int)cal.GetDayOfWeek(dt)).DayOfYear) )
                    return item.TaxesRatio;
            }
            return 0;
        }
        public double MonthlyControl(IEnumerable<TaxesItem> list, DateTime dt)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("monthly")))
            {
                if (item.Date.Month.Equals(dt.Month) && item.Date.Year.Equals(dt.Year))
                    return item.TaxesRatio;
            }
            return 0;
        }
        public double YearlyControl(IEnumerable<TaxesItem> list, DateTime dt)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("yearly")))
            {
                if (item.Date.Year.Equals(dt.Year))
                    return item.TaxesRatio;
            }
            return 0;
        }
    }
}
