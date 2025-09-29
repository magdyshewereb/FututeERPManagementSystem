using System.Data;

namespace ERPManagement.Application.Contracts.Persistence.Connection
{
	public interface IApplicationReadDbConnection
	{
		Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
			CancellationToken cancellationToken = default);

		Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
			CancellationToken cancellationToken = default);

		Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
			CancellationToken cancellationToken = default);
	}
}
