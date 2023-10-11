using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace AdoNetProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                               .AddJsonFile(@"C:\Users\30698\source\Advanced\DapperORMProject\DapperORMProject\appsettings.json")
                               .Build();
            SqlConnection conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));


            Wallet wallet = new Wallet();
           
            

            var walletToInsert = new Wallet
            {
                Holder = "Ibrahim Omar",
                Balance =  20000
            };

            //wallet.RetrieveWalletsFromDatabase(conn);

            //wallet.InsertWalletRecord(conn, walletToInsert);

            //  wallet.InsertWalletRecordAndReturnId(conn, walletToInsert);

            // wallet.InsertWalletRecordUsingStoredProcedure(conn, walletToInsert);

            //  wallet.AddWalletToDatabase(wallet, conn); Errors 
            //  wallet.UpdateWallet(conn);

            //wallet.DeleteWallet(conn);

            // wallet.RetrieveWallets(conn);

            // wallet.PerformFundsTransfer(conn);
        }
    }
}
