using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public readonly TodoService service;
        public TodoController()
        {
            this.service = new TodoService();
        }

        [HttpGet]
        [Route("~/get/to/do/item/list")]
        public async Task<IActionResult> getTodoItemList()
        {
            try
            {
               string LoggedinUserName = User.FindFirst("UserName")?.Value;

                return getResponse(await service.getTodoMasterList());
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpPost]
        [Route("~/create/new/to/do/item")]
        public async Task<IActionResult> createNewTodoItem(TodoMaster master)
        {
            try
            {
                //var Files = Request.Form.Files;
                return getResponse(await service.createTodoMaster(master));
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpGet]
        [Route("~/get/to/do/item/by/id/{todoid}")]
        public async Task<IActionResult> getTodoItemByID(int todoid)
        {
            try
            {
                return getResponse(await service.getTodoMasterByID(todoid));
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        
        [HttpGet]
        [Route("~/get/to/do/item/list/by/status/{status}")]
        public async Task<IActionResult> getTodoItemListByStatus(string status)
        {
            try
            {
                return getResponse(await service.getTodoMasterListByStatus(status));
            }
            catch (Exception ex) { return getResponse(ex); }
        }
    }
}
