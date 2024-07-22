using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            BoBeneficiarios bo = new BoBeneficiarios();

            if (!CpfValidator.IsValid(model.CPF))
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "CPF inválido." });
            }



            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(new { success = false, message = string.Join(Environment.NewLine, erros) });
            }
            else
            {
                if (bo.VerificarExistencia1(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "O CPF já está em uso." });
                }


                model.Id = bo.Incluir(new Beneficiarios()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IdCliente = model.IdCliente
                });

                return Json(new { success = true, message = "Cadastro efetuado com sucesso" });
            }
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            BoBeneficiarios bo = new BoBeneficiarios();

            if (!CpfValidator.IsValid(model.CPF))
            {
                return Json(new { success = false, message = "CPF inválido" });
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                var existingBenef = bo.Consultar(model.Id);
                if (existingBenef.CPF != model.CPF && bo.VerificarExistencia1(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json("O CPF já está em uso.");
                }

                bo.Alterar(new Beneficiarios()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IdCliente = model.IdCliente
                });



                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoBeneficiarios bo = new BoBeneficiarios();
            Beneficiarios beneficiarios = bo.Consultar(id);
            Models.BeneficiarioModel model = null;

            if (beneficiarios != null)
            {
                model = new BeneficiarioModel()
                {
                    Id = beneficiarios.Id,                  
                    Nome = beneficiarios.Nome,
                    CPF = beneficiarios.CPF,
                    IdCliente = beneficiarios.IdCliente
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult BeneficiarioList(int idCliente, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Beneficiarios> beneficiarios = new BoBeneficiarios().Pesquisa(idCliente, jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                
                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}