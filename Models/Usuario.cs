using BarberShopSystem;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;

namespace BarberShopSyste.Models;

public class Usuario
{
    public int id { get; set; }
    public string nome { get; set; }
    public string email { get; set; }

    public string senha { get; set; }

    public TipoUsuarioEnum tipoUsuario { get; set; }
    
    public  DateTime dataCriacao {get ; set;} 
    public  string telefone {get ; set;} 

    public Barber barber { get; set;} 
    public Cliente client { get; set;} 
}

