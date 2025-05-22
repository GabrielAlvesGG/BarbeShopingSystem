using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;

namespace BarberShopSystem.Service;

public class RecoveryPasswordService : IRecoveryPasswordService
{
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserRepository _userRepository;
    private readonly RecoveryPasswordRepository _recoveryPasswordRepository;

    public RecoveryPasswordService( IEmailService emailService,
                                    IHttpContextAccessor httpContextAccessor,
                                    UserRepository userRepository,
                                    RecoveryPasswordRepository recoveryPasswordRepository)
    {
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _recoveryPasswordRepository = recoveryPasswordRepository;
    }


    public User LoginConfirm(string login)
    {
        try
        {
            User user = new User();
            user.email = login;
            return _userRepository.GetUser(user);
        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
    }

    public bool SendCodConfirm(User user) {
        try
        {
            ProcessingConfirmationCode(user);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private async Task ProcessingConfirmationCode(User user)
    {
        try
        {
            var resetToken = new RecoveryPassword
            {
                email = user.email,
                token = CreateToken(),
                expiration = DateTime.UtcNow.AddMinutes(30),
                used = false,
                idUser = user.id
            };

            _recoveryPasswordRepository.SaveRecoveryRequest(resetToken);
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                throw new InvalidOperationException("HttpContext não está disponível.");
            }

            string baseUrl = $"{request.Scheme}://{request.Host}";
            string resetLink = $"{baseUrl}/Login/RecoveryPassword?token={resetToken.token}";

            // Envia o e-mail com o link de recuperação
            string subject = "Recuperação de Senha";
            string body = $"Clique no link para redefinir sua senha: <a href='{resetLink}'>Redefinir Senha</a>";
            await _emailService.SendEmailAsync(resetToken.email, subject, body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private string CreateToken()
    {
        return Guid.NewGuid().ToString();
    }

    public bool ValidateToken(string token)
    {
        try
        {
            return _recoveryPasswordRepository.ValidateToken(token);
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
            User userResetPassword = _recoveryPasswordRepository.FindUserByResetToken(resetPasswordDto.token);
            userResetPassword.senha = resetPasswordDto.password;
            _userRepository.UpdatePassword(userResetPassword);
            _recoveryPasswordRepository.UpdateTokenUsed(resetPasswordDto.token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
