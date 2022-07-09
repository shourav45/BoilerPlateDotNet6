using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{

    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class VersioningController : ControllerBase
    {
        private readonly TodoService service;

        public VersioningController()
        {
            this.service = new TodoService();
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("~/get/to/do/item/for/version")]
        public async Task<IActionResult> getTodoItemList()
        {
            try
            {
                return getResponse(await service.getTodoMasterList());
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("~/get/to/do/item/for/version")]
        public async Task<IActionResult> getTodoItemListForAnotherVersion()
        {
            try
            {
                return getResponse(await service.getTodoMasterList());
            }
            catch (Exception ex) { return getResponse(ex); }
        }
    }
}
