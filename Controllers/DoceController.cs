using EcolinxCMS.Api.Helpers;
using EcolinxCMS.Business.UnitOfService;
using EcolinxCMS.Domain.Models;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EcolinxCMS.Api.Controllers
{
    public class DoceController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Doce.Get();
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
                var item = _service.Doce.Get(id);
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
        public HttpResponseMessage Create(Doce doce)
        {
            var response = new HttpResponseMessage();

            try
            {
                doce = _service.Doce.Create(doce);
                response = Request.CreateResponse(HttpStatusCode.OK, doce);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [ActionName("update")]
        public HttpResponseMessage Update(Doce doce)
        {
            var response = new HttpResponseMessage();

            try
            {
                doce = _service.Doce.Update(doce);
                response = Request.CreateResponse(HttpStatusCode.OK, doce);
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
                var doce = _service.Doce.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK, doce);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [ActionName("upload")]
        public Task<string> Upload(int id)
        {
            try
            {
                //cria a parta obra
                if (Request.Content.IsMimeMultipartContent())
                {
                    var pathImagem = HttpContext.Current.Server.MapPath("~/Content/images/site/doce");
                    var pathThumb = HttpContext.Current.Server.MapPath("~/Content/images/site/doce/thumbs");
                    var streamProvider = new GuidMultipartFormDataStreamProvider(pathImagem);

                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<string>(t =>
                    {
                        var fileName = "";

                        //verifica se tem imagem
                        if (streamProvider.FileData.Count > 0)
                        {
                            //altera o nome da imagem
                            fileName = streamProvider.GetLocalFileName(streamProvider.FileData[0].Headers);

                            //cria o thumb
                            var combine = Path.Combine(pathImagem, fileName);
                            KalikoImage image = new KalikoImage(combine);
                            KalikoImage thumb = image.Scale(new CropScaling(350, 350));

                            image.BackgroundColor = Color.White;
                            thumb.SaveJpg(Path.Combine(pathThumb, fileName), 99);

                            //dispose das imagens
                            thumb.Dispose();
                            image.Dispose();

                            //verifica se é para atualizar no banco de dados
                            if (id > 0)
                            {
                                var doce = _service.Doce.Get(id);
                                doce.Imagem = fileName;
                                doce.Thumbnail = fileName;
                                _service.Doce.Update(doce);
                            }
                        }

                        return fileName;
                    });

                    return task;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message));
            }

        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}
