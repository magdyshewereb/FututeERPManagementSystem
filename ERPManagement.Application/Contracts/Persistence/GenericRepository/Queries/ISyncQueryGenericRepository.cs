using ERPManagement.Application.Shared.Enums;
using System.Linq.Expressions;

namespace RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries
{
	public partial interface IQueryGenericRepository<T>
    {
        IQueryable<T> GetQueriable();
        T Find(long id);

        TResult GetSingle<TResult>(
            Expression<Func<T, bool>>? query,
            Func<T, TResult> selector,
            List<string>? includes = default,
            bool asTracking = false);

        List<TResult> GetList<TResult>(
            Expression<Func<T, bool>>? query,
            Func<T, TResult> selector,
            Expression<Func<T, object>>? orderBy = default,
            OrderType? orderType = OrderType.ASC,
            List<string>? includes = default,
            int? skip = default,
            int? take = null,
            bool? distinct = false,
            bool asTracking = false);

        List<T> GetList(
            Expression<Func<T, bool>>? query = default,
            Expression<Func<T, object>>? orderBy = default,
            OrderType? orderType = OrderType.ASC,
            List<string>? includes = default,
            int? skip = 0,
            int? take = null,
            bool? distinct = false,
            bool asTracking = false);

        List<TResult> GetAll<TResult>(
            Func<T, TResult> selector,
            Expression<Func<T, object>>? orderBy = default,
            OrderType? orderType = OrderType.ASC,
            List<string>? includes = default,
            int? skip = null,
            int? take = null,
            bool? distinct = null,
            bool asTracking = false);

        List<T> GetAll(
            Expression<Func<T, object>>? orderBy = default,
            OrderType? orderType = OrderType.ASC,
            List<string>? includes = null,
            int? skip = 0,
            int? take = null,
            bool? distinct = false,
            bool asTracking = false);

        long GetCount(Expression<Func<T, bool>>? query = default);
        bool Any(Expression<Func<T, bool>> query);
        bool IsNameExist(Expression<Func<T, bool>> query);
    }
}
