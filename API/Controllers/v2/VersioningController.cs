using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers.v2
{


    [ApiVersion("1.0")]
    [ApiController]
    public class VersioningController : ControllerBase
    {
        private readonly TodoService service;

        public VersioningController()
        {
            this.service = new TodoService();
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("~/get/to/do/item/for/version")]
        public async Task<IActionResult> getTodoItemList()
        {
            try
            {
                return getResponse(await service.getTodoMasterListByStatus("New"));
            }
            catch (Exception ex) { return getResponse(ex); }
        }
    }
}
