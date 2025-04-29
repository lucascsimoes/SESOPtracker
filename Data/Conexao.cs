using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Runtime.CompilerServices;

namespace SCCWEB.Data
{
    public class Conexao
    {
        public static OracleConnection GetConexao()
        {
            OracleConnection? SDO = null;
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json") 
                .Build();

                SDO = new OracleConnection(configuration.GetConnectionString("DataBaseHml"));
                //SDO = new OracleConnection(configuration.GetConnectionString("DataBasePrd"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de Conexão: " + ex.Message);
            }
            return SDO;
        }
    }
}

