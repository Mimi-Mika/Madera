using SQLiteSyncCOMCsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Synchro
{
    static class Synchronisation
    {
        private const string wsUrl = "http://localhost:8080/SqliteSync_315/API3/";
        private const string connString = @"Data Source=|DataDirectory|\bdd_madera.db;";

        public static void ReinitDB(string sub)
        {
            SQLiteSyncCOMClient sqlite = new SQLiteSyncCOMClient(connString, wsUrl);
            sqlite.ReinitializeDatabase(sub);
        }

        public static void SendAndReceiveDB(string sub)
        {
            SQLiteSyncCOMClient sqlite = new SQLiteSyncCOMClient(connString, wsUrl);
            sqlite.SendAndRecieveChanges(sub);
        }

    }
}
