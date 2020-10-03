using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxesAPI.Models;

namespace TaxesAPI.Services
{
    public interface ITaxes
    {
        public Task<TaxesItem> Read(int id);
        public Task<IEnumerable<TaxesItem>> Read(string Municipality);
        public Task<int> Save(TaxesItem taxes);
        public Task<int> Save(IEnumerable<TaxesItem> taxes);
        public Task<ActionResult<TaxesItem>> Delete(int id);
        public Task<IEnumerable<int>> Delete(IEnumerable<int> ids);
        public Task<double> GetRatio(DateTime dt, string municipality);
    }

    public class Taxes : ITaxes
    {
        private readonly TaxesContext _context;

        public Taxes(TaxesContext context)
        {
            _context = context;
        }

        public async Task<TaxesItem> Read(int id)
        {
            return await _context.TaxesItem.FirstOrDefaultAsync(i => i.Id.Equals(id));
        }
        public async Task<IEnumerable<TaxesItem>> Read(string Municipality)
        {
            return await _context.TaxesItem.Where(i => i.Municipality.Equals(Municipality)).ToListAsync();
        }
        public async Task<int> Save(TaxesItem taxes)
        {
            var isNew = await _context.TaxesItem.FindAsync(taxes.Id);
            if (isNew == null)
            {
                await _context.TaxesItem.AddAsync(taxes);
            }
            else
            {
                _context.Update(taxes);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Save(IEnumerable<TaxesItem> taxes)
        {
            await _context.TaxesItem.AddRangeAsync(taxes);
            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<TaxesItem>> Delete(int id)
        {
            var taxesItem = await _context.TaxesItem.FindAsync(id);
            if (taxesItem  != null)
            {
                _context.Remove(taxesItem);
                await _context.SaveChangesAsync();
            }
            return taxesItem;
        }
        public async Task<IEnumerable<int>> Delete(IEnumerable<int> ids)
        {
            TaxesItem taxesItem;
            List<int> deleted = new List<int>();
            foreach (var item in ids.ToList())
            {
                taxesItem = await _context.TaxesItem.FindAsync(item);
                if (taxesItem != null)
                {
                    deleted.Add(item);
                    _context.Remove(taxesItem);
                }
            }
            await _context.SaveChangesAsync();
            return deleted;

        }

        public async Task<double> GetRatio(DateTime dt, string municipality)
        {
            var taxesMunicipality = await Read(municipality);
            var list = taxesMunicipality.ToList();
            foreach (var item in list.Where(i => i.TaxesSchedule.Equals("daily")))
            {
                if(item.Date == dt)
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

    }

}
