using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
   
    internal class DaoBeneficiarios : AcessoDados
    {
  
        
        internal long Incluir(DML.Beneficiarios beneficiarios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiarios.Nome ));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiarios.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBenV2", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

       
        internal DML.Beneficiarios Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben.FirstOrDefault();
        }

        // retornar erro 
        internal bool VerificarExistencia(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaBenef", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal List<Beneficiarios> Pesquisa(int idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm));
            parametros.Add(new System.Data.SqlClient.SqlParameter("quantidade", quantidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao));
            parametros.Add(new System.Data.SqlClient.SqlParameter("crescente", crescente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("idCliente", idCliente));

            DataSet ds = base.Consultar("FI_SP_PesqBenef", parametros);
            List<DML.Beneficiarios> ben= Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return ben;
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        internal List<DML.Beneficiarios> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal void Alterar(DML.Beneficiarios beneficiarios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiarios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiarios.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiarios.IdCliente));

            base.Executar("FI_SP_AltBenef", parametros);
        }


        
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelCliente", parametros);
        }

        private List<DML.Beneficiarios> Converter(DataSet ds)
        {
            List<DML.Beneficiarios> lista = new List<DML.Beneficiarios>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiarios ben = new DML.Beneficiarios();
                    ben.Id = row.Field<long>("Id");                   
                    ben.Nome = row.Field<string>("Nome");   
                    ben.CPF = row.Field<string>("CPF");
                    ben.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(ben);
                }
            }

            return lista;
        }
    }
}
