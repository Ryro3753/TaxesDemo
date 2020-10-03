using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxesAPI.Models;

namespace TaxesAPI.Services
{
    public interface ITaxesService
    {
        public Task<TaxesItem> Read(int id);
        public Task<IEnumerable<TaxesItem>> Read(string Municipality);
        public Task<int> Save(TaxesItem taxes);
        public Task<int> Save(IEnumerable<TaxesItem> taxes);
        public Task<ActionResult<TaxesItem>> Delete(int id);
        public Task<IEnumerable<int>> Delete(IEnumerable<int> ids);
    }

    public class TaxesService : ITaxesService
    {
        private readonly TaxesContext _context;

        public TaxesService(TaxesContext context)
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

        

    }

}
