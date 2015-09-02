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
    public class PortifolioController : ApiController
    {
        private IUnitOfService _service = new UnitOfService();

        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                var lista = _service.Portifolio.Get();
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
                var item = _service.Portifolio.Get(id);
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
                var path = HttpContext.Current.Server.MapPath("~/Content/images/site/portifolio");
                var imagePath = @"../Content/images/site/portifolio";
                var thumbPath = @"../Content/images/site/portifolio/thumbs";

                var lista = new List<FileHelper>();

                //faz loop listando as imagens
                string[] filePaths = Directory.GetFiles(path);
                foreach (var item in filePaths)
                {
                    //informacoes do arquivo
                    var file = new FileInfo(item);

                    //adiciona na lista
                    lista.Add(new FileHelper
                    {
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
        public HttpResponseMessage Create(Portifolio portifolio)
        {
            var response = new HttpResponseMessage();

            try
            {
                portifolio = _service.Portifolio.Create(portifolio);
                response = Request.CreateResponse(HttpStatusCode.OK, portifolio);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpPut]
        [ActionName("update")]
        public HttpResponseMessage Update(Portifolio portifolio)
        {
            var response = new HttpResponseMessage();

            try
            {
                portifolio = _service.Portifolio.Update(portifolio);
                response = Request.CreateResponse(HttpStatusCode.OK, portifolio);
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
                var portifolio = _service.Portifolio.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK, portifolio);
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
                    var pathImagem = HttpContext.Current.Server.MapPath("~/Content/images/site/portifolio");
                    var pathThumb = HttpContext.Current.Server.MapPath("~/Content/images/site/portifolio/thumbs");
                    var imagePath = @"../Content/images/site/portifolio";
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

                        return new FroalaUploadHelper
                        {
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
                var pathImagem = HttpContext.Current.Server.MapPath("~/Content/images/site/portifolio");
                var pathThumb = HttpContext.Current.Server.MapPath("~/Content/images/site/portifolio/thumbs");
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
