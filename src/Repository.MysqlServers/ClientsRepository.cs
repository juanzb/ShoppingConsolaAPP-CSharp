﻿using Models;
using MySql.Data.MySqlClient;
using Repository.Interfaces;

namespace Repository.MysqlServers
{
    public class ClientsRepository : Repository, IClientsRepository
    {
        public ClientsRepository(MySqlConnection connect, MySqlTransaction transaction) 
        { 
            this._connect = connect;
            this._transaction = transaction;
        }

        public List<Clients> GetAll()
        {
            var result = new List<Clients>();
            try
            {
                const string queryDb = "SELECT * FROM clients";
                using (var command = CreateMySqlCommand(queryDb))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Clients
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error DB: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
            return result;
        }

        public Clients GetById(int id)
        {
            var result = new Clients();
            try
            {
                const string queryDB = "SELECT * FROM clients WHERE id = @id";
                var command = CreateMySqlCommand(queryDB);
                command.Parameters.AddWithValue("id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Clients
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name")
                        };
                    }
                    else
                    {
                        throw new ArgumentException("No se encuentras datos en la base de datos");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error DB: {ex.Message}");
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error de Argumento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
            return result;
        }

        public void Create(Clients client)
        {
            try
            {
                const string queryDB = "INSERT INTO clients (name) VALUES (@A)";
                using (MySqlCommand command = CreateMySqlCommand(queryDB))
                {
                    command.Parameters.AddWithValue("@A", client.Name);
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error DB: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
        }

        public void Update(Clients client)
        {
            try
            {
                const string queryDB = "UPDATE invoices SET clientID=@A, iva=@B, subtotal=@C, total=@D WHERE id=@clientId";
                using (MySqlCommand command = CreateMySqlCommand(queryDB))
                {
                    command.Parameters.AddWithValue("@clientId", client.Id);
                    command.Parameters.AddWithValue("@A", client.Name);
                    var res = command.ExecuteNonQuery();
                    if (res < 1) throw new ArgumentException("No existe el ID del cliente para actualizar el nombre");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error DB: {ex.Message}");
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
        }

        public void Remove(int clientID)
        {
            try
            {
                const string queryDb = "DELETE FROM clients WHERE id=@clientsID";
                using (MySqlCommand command = CreateMySqlCommand(queryDb))
                {
                    command.Parameters.AddWithValue("@invoiceID", clientID);
                    var res = command.ExecuteNonQuery();
                    if (res < 1) throw new ArgumentException("No existe el ID del cliente para actualizar el nombre");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error DB: {ex.Message}");
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado: {ex.Message}");
                throw;
            }
        }

    }
}
