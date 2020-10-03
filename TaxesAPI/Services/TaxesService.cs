using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxesAPI.Models;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace TaxesAPI.Services
{
    public interface ITaxesService
    {
        public Task<TaxesItem> ReadAsync(int id);
        public Task<IEnumerable<TaxesItem>> ReadAsync(string Municipality);
        public Task<int> SaveAsync(TaxesItem taxes);
        public Task<int> SaveAsync(IEnumerable<TaxesItem> taxes);
        public Task<ActionResult<TaxesItem>> DeleteAsync(int id);
        public Task<IEnumerable<int>> DeleteAsync(IEnumerable<int> ids);
        public Task<int> ReadCSVAsync(string path);
    }

    public class TaxesService : ITaxesService
    {
        private readonly TaxesContext _context;

        public TaxesService(TaxesContext context)
        {
            _context = context;
        }

        public async Task<TaxesItem> ReadAsync(int id) // Single Read
        {
            return await _context.TaxesItem.FirstOrDefaultAsync(i => i.Id.Equals(id));
        }
        public async Task<IEnumerable<TaxesItem>> ReadAsync(string municipality) //Bulk Read
        {
            if (string.IsNullOrEmpty(municipality))
                throw new ArgumentNullException(nameof(municipality));
            return await _context.TaxesItem.Where(i => i.Municipality.Equals(municipality)).ToListAsync();
        }
        public async Task<int> SaveAsync(TaxesItem taxes) //Single Insert
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
        public async Task<int> SaveAsync(IEnumerable<TaxesItem> taxes)//Bulk Insert
        {
            await _context.TaxesItem.AddRangeAsync(taxes);
            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<TaxesItem>> DeleteAsync(int id) //Single Delete
        {
            var taxesItem = await _context.TaxesItem.FindAsync(id);
            if (taxesItem  != null)
            {
                _context.Remove(taxesItem);
                await _context.SaveChangesAsync();
            }
            return taxesItem;
        }
        public async Task<IEnumerable<int>> DeleteAsync(IEnumerable<int> ids) //Bulk Delete
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

        public async Task<int> ReadCSVAsync(string path) //Read from CSV
        {
            using (var reader = new StreamReader("path"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TaxesItem>();
                return  await SaveAsync(records);
            }
        }

        

    }

}
