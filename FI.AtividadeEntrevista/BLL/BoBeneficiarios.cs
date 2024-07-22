using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiarios
    {
      
        public long Incluir(DML.Beneficiarios beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Incluir(beneficiarios);
        }

       
        public void Alterar(DML.Beneficiarios beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            ben.Alterar(beneficiarios);
        }

        
        public DML.Beneficiarios Consultar(long id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Consultar(id);
        }

       
        public void Excluir(long id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            ben.Excluir(id);
        }

       
        public List<DML.Beneficiarios> Listar()
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Listar();
        }

        public List<DML.Beneficiarios> Pesquisa(int idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            return cli.Pesquisa(idCliente, iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

       
        public bool VerificarExistencia1(string CPF)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            return cli.VerificarExistencia(CPF);
        }



    }
}
