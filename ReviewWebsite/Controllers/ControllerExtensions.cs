﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewWebsite.Models.ViewModel;

namespace ReviewWebsite.Controllers
{
    public static class ControllerExtensions
    {

        public const int RESPONCE_CODE_200 = 200;
        public const int RESPONCE_CODE_400 = 400;
        public const int RESPONCE_CODE_500 = 500;
        public const int RESPONCE_CODE_501 = 501;

        public static JsonResult ResponseJson(this Controller controller, int code,string data="",string extraData="", string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = code switch
                {
                    RESPONCE_CODE_200 => "",
                    RESPONCE_CODE_400 => "資料錯誤",
                    RESPONCE_CODE_500 => "更新資料失敗,請重新嘗試",
                    RESPONCE_CODE_501 => "資料已經被更新過了,請刷新後再重新編輯",
                    _ => "未知錯誤"
                };
            }

            return new JsonResult(new ResponseViewModel
            {
                Code = code.ToString(),
                Message = message,
                Data = data,
                ExtraData = extraData
            });
        }


        public static IActionResult ResolveView(this Controller controller, string viewName, object? model)
        {
            if (controller.Request.IsAjaxRequest())
            {
                return controller.PartialView($"_{viewName}", model);
            }

            return controller.View(viewName, model);
        }

        private static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static long TotalSeconds(this DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(utcDateTime - epoch).TotalSeconds;
        }

    }
}
