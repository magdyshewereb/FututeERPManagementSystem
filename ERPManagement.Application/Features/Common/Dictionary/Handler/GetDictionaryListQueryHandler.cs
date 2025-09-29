using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Common.Base.Queries;
using ERPManagement.Application.Features.Common.Dictionary.Models;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;

namespace ERPManagement.Application.Features.Common.Dictionary.Handler
{
    public class GetDictionaryListQueryHandler
        : CachingBaseQueryHandler<SystemDictionary, DictionaryVm>,
        IQuerytHandler<GetDictionaryListQuery, List<DictionaryVm>>
    {
        IWebHostEnvironment _hostEnvironment;
        public GetDictionaryListQueryHandler(IQueryGenericRepository<SystemDictionary> modelRepository,
            IMapper mapper, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment) : base(modelRepository, mapper, memoryCache)
        {

            _hostEnvironment = webHostEnvironment;
        }


        public async Task<List<DictionaryVm>> Handle(GetDictionaryListQuery request, CancellationToken cancellationToken)
        {
            var dictList = await GetListAsync(i => i.Name == request.PropName || request.PropName == null);
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, "StaticDictionaryData", "staticmessage.json");
            StreamReader streamReader = new StreamReader(filePath);
            var staticStr = streamReader.ReadToEnd();
            var staticList = JsonConvert.DeserializeObject<List<DictionaryVm>>(staticStr);
            dictList.AddRange(staticList);
            return dictList;
        }
    }
}
