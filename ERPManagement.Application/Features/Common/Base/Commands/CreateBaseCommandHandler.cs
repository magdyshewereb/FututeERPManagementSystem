using AutoMapper;
using ERPManagement.Application.Contracts.Persistence;
using Hr.Application.Interfaces.GenericRepository.Command;

namespace ERPManagement.Application.Features.Common.Base.Commands
{
	public class CreateBaseCommandHandler<T, TViewModel>
		 where T : class
		where TViewModel : class

	{
		internal readonly IBaseRepository<T> _repo;
		internal readonly IMapper _mapper;
		internal IUnitOfwork _unitOfwork;

		public CreateBaseCommandHandler(IBaseRepository<T> branchRepository, IMapper mapper, IUnitOfwork unitOfwork)
		{
			_repo = branchRepository;
			_mapper = mapper;
			_unitOfwork = unitOfwork;
		}



		internal async Task<T> CreateHandle(TViewModel request, CancellationToken cancellationToken)
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

			await _repo.AddAsync(model);
			var rowsaffected = _unitOfwork.Save();
			return model;

		}
	}
}