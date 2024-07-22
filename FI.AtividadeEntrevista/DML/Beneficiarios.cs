using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    public class Beneficiarios
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }   

        /// <summary>
        /// CPF
        /// </summary>
        public string CPF { get; set; }

        ///<summary>
        /// IdCliente
        /// </summary>
        public long IdCliente { get; set; }

        /// <summary>
        /// Cliente
        /// </summary>
        public Cliente Cliente {  get; set; }   
    }
}
