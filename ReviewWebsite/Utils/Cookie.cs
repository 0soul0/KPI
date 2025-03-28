
namespace ReviewWebsite.Utils
{
    public class Cookie
    {

        public static string GetCookie(HttpRequest request, string key)
        {
            if (request.Cookies[key] != null) { 
                return request.Cookies[key];
            }
            return "";
            
        }

        public static void SetCookie(HttpResponse reponse, string key, string value)
        {
            reponse.Cookies.Append(key, value);
        }
    }
}
