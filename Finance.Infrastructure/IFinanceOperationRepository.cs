using Finance.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.Infrastructure
{
    public interface IFinanceOperationRepository
    {
        Task CreateAsync(FinanceOperation operation);
        Task<IEnumerable<FinanceOperation>> GetAsync();
        Task<FinanceOperation> GetByIdAsync(int id);
        Task<IEnumerable<FinanceOperation>> GetByDataAsync(DateTime data, bool type);
        Task<IEnumerable<FinanceOperation>> GetByPeriodAsync(DateTime data1, DateTime data2, bool type);
    }
}
