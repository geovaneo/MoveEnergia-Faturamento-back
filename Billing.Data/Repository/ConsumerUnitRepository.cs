using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class ConsumerUnitRepository : BaseRepository<ConsumerUnit>, IConsumerUnitRepository
    {
        public ConsumerUnitRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<ConsumerUnit>> GetByIdUserAsync(long idUser)
        {

            var result = await _context.Set<ConsumerUnit>().AsNoTracking()
                                       .Include(c => c.Customer)
                                            .ThenInclude(c => c.Addresses)
                                                .ThenInclude(a => a.City)
                                                    .ThenInclude(c => c.UF)
                                                        .ThenInclude( u => u.Country)
                                       .Include(c => c.State)
                                       .Include(c => c.Subgroup) 
                                       .Where(c => c.UserId == idUser).ToListAsync();

            return result;
        }
        public async Task<List<ConsumerUnit>> GetAdressByIdUserAsync(long idUser)
        {

            var result = await _context.Set<ConsumerUnit>().AsNoTracking()
                                       .Include(c => c.Customer)
                                            .ThenInclude(c => c.Addresses)
                                                .ThenInclude(a => a.City)
                                                    .ThenInclude(c => c.UF)
                                                        .ThenInclude(u => u.Country)
                                       .Include(c => c.State)
                                       .Where(c => c.UserId == idUser).ToListAsync();
            return result;
        }
        public async Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByUcAsync(string UC)
        {

            var currentInvoice = await (
                        from d in _context.DetalhesFaturaCache
                        join c in _context.ConsumerUnits on d.Uc equals c.UC
                        join f in _context.FaturaCache on d.FaturaId equals f.FaturaId
                        where d.Uc == UC
                        orderby d.MesReferencia descending
                        select new CurrentInvoiceResponseDto
                        {
                            Id = f.Id,
                            CustomerId = c.CustomerId,
                            BillingNumber = d.FaturaId.ToString(),
                            IssuedDate = d.DataEmissao.Value.ToString("dd/MM/yyyy"),
                            InstallationNumber = c.UC,
                            ClientNumber = c.UC,
                            BillingMonth = d.MesReferencia.ToString(),
                            DueDate = d.DataVencimento.Value.ToString("dd/MM/yyyy"),
                            TotalValue = d.Total.Value,
                            CompensatedEnergy = d.QtEnergiaCompensadaHfp.ToString(),    
                            MonthSavings = d.VlEconomia.ToString(),
                            invoicesStatus = f.Status,
                            TotalSavings = new List<string>(),                            
                        }).FirstOrDefaultAsync();

            if (currentInvoice != null)
            {
                currentInvoice.TotalSavings = await _context.DetalhesFaturaCache
                    .Where(d => d.Uc == UC && d.VlEconomia != null && d.VlEconomia != "" && d.VlEconomia != "0")
                    .OrderBy(d => d.MesReferencia)
                    .Select(d => d.VlEconomia)
                    .ToListAsync();
            }

            return currentInvoice;
        }
        public async Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByIdUcAsync(int idUC)
        {

            var currentInvoice = await (
                        from d in _context.DetalhesFaturaCache
                        join c in _context.ConsumerUnits on d.Uc equals c.UC
                        join f in _context.FaturaCache on d.FaturaId equals f.FaturaId
                        where c.Id == idUC 
                        orderby d.MesReferencia descending
                        select new CurrentInvoiceResponseDto
                        {
                            Id = f.Id,
                            CustomerId = c.CustomerId,
                            BillingNumber = d.FaturaId.ToString(),
                            IssuedDate = d.DataEmissao.Value.ToString("dd/MM/yyyy"),
                            InstallationNumber = c.UC,
                            ClientNumber = c.UC,
                            BillingMonth = d.MesReferencia.ToString(),
                            DueDate = d.DataVencimento.Value.ToString("dd/MM/yyyy"),
                            TotalValue = d.Total.Value,
                            CompensatedEnergy = d.QtEnergiaCompensadaHfp.ToString(),
                            MonthSavings = d.VlEconomia.ToString(),
                            invoicesStatus = f.Status,
                            TotalSavings = new List<string>(),
                            UC = c.UC
                        }).FirstOrDefaultAsync();

            if (currentInvoice != null)
            {
                currentInvoice.TotalSavings = await _context.DetalhesFaturaCache
                    .Where(d => d.Uc == currentInvoice.UC && d.VlEconomia != null && d.VlEconomia != "" && d.VlEconomia != "0")
                    .OrderBy(d => d.MesReferencia)
                    .Select(d => d.VlEconomia)
                    .ToListAsync();
            }

            return currentInvoice;
        }
        public async Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcAsync(int idUc)
        {
            var result = await _context.Set<ConsumerUnit>().AsNoTracking()
                                            .Include(c => c.ConsumerUnitMeasurements)
                                       .Where(c => c.Id == idUc).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcReferenMonthAsync(int idUc, DateTime referenceDate)
        {
            var result = await _context.Set<ConsumerUnit>()
                .AsNoTracking()
                .Include(c => c.ConsumerUnitMeasurements
                    .Where(m => m.Date.Month == referenceDate.Month && m.Date.Year == referenceDate.Year))
                .Where(c => c.Id == idUc).FirstOrDefaultAsync();

            return result;
        }
    }
}
