using Finance.Infrastructure.CustomExceptions;
using Finance.Infrastructure;
using Finance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Application
{
    public class FinanceOperationService : IFinanceOperationService
    {
        private readonly IRepositoryManager _repository;

        public FinanceOperationService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(FinanceOperation operation)
        {
            await _repository.FinanceOperation.CreateAsync(operation);
            await _repository.SaveChangesAsync();
        }
        public async Task<IEnumerable<FinanceOperation>> GetAsync()
        {
            return await _repository.FinanceOperation.GetAsync();
        }

        public async Task<IEnumerable<object>> GetByDataAsync(string dataStr)
        {
            var isData = DateTime.TryParse(dataStr, out var data);

            if (!isData)
            {
                throw new BadRequestException();
            }

            decimal sumIncome = 0;
            decimal sumExpence = 0;
            var result = new List<object>();

            var listIncome = await _repository.FinanceOperation.GetByDataAsync(data, true);
            var listExpence = await _repository.FinanceOperation.GetByDataAsync(data, false);

            if (!listExpence.Any() && !listIncome.Any())
            {
                throw new NotFoundException();
            }

            foreach (var oper in listIncome)
            {
                sumIncome += oper.Value;
            }

            foreach (var oper in listExpence)
            {
                sumExpence += oper.Value;
            }

            result.Add(sumIncome);
            result.Add(sumExpence);
            result.Add(listIncome);
            result.Add(listExpence);

            return result;
        }

        public async Task<IEnumerable<object>> GetByPeriodAsync(string dataBeginnigStr, string dataEndStr)
        {
            var isData1 = DateTime.TryParse(dataBeginnigStr, out var data1);
            var isData2 = DateTime.TryParse(dataEndStr, out var data2);

            if (!isData1 || !isData2)
            {
                throw new BadRequestException();
            }

            decimal sumIncome = 0;
            decimal sumExpence = 0;
            var result = new List<object>();

            var listIncome = await _repository.FinanceOperation.GetByPeriodAsync(data1, data2, true);
            var listExpence = await _repository.FinanceOperation.GetByPeriodAsync(data1, data2, false);

            if (!listExpence.Any() && !listIncome.Any())
            {
                throw new NotFoundException();
            }

            foreach (var oper in listIncome)
            {
                sumIncome += oper.Value;
            }

            foreach (var oper in listExpence)
            {
                sumExpence += oper.Value;
            }

            result.Add(sumIncome);
            result.Add(sumExpence);
            result.Add(listIncome);
            result.Add(listExpence);

            return result;
        }
    }
}
