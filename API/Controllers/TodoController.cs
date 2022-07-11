using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoService service;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public TodoController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            this.service = new TodoService();
            _configuration = config;
            _environment = environment;
        }

        [HttpPost]
        [Route("~/file/upload/by/form/data")]
        public async Task<IActionResult> createNewTodoItemWithAttachment(IFormCollection formData, IFormFile AttachFile)
        {
            try
            {
                string UploadPath = _configuration["FileUploadPath:TodoAttachments"];

                foreach (var file in formData.Files)
                {
                    var uploads = Path.Combine(UploadPath);
                    if (file.Length > 0)
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            _ = file.CopyToAsync(fileStream);
                        }
                    }
                }

                return getResponse("File Uploaded Successfully.");
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpGet]
        [Route("~/get/to/do/item/list")] //API CALL RATING CONFIGURED IN PROGRAM.CS
        public async Task<IActionResult> getTodoItemList()
        {
            try
            {
                //ANY WHERE IN API CAN GET THE USER INFORMATIONS FROM USER OBJECT

               //string LoggedinUserName = User.FindFirst("UserName")?.Value;

                return getResponse(await service.getTodoMasterList());
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpPost]
        [Route("~/create/new/to/do/item")]
        public async Task<IActionResult> createNewTodoItem(TodoMasterDTO master)
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
