using EcolinxCMS.Business.UnitOfService;
using EcolinxCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EcolinxCMS.Api.Controllers
{
    public class PortifolioItemImagemController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage Get(int id)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.PortifolioItemImagem.Get(id);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("item")]
        public HttpResponseMessage GetById(int id)
        {
            var response = new HttpResponseMessage();

            try
            {
                var item = _service.PortifolioItemImagem.GetById(id);
                response = Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage Create(PortifolioItemImagem itemimagem)
        {
            var response = new HttpResponseMessage();

            try
            {
                itemimagem = _service.PortifolioItemImagem.Create(itemimagem);
                response = Request.CreateResponse(HttpStatusCode.OK, itemimagem);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage Delete(int id)
        {
            var response = new HttpResponseMessage();

            try
            {
                var itemimagem = _service.PortifolioItemImagem.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK, itemimagem);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}
