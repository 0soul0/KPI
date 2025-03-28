namespace ReviewWebsite.Middleware
{
    public class AuthMiddleware
    {

        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            //if (context.Request.Cookies["token"] == null) {
            //    context.Response.Redirect("/Error");
            //    return;
            //}

            // 排除登入頁面和靜態資源
            //if (context.Request.Path.StartsWithSegments("/Home/Index"))
            //{
            //    await _next(context);
            //    return;
            //}


            // 檢查用戶是否已經驗證
            //if (!context.User.Identity.IsAuthenticated)
            //{
            //    context.Response.Redirect("/Home/Index");
            //    return;
            //}

            // 繼續執行下一個 Middleware
            await _next(context);
        }

    }
}
