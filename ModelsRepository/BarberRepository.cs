﻿using BarberShopSystem.Data; // Adicione esta linha, substitua pelo namespace correto
using MySql.Data.MySqlClient;
using BarberShopSystem.ModelsRepository;
using BarberShopSystem.Models;
using System.Threading;

namespace BarberShopSystem.ModelsRepository
{
    public class BarberRepository : DataBaseRepository // Corrija o nome do tipo
    {
        public BarberRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public void InsertOrUpdateBarber(Barber barber)
        {
            try
            {

                barber.disponibilidade = new Barber().disponibilidade;
                MySqlConnection connection = GetConnection();
                connection.Open();
                string sqlCommand = string.Empty;
                if (barber.id == 0)
                    sqlCommand = $"Insert into Barbeiros ( UsuarioId, Especialidades, Disponibilidade) VALUES ( '{barber.usuarioId}', '{barber.especialidade}','{barber.disponibilidade}')";
                else
                    sqlCommand = $"UPDATE Barbeiros SET UsuarioId='{barber.usuarioId}', Especialidades='{barber.especialidade}', Disponibilidade='{barber.disponibilidade}' WHERE Id={barber.id};";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        

        //public Barber GetBarber(int idOldBarber)
        //{
        //    try
        //    {

        //        MySqlConnection connection = GetConnection();
        //        connection.Open();

        //        var command = new MySqlCommand($"SELECT * FROM Barber WHERE id={idOldBarber}", connection);
        //        var reader = command.ExecuteReader();

        //        Barber Barber = new Barber();

        //        while (reader.Read())
        //        {
        //            Barber = new Barber
        //            {
        //                Id = reader.GetInt32("Id"),
        //                Name = reader.GetString("Name"),
        //                Email = reader.GetString("Email"),
        //                cpf = reader.GetString("CpfCnpj"),
        //                DateOfBirth = reader.GetDateTime("DateOfBirth"),
        //                PassWord = reader.GetString("PassWord"),
        //                Phone = reader.GetString("Phone")
        //            };
        //        }
        //        connection.Close();
        //        return Barber;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}

        public void DeleteBarber(int idBarber)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();

                string sqlCommand = string.Empty;
              
                sqlCommand = $"DELETE FROM Barber WHERE Id={idBarber}";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
} 
