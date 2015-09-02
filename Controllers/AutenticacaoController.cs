using EcolinxCMS.Business.UnitOfService;
using EcolinxCMS.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EcolinxCMS.Api.Controllers
{
    public class AutenticacaoController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login(Autenticacao autenticacao)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Autenticacao.Authenticate(autenticacao.Email, autenticacao.Senha);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}
