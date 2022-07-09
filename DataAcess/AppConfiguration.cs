using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class AppConfiguration
    {
        public readonly IConfiguration _Configuration;
        AppConfiguration(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }

        public string getConnectionString()
        {
            string connectionString;
            connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this._Configuration, "CONN");
           // return _Configuration.GetConnectionString("CONN");
           return connectionString;
        }
    }
}
