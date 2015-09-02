using EcolinxCMS.Api.Helpers;
using EcolinxCMS.Business.UnitOfService;
using EcolinxCMS.Domain.Helpers;
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
    public class PaginaController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.Get();
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listbypai")]
        public HttpResponseMessage GetByPai(int idPai)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.GetByPai(idPai);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listbytipo")]
        public HttpResponseMessage GetByTipo(int idTipo)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.GetByTipo(idTipo);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listbypublicado")]
        public HttpResponseMessage GetByPublicado(bool isPublicado)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.GetByPublicado(isPublicado);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listbyativo")]
        public HttpResponseMessage GetByAtivo(bool isAtivo)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.GetByAtivo(isAtivo);
                response = Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listbypublico")]
        public HttpResponseMessage GetByPublico(bool isPublico)
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Pagina.GetByPublico(isPublico);
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
                var item = _service.Pagina.Get(id);
                response = Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("iteminicial")]
        public HttpResponseMessage GetInicial()
        {
            var response = new HttpResponseMessage();

            try
            {
                var item = _service.Pagina.GetInicial();
                response = Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [ActionName("listimages")]
        public HttpResponseMessage GetImages() 
        {
            var response = new HttpResponseMessage();

            try
            {
                var path = HttpContext.Current.Server.MapPath("~/Content/images/site/pagina");
                var imagePath = @"../Content/images/site/pagina";
                var thumbPath = @"../Content/images/site/pagina/thumbs";

                var lista = new List<FileHelper>();

                //faz loop listando as imagens
                string[] filePaths = Directory.GetFiles(path);
                foreach (var item in filePaths)
                {
                    //informacoes do arquivo
                    var file = new FileInfo(item);

                    //adiciona na lista
                    lista.Add(new FileHelper { 
                        Filename = file.Name,
                        Path = imagePath + "/" + file.Name,
                        Thumb = thumbPath + "/" + file.Name
                    });
                }

                //cria response
                response = Request.CreateResponse(HttpStatusCode.OK, lista.Select(a => a.Path));
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage Create(Pagina pagina)
        {
            var response = new HttpResponseMessage();

            try
            {
                pagina = _service.Pagina.Create(pagina);
                response = Request.CreateResponse(HttpStatusCode.OK, pagina);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [ActionName("update")]
        public HttpResponseMessage Update(Pagina pagina)
        {
            var response = new HttpResponseMessage();

            try
            {
                pagina = _service.Pagina.Update(pagina);
                response = Request.CreateResponse(HttpStatusCode.OK, pagina);
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
                var pagina = _service.Pagina.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK, pagina);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [ActionName("upload")]
        public Task<FroalaUploadHelper> Upload()
        {
            try
            {
                //cria a parta obra
                if (Request.Content.IsMimeMultipartContent())
                {
                    var pathImagem = HttpContext.Current.Server.MapPath("~/Content/images/site/pagina");
                    var pathThumb = HttpContext.Current.Server.MapPath("~/Content/images/site/pagina/thumbs");
                    var imagePath = @"../Content/images/site/pagina";
                    var streamProvider = new CustomMultipartFormDataStreamProvider(pathImagem);

                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<FroalaUploadHelper>(t =>
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
                            KalikoImage thumb = image.Scale(new CropScaling(150, 150));

                            image.BackgroundColor = Color.White;
                            thumb.SaveJpg(Path.Combine(pathThumb, fileName), 99);

                            //dispose das imagens
                            thumb.Dispose();
                            image.Dispose();
                        }

                        return new FroalaUploadHelper { 
                            link = imagePath + "/" + fileName
                        };
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

        [HttpPost]
        [ActionName("deleteimage")]
        public HttpResponseMessage DeleteImage(FroalaDeleteHelper imagem)
        {
            var response = new HttpResponseMessage();

            try
            {
                var pathImagem = HttpContext.Current.Server.MapPath("~/Content/images/site/pagina");
                var pathThumb = HttpContext.Current.Server.MapPath("~/Content/images/site/pagina/thumbs");
                var filename = imagem.src.Substring(imagem.src.LastIndexOf(@"/") + 1);

                System.IO.File.Delete(pathImagem + @"/" + filename);
                System.IO.File.Delete(pathThumb + @"/" + filename);

                response = Request.CreateResponse(HttpStatusCode.OK, imagem);
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
