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
    public class PaginaTipoController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.PaginaTipo.Get();
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
                var item = _service.PaginaTipo.Get(id);
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
        public HttpResponseMessage Create(PaginaTipo paginatipo)
        {
            var response = new HttpResponseMessage();

            try
            {
                paginatipo = _service.PaginaTipo.Create(paginatipo);
                response = Request.CreateResponse(HttpStatusCode.OK, paginatipo);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [ActionName("update")]
        public HttpResponseMessage Update(PaginaTipo paginatipo)
        {
            var response = new HttpResponseMessage();

            try
            {
                paginatipo = _service.PaginaTipo.Update(paginatipo);
                response = Request.CreateResponse(HttpStatusCode.OK, paginatipo);
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
                var paginatipo = _service.PaginaTipo.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK, paginatipo);
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
