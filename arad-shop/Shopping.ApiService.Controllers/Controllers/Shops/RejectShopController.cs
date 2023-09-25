﻿using System.Threading.Tasks;
using System.Web.Http;
using Shopping.Commands.Commands.Persons.Commands.Shop;
using Shopping.Commands.Commands.Persons.Responses;
using Shopping.Infrastructure.Core.CommandBus;

namespace Shopping.ApiService.Controllers.Controllers.Shops
{
    public class RejectShopController : ApiControllerBase
    {
        public RejectShopController(ICommandBus bus) : base(bus)
        {
        }
        [Authorize(Roles = "Support,Admin")]
        public async Task<IHttpActionResult> Put(RejectShopCommand command)
        {
            command.UserId = UserId;
            command.FirstName = FirstName;
            command.LastName = LastName;
            var response = await Bus.Send<RejectShopCommand, RejectShopCommandResponse>(command);
            return Ok(response);
        }
    }
}