
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AdoNetProject
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Holder { get; set; }
        public decimal? Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
        public void RetrieveWalletsFromDatabase(SqlConnection conn)
        {
            try
            {
                var sql = "SELECT * FROM WALLETS";

                SqlCommand command = new SqlCommand(sql, conn);

                command.CommandType = CommandType.Text;

                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                Wallet wallet;

                while (reader.Read())
                {
                    wallet = new Wallet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Holder = reader.GetString(reader.GetOrdinal("Holder")),
                        Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
                    };

                    Console.WriteLine(wallet);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void InsertWalletRecord(SqlConnection conn, Wallet walletToInsert)
        {
            try
            {
                var sql = "INSERT INTO WALLETS (Holder, Balance) VALUES (@Holder, @Balance)";

                SqlParameter holderParameter = new SqlParameter
                {
                    ParameterName = "@Holder",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Holder,
                };

                SqlParameter balanceParameter = new SqlParameter
                {
                    ParameterName = "@Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Balance,
                };

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.Add(holderParameter);
                command.Parameters.Add(balanceParameter);

                command.CommandType = CommandType.Text;

                conn.Open();

                if (command.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Wallet for {walletToInsert.Holder} added successfully");
                }
                else
                {
                    Console.WriteLine($"ERROR: Wallet for {walletToInsert.Holder} was not added");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the wallet record: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }



        public void InsertWalletRecordAndReturnId(SqlConnection conn, Wallet walletToInsert)
        {
            try
            {
                var sql = "INSERT INTO WALLETS (Holder, Balance) VALUES (@Holder, @Balance);" +
                          "SELECT CAST(scope_identity() AS int)";

                SqlParameter holderParameter = new SqlParameter
                {
                    ParameterName = "@Holder",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Holder,
                };

                SqlParameter balanceParameter = new SqlParameter
                {
                    ParameterName = "@Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Balance,
                };

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.Add(holderParameter);
                command.Parameters.Add(balanceParameter);

                command.CommandType = CommandType.Text;

                conn.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    walletToInsert.Id = id;
                    Console.WriteLine($"Wallet {walletToInsert} added successfully with ID {walletToInsert.Id}");
                }
                else
                {
                    Console.WriteLine($"ERROR: Wallet {walletToInsert} was not added");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the wallet record: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertWalletRecordUsingStoredProcedure(SqlConnection conn, Wallet walletToInsert)
        {
            try
            {
                SqlCommand command = new SqlCommand("AddWallet", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Holder", walletToInsert.Holder);
                command.Parameters.AddWithValue("@Balance", walletToInsert.Balance);

                conn.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Wallet for {walletToInsert.Holder} added successfully");
                }
                else
                {
                    Console.WriteLine($"ERROR: Wallet for {walletToInsert.Holder} was not added");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the wallet record: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddWalletToDatabase(Wallet walletToInsert, SqlConnection conn)
        {
            try
            {
               // SqlConnection conn = new SqlConnection(configuration.GetSection("constr").Value);

                SqlParameter holderParameter = new SqlParameter
                {
                    ParameterName = "@Holder",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Holder,
                };

                SqlParameter balanceParameter = new SqlParameter
                {
                    ParameterName = "@Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = walletToInsert.Balance,
                };

                SqlCommand command = new SqlCommand("AddWallet", conn);

                command.Parameters.Add(holderParameter);
                command.Parameters.Add(balanceParameter);

                command.CommandType = CommandType.StoredProcedure;

                conn.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Wallet for {walletToInsert.Holder} added successfully");
                }
                else
                {
                    Console.WriteLine($"ERROR: Wallet for {walletToInsert.Holder} was not added");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void UpdateWallet(SqlConnection conn)
        {
            try
            {
             

                var sql = "UPDATE Wallets SET Holder = @Holder, Balance = @Balance WHERE Id = @Id";

                SqlParameter idParameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = 1,
                };
                SqlParameter holderParameter = new SqlParameter
                {
                    ParameterName = "@Holder",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = "Ibrahim",
                };
                SqlParameter balanceParameter = new SqlParameter
                {
                    ParameterName = "@Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = 9000,
                };

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.Add(idParameter);
                command.Parameters.Add(holderParameter);
                command.Parameters.Add(balanceParameter);

                command.CommandType = CommandType.Text;

                conn.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Wallet updated successfully");
                }
                else
                {
                    Console.WriteLine("ERROR: Wallet was not updated");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void DeleteWallet(SqlConnection conn)
        {
            try
            {
              

                var sql = "DELETE FROM Wallets WHERE Id = @Id";

                SqlParameter idParameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = 1,
                };

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.Add(idParameter);

                command.CommandType = CommandType.Text;

                conn.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Wallet deleted successfully");
                }
                else
                {
                    Console.WriteLine("ERROR: Wallet was not deleted");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void RetrieveWallets(SqlConnection conn)
        {
            try
            {
               

                var sql = "SELECT * FROM WALLETS";

                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                conn.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var wallet = new Wallet
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Holder = Convert.ToString(dr["Holder"]),
                        Balance = Convert.ToDecimal(dr["Balance"]),
                    };

                    Console.WriteLine(wallet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void PerformFundsTransfer(SqlConnection conn)
        {
            try
            {
               

                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "UPDATE Wallets Set Balance = Balance - 1000 Where Id = 2";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE Wallets Set Balance = Balance + 1000 Where Id = 3";
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine("Transaction of transfer completed successfully");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Transaction rolled back: {ex.Message}");
                    }
                    catch (Exception rollbackEx)
                    {
                        Console.WriteLine($"Error rolling back transaction: {rollbackEx.Message}");
                    }
                }
                finally
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception closeEx)
                    {
                        Console.WriteLine($"Error closing connection: {closeEx.Message}");
                    }
                }
            }
            catch (Exception outerEx)
            {
                Console.WriteLine($"An error occurred: {outerEx.Message}");
            }
        }

    }
}