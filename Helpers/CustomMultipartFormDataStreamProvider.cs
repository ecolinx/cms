using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace EcolinxCMS.Api.Helpers
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path)
        { }
        
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "";

            //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            return name.Replace("\"", string.Empty); 
        }
    }

    public class GuidMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private Guid _guid;

        public GuidMultipartFormDataStreamProvider(string path) : base(path)
        {
            this._guid = Guid.NewGuid();
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "";
            name = name.Replace("\"", string.Empty); 
            var extension = name.Substring(name.LastIndexOf(@"."));
            var guid = this._guid.ToString();


            //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            return string.Format("{0}{1}", guid, extension);
        }
    }
}