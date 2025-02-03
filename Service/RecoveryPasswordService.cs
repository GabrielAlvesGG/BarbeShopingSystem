using BarberShopSyste.Models;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;

namespace BarberShopSystem.Service;

public class RecoveryPasswordService
{
    private readonly EmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RecoveryPasswordService( EmailService emailService, IHttpContextAccessor httpContextAccessor)
    {
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }


    public Usuario LoginConfirm(string login)
    {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            UserRepository loginRepository = new UserRepository(configuration);
            Usuario user = new Usuario();
            user.email = login;
            return  loginRepository.GetUser(user);

        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
    }

    public bool SendCodConfirm(string login) {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            ProcessingConfirmationCode(login, configuration);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private async Task ProcessingConfirmationCode(string login, IConfiguration configuration)
    {
        var resetToken = new RecoveryPassword
        {
            email = login,
            token = CreateToken(),
            expiration = DateTime.UtcNow.AddMinutes(30),
            used = false
        };

        new RecoveryPasswordRepository(configuration).SaveRecoveryRequest(resetToken);
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
        {
            throw new InvalidOperationException("HttpContext não está disponível.");
        }

        string baseUrl = $"{request.Scheme}://{request.Host}";
        string resetLink =  $"{baseUrl}/Login/RecoveryPassword?token={resetToken.token}";

        // Envia o e-mail com o link de recuperação
        string subject = "Recuperação de Senha";
        string body = $"Clique no link para redefinir sua senha: <a href='{resetLink}'>Redefinir Senha</a>";
        await _emailService.SendEmailAsync(resetToken.email, subject, body);

    }

    private static string CreateToken()
    {
        return Guid.NewGuid().ToString();
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            return new RecoveryPasswordRepository(configuration).ValidateToken(token);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void ResetPasswored(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            Usuario userResetPassword = new RecoveryPasswordRepository(configuration).FindUserByResetToken(resetPasswordDto.token);
            userResetPassword.senha = resetPasswordDto.password;
            new UserRepository(configuration).UpdatePassword(userResetPassword);
            new RecoveryPasswordRepository(configuration).UpdateTokenUsed(resetPasswordDto.token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
