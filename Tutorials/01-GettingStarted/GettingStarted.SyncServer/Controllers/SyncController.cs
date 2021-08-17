using BIT.Data.Functions;
using BIT.Data.Services;
using BIT.Data.Sync;
using BIT.Xpo.Providers.OfflineDataSync;
using BIT.Xpo.Providers.OfflineDataSync.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GettingStarted.SyncServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncController : SyncControllerBase
    {

        public SyncController(IObjectSerializationService objectSerializationService, ISyncDataStore MasterDS) : base(objectSerializationService, MasterDS)
        {

        }

        public async override Task<IDataResult> Post()
        {
            return await base.Post();
        }

        protected override IDataResult Process(IDataParameters parameters)
        {

            return base.Process(parameters);
        }

    
        public override Task<string> Get()
        {
            return base.Get();
        }



    }
}
