﻿using Models;
using MySql.Data.MySqlClient;
using Parameters;

namespace Repositories
{
    public class InvoicesDetailsRepository
    {
        public List<InvoicesDetails> AllInvoicesDetailsRepo()
        {
            var result = new List<InvoicesDetails>();
            try
            {
                using (var connect = new MySqlConnection(ParametersDB.ShopDB))
                {
                    connect.Open();
                    const string queryDb = "SELECT * FROM invoicesDetails";
                    using (var command = new MySqlCommand(queryDb, connect))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new InvoicesDetails
                                {
                                    Id = reader.GetInt32("id"),
                                    InvoiceID = reader.GetInt32("invoiceID"),
                                    ProductID = reader.GetInt32("productID"),
                                    Quantity = reader.GetInt32("quantity"),
                                    Price = reader.GetDecimal("price"),
                                    Iva = reader.GetDecimal("iva"),
                                    SubTotal = reader.GetDecimal("Subtotal"),
                                    Total = reader.GetDecimal("total"),
                                });
                            }
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

        public InvoicesDetails GetInvoicesDetailRepo(int id)
        {
            var result = new InvoicesDetails();
            try
            {
                using (var connect = new MySqlConnection(ParametersDB.ShopDB))
                {
                    connect.Open();
                    const string queryDB = "SELECT * FROM invoicesDetails WHERE id = @id";
                    var command = new MySqlCommand(queryDB, connect);
                    command.Parameters.AddWithValue("id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new InvoicesDetails
                            {
                                Id = reader.GetInt32("id"),
                                InvoiceID = reader.GetInt32("invoiceID"),
                                ProductID = reader.GetInt32("productID"),
                                Quantity = reader.GetInt32("quantity"),
                                Price = reader.GetDecimal("price"),
                                Iva = reader.GetDecimal("iva"),
                                SubTotal = reader.GetDecimal("Subtotal"),
                                Total = reader.GetDecimal("total"),
                            };
                        }
                        else
                        {
                            throw new ArgumentException("No se encuentras datos en la base de datos");
                        }
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

        public List<InvoicesDetails> GetInvoicesDetailByInvoiceIDRepo(int invoiceID)
        {
            var result = new List<InvoicesDetails>();
            try
            {
                using (MySqlConnection connect = new MySqlConnection(ParametersDB.ShopDB))
                {
                    connect.Open();
                    string queryDb = "SELECT * FROM invoicesdetails WHERE invoiceID = @invoiceID";
                    var commandMysql = new MySqlCommand(queryDb, connect);
                    commandMysql.Parameters.AddWithValue("@invoiceID", invoiceID);

                    using (var reader = commandMysql.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new InvoicesDetails
                            {
                                Id = reader.GetInt32("id"),
                                InvoiceID = reader.GetInt32("invoiceID"),
                                ProductID = reader.GetInt32("productID"),
                                Quantity = reader.GetInt32("quantity"),
                                Price = reader.GetDecimal("price"),
                                Iva = reader.GetDecimal("iva"),
                                SubTotal = reader.GetDecimal("Subtotal"),
                                Total = reader.GetDecimal("total"),
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

        public void InsertInvoiceDetailRepo(int invoiceID, List<InvoicesDetails> details, MySqlConnection connect, MySqlTransaction transaction)
        {
            try
            {
                const string queryDB = "INSERT INTO invoicesdetails " +
                    "(invoiceID, productID, quantity, price, iva, subtotal, total) " +
                    "VALUES (@A,@B,@C,@D,@E,@F,@G)";
                foreach (var detail in details)
                {
                    using (var command = new MySqlCommand(queryDB, connect, transaction))
                    {
                        command.Parameters.AddWithValue("@A", invoiceID);
                        command.Parameters.AddWithValue("@B", detail.ProductID);
                        command.Parameters.AddWithValue("@C", detail.Quantity);
                        command.Parameters.AddWithValue("@D", detail.Price);
                        command.Parameters.AddWithValue("@E", detail.Iva);
                        command.Parameters.AddWithValue("@F", detail.SubTotal);
                        command.Parameters.AddWithValue("@G", detail.Total);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inserting invoice detail: {ex.Message}");
                throw;
            }
        }

        public void DeleteInvoiceDetailRepo(int invoiceID, MySqlConnection connect, MySqlTransaction transaction)
        {
            try
            {
                const string queryDB = "Delete FROM invoicesdetails WHERE invoiceID=@invoiceID";
                using (var command = new MySqlCommand(queryDB, connect, transaction))
                {
                    command.Parameters.AddWithValue("@invoiceID", invoiceID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting invoice detail: {ex.Message}");
                throw;
            }
        }
    }
}
