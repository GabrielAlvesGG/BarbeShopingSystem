using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BarberShopSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> InsertUserAndUpdate([FromForm] string user, IFormFile file) 
        {
            try
            {
                // Desserializar o JSON do usuário
                User userObj = JsonSerializer.Deserialize<User>(user);

                // Chamar o serviço para salvar
                await _userService.InsertOrUpdateUserAsync(userObj, file);

                return Ok("Usuário cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Erro ao cadastrar usuário.");
            }
        }
    }
}
