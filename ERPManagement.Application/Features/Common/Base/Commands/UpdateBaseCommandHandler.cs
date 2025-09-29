using AutoMapper;
using ERPManagement.Application.Contracts.Persistence;
using Hr.Application.Interfaces.GenericRepository.Command;

namespace ERPManagement.Application.Features.Common.Base.Commands
{
	public class UpdateBaseCommandHandler<T, TViewModel>
			 where T : class
		where TViewModel : class
	{
		internal readonly IBaseRepository<T> _repo;
		internal readonly IMapper _mapper;
		internal IUnitOfwork _unitOfwork;

		public UpdateBaseCommandHandler(IBaseRepository<T> branchRepository, IMapper mapper, IUnitOfwork unitOfwork)
		{
			_repo = branchRepository;
			_mapper = mapper;
			_unitOfwork = unitOfwork;
		}
		public async Task<T> UpdateHandle(TViewModel request, CancellationToken cancellationToken)
		{

			T model = _mapper.Map<T>(request);

			#region Set "NameEn" to "NameAr" if "NameEn" is null or empty
			// Get properties of the model
			var properties = model.GetType().GetProperties();

			// Get "NameAr" and "NameEn" properties
			var nameArProperty = properties.FirstOrDefault(p => p.Name == "NameAr");
			var nameEnProperty = properties.FirstOrDefault(p => p.Name == "NameEn");

			if (nameArProperty != null && nameEnProperty != null)
			{
				var nameEnValue = nameEnProperty.GetValue(model) as string;
				var nameArValue = nameArProperty.GetValue(model) as string;

				// Set "NameEn" to "NameAr" if "NameEn" is null or empty
				if (string.IsNullOrEmpty(nameEnValue))
				{
					nameEnProperty.SetValue(model, nameArValue);
				}
			}

			#endregion

			await _repo.UpdateAsync(model);

			_unitOfwork.Save();
			return model;

		}
	}
}