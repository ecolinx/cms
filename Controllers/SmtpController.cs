using EcolinxCMS.Business.UnitOfService;
using EcolinxCMS.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EcolinxCMS.Api.Controllers
{
    public class SmtpController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpPost]
        [ActionName("sendgrid")]
        public HttpResponseMessage SendGrid(SmtpHelper smtp)
        {
            var response = new HttpResponseMessage();

            try
            {
                _service.SmtpService.SendEmail(smtp);
                response = Request.CreateResponse(HttpStatusCode.OK, "Email enviado com sucesso");
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
