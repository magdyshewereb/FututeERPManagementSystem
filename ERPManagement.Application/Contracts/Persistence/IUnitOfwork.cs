namespace ERPManagement.Application.Contracts.Persistence;

public interface IUnitOfwork
{
	Task<int> SaveAsync();
	int Save();
	Task DisposeAsync();
	void Dispose();
}