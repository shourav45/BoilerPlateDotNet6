using Microsoft.AspNetCore.Mvc;
using Model.DTO;
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

                // Source
                //if (!string.IsNullOrEmpty(ex.Source)) Message = "Source -> " + ex.Source + "\r\n";

                // Message
                //if (!string.IsNullOrEmpty(ex.Message)) Message += ex.Message + "\r\n";

                if (ex.InnerException != null)
                {
                    var iEx = ex.InnerException;
                    if (!string.IsNullOrEmpty(iEx.Message))
                    {
                        //if (!string.IsNullOrEmpty(iEx.Source)) Message += "Source -> " + iEx.Source + "\r\n";
                        Message += iEx.Message;
                    }
                }

                return Message;
            }
            catch { return ""; }
        }
    }
}
