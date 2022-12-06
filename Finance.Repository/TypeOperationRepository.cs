using AutoMapper;
using Finance.Infrastructure;
using Finance.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Repository
{
    public class TypeOperationRepository : ITypeOperationRepository
    {
        private readonly FinanceContext _db;
        private readonly IMapper _mapper;

        public TypeOperationRepository(FinanceContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(ViewModel.TypeOperation operation)
        {
            await _db.TypeOperations.AddAsync(_mapper.Map<TypeOperation>(operation));
        }
        public async Task<IEnumerable<ViewModel.TypeOperation>> GetAsync()
        {
            var listOperation = await _db.TypeOperations.ToListAsync();

            return _mapper.Map<IEnumerable<ViewModel.TypeOperation>>(listOperation);
        }

        public async Task<ViewModel.TypeOperation> GetByIdAsync(int id)
        {
            var operation = await _db.TypeOperations.FirstOrDefaultAsync(x => x.TypeOperationId == id);

            return _mapper.Map<ViewModel.TypeOperation>(operation);
        }

        public async Task<IEnumerable<ViewModel.TypeOperation>> GetByTypeAsync(bool type)
        {
            var listOperation = await _db.TypeOperations.Where(x => x.IsIncome == type).ToListAsync();

            return _mapper.Map<IEnumerable<ViewModel.TypeOperation>>(listOperation);
        }
    }

}
