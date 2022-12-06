using Finance.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.Infrastructure
{
    public interface ITypeOperationRepository
    {
        Task CreateAsync(TypeOperation operation);
        Task<IEnumerable<TypeOperation>> GetAsync();
        Task<IEnumerable<TypeOperation>> GetByTypeAsync(bool type);
        Task<TypeOperation> GetByIdAsync(int id);
    }
}
