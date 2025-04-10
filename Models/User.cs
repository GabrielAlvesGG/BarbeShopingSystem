using BarberShopSystem;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;

namespace BarberShopSystem.Models;

public class User
{
    public int id { get; set; }
    public string nome { get; set; }
    public string email { get; set; }

    public string senha { get; set; }

    public TipoUsuarioEnum tipoUsuario { get; set; }
    
    public  DateTime dataCriacao {get ; set;} 
    public  string telefone {get ; set;}

    public Barber barber { get; set; } = new Barber();
    public Cliente client { get; set; } = new Cliente();
}

