using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.DataAccess
{
    public class DbSession
    {
        public static string ConnectionString { get; set; } = "Server=localhost;Database=bookStoreDb;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
