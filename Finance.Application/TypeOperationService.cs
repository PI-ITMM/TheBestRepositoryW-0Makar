using Finance.Infrastructure;
using Finance.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.Application
{
    public class TypeOperationService : ITypeOperationService
    {
        private readonly IRepositoryManager _repository;

        public TypeOperationService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task DeleteAsync(int id)
        {
            var typeOperation = await _repository.TypeOperation.GetByIdAsync(id);

            if (typeOperation == null)
            {
                throw new NotFoundException();
            }

            _repository.TypeOperation.Delete(typeOperation);
            await _repository.SaveChangesAsync();
        }
        public async Task EditAsync(TypeOperation newOperation)
        {
            var oldTypeOperation = await _repository.TypeOperation.GetByIdAsync(newOperation.TypeOperationId);

            if (oldTypeOperation == null)
            {
                throw new NotFoundException();
            }

            _repository.TypeOperation.Edit(oldTypeOperation, newOperation);
            await _repository.SaveChangesAsync();
        }

        public async Task CreateAsync(TypeOperation operation)
        {
            await _repository.TypeOperation.CreateAsync(operation);
            await _repository.SaveChangesAsync();
        }
        public async Task<IEnumerable<TypeOperation>> GetAsync()
        {
            return await _repository.TypeOperation.GetAsync();
        }

        public async Task<IEnumerable<TypeOperation>> GetByTypeAsync(bool type)
        {
            return await _repository.TypeOperation.GetByTypeAsync(type);
        }
    }
}
