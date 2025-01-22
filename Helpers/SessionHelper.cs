using Microsoft.AspNetCore.Http;

namespace BarberShopSystem.Helpers
{
    public static class SessionHelper
    {
        private static ISession Session => GetHttpContext().Session;

        // Recupera o HttpContext atual
        private static HttpContext GetHttpContext()
        {
            var httpContextAccessor = new HttpContextAccessor();
            return httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");
        }

        // Propriedade para o UserId
        public static int? UserId
        {
            get => Session.GetInt32("Id");
            set => Session.SetInt32("Id", value ?? 0); // Define como 0 se for nulo
        }

        // Propriedade para o UserName
        public static string UserName
        {
            get => Session.GetString("Name");
            set => Session.SetString("Name", value);
        }

        // Método para limpar a sessão
        public static void ClearSession()
        {
            Session.Clear();
        }
        public static bool IsUserLoggedIn()
        {
            return SessionHelper.UserId != 0 && SessionHelper.UserId != null;
        }
    }
}
