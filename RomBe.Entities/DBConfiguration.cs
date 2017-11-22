using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities
{
    public class DBConfiguration : DbConfiguration
    {
        public DBConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient",() => new SqlAzureExecutionStrategy(2, TimeSpan.FromSeconds(30)));
        }
    }
}
