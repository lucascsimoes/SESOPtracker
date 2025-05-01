using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using SCCWEB.Data;
using SESOPtracker.Data;
using SESOPtracker.Models.Entities;
using SQLitePCL;

namespace SESOPtracker.Controllers
{
    public class PermissoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public static int codpess { get; set; } = 3307;
        public static int codpess { get; set; } = 70894;

        public static Permissao GetPermissao()
        {
            int permissao = 1;
            string grupo = "";

            try
            {
                using (OracleConnection conn = Conexao.GetConexao())
                {
                    string sql = "SELECT * FROM ADMINISTRADORES_ESAJ WHERE codpess = :codpess";
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("codpess", codpess));
                        conn.Open();

                        using (OracleDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                permissao = rd["permPatrimonio"] == DBNull.Value ? 1 : Convert.ToInt32(rd["permPatrimonio"]);
                                grupo = rd["grupo"] == DBNull.Value ? "" : rd["grupo"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro de SQL: " + ex.Message);
            }

            return new Permissao
            {
                codpess = codpess,
                grupo = grupo!.ToString(),
                permPatrimonio = Convert.ToInt32(permissao)
            };
        }
    }
}
