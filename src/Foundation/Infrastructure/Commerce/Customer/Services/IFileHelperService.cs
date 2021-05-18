using System.IO;

namespace Foundation.Infrastructure.Commerce.Customer.Services
{
    public interface IFileHelperService
    {
        T[] GetImportData<T>(Stream file) where T : class;
    }
}