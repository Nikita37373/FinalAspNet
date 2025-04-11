using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.DAL.Repositories.Laptops
{
    public interface ILaptopRepository
        : IGenericRepository<Laptop, string>
    {
        IQueryable<Laptop> GetLaptops(Expression<Func<Laptop, bool>>? pred = null);
    }
}
