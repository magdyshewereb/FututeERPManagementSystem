using AutoMapper;
using ERPManagement.Application.Contracts.Persistence;
using Hr.Application.Interfaces.GenericRepository.Command;

namespace ERPManagement.Application.Features.Common.Base.Commands
{
	public class DeleteBaseCommandHandler<T> where T : class

	{

		internal readonly IBaseRepository<T> _repo;
		internal readonly IMapper _mapper;
		internal IUnitOfwork _unitOfwork;

		public DeleteBaseCommandHandler(IBaseRepository<T> branchRepository, IMapper mapper, IUnitOfwork unitOfwork)
		{
			_repo = branchRepository;
			_mapper = mapper;
			_unitOfwork = unitOfwork;
		}
		internal async Task<bool> DeleteHandle(int id, CancellationToken cancellationToken)
		{
			await _repo.DeleteAsync(id);
			await _unitOfwork.SaveAsync();

			return true;
		}


	}
}
