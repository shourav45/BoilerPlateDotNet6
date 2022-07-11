using DTO;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace API.Controllers
{
    public class ControllerBase : Controller
    {
        protected ActionResult getResponse(object Data, string Message = "")
        {
            CommonResponseDto response = new CommonResponseDto();
            response.Status = "OK";
            response.Message = Message;
            response.Data = Data;

            var result = new JSONSerialize().getJSONString(response, true);
            //SAVE DATA/API CALL LOG IF NESSAGERY
            return Ok(result);
        }

        protected ActionResult getResponse(Exception ex)
        {
            CommonResponseDto response = new CommonResponseDto();
            response.Status = "ERROR";
            response.Message = getAllErrorString(ex);
            response.Data = ex;

            // SAVE LOG
            //try
            //{
            //    saveLog(response.Message);
            //}
            //catch { }

            return BadRequest(new JSONSerialize().getJSONString(response, true));
        }

        public static string getAllErrorString(Exception ex)
        {
            try
            {
                string Message = "";

                if (ex.InnerException != null)
                {
                    var iEx = ex.InnerException;
                    if (!string.IsNullOrEmpty(iEx.Message))
                    {
                        Message += iEx.Message;
                    }
                }

                return Message;
            }
            catch { return ""; }
        }

    }
}
