﻿using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Frobozz.CapabilityContracts.Gdpr.Logic;
using Frobozz.CapabilityContracts.Gdpr.Model;
using Xlent.Lever.Libraries2.WebApi.Crud.ApiControllers;

namespace Frobozz.GdprConsent.NexusAdapter.WebApi.Controllers
{
    /// <summary>
    /// ApiController for Product that does inputcontrol. Logic is separated into another layer. 
    /// </summary>
    [RoutePrefix("api/Persons")]
    public class PersonsController : CrudApiController<Person>, IPersonService
    {
        private readonly IGdprCapability _logic;

        /// <summary>
        /// Constructor 
        /// </summary>
        public PersonsController(IGdprCapability logic)
        :base(logic.PersonService)
        {
            _logic = logic;
        }

        /// <inheritdoc />
        [HttpGet]
        [Route("FindByName")]
        public async Task<Person> FindFirstOrDefaultByNameAsync(string name, CancellationToken token = default(CancellationToken))
        {
            return await _logic.PersonService.FindFirstOrDefaultByNameAsync(name, token);
        }
    }
}
