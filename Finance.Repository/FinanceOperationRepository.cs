using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Finance.Infrastructure;
using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Repository
{
    public class FinanceOperationRepository : IFinanceOperationRepository
    {
        private readonly FinanceContext _db;
        private readonly IMapper _mapper;

        public FinanceOperationRepository(FinanceContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }
        public void Edit(ViewModel.FinanceOperation operationWithOldData, ViewModel.FinanceOperation operationWithNewData)
        {
            _db.Entry(_mapper.Map<FinanceOperation>(operationWithOldData)).CurrentValues.SetValues(_mapper.Map<FinanceOperation>(operationWithNewData));
        }
        public async Task CreateAsync(ViewModel.FinanceOperation operation)
        {
            await _db.Operations.AddAsync(_mapper.Map<FinanceOperation>(operation));
        }
        public async Task<IEnumerable<ViewModel.FinanceOperation>> GetAsync()
        {
            var listOperation = await _db.Operations.Include(p => p.TypeOperation).ToListAsync();

            return _mapper.Map<IEnumerable<ViewModel.FinanceOperation>>(listOperation);
        }

        public async Task<IEnumerable<ViewModel.FinanceOperation>> GetByDataAsync(DateTime date, bool type)
        {
            var selectedOperation = new List<FinanceOperation>();
            var listOperation = await _db.Operations.Include(p => p.TypeOperation).Where(x => x.TypeOperation.IsIncome == type).ToListAsync();

            Parallel.ForEach(listOperation, oper =>
            {
                if (DateTime.Parse(oper.Data) == date)
                {
                    selectedOperation.Add(oper);
                }
            });

            return _mapper.Map<IEnumerable<ViewModel.FinanceOperation>>(selectedOperation);
        }

        public async Task<ViewModel.FinanceOperation> GetByIdAsync(int id)
        {
            var operation = await _db.Operations.FirstOrDefaultAsync(x => x.FinanceOperationId == id);

            return _mapper.Map<ViewModel.FinanceOperation>(operation);
        }

        public async Task<IEnumerable<ViewModel.FinanceOperation>> GetByPeriodAsync(DateTime date1, DateTime date2, bool type)
        {
            var selectedOperation = new List<FinanceOperation>();
            var listOperation = await _db.Operations.Include(p => p.TypeOperation).Where(x => x.TypeOperation.IsIncome == type).ToListAsync();

            Parallel.ForEach(listOperation, oper =>
            {
                if (DateTime.Parse(oper.Data) > date1 && DateTime.Parse(oper.Data) < date2)
                {
                    selectedOperation.Add(oper);
                }
            });

            return _mapper.Map<IEnumerable<ViewModel.FinanceOperation>>(listOperation);
        }
    }
}
