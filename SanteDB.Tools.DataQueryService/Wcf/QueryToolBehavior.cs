using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Tools.DataQueryService.Wcf
{
    /// <summary>
    /// Query tool behavior
    /// </summary>
    [ServiceBehavior(ConfigurationName = "QueryBuilderTool")]
    public class QueryToolBehavior : IQueryToolContract
    {

        private TraceSource m_traceSource = new TraceSource("SanteDB.Tools.DataQueryService");

        /// <summary>
        /// Get static content
        /// </summary>
        public Stream StaticContent(string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                    content = "index.html";

                string filename = content.Contains("?")
                    ? content.Substring(0, content.IndexOf("?", StringComparison.Ordinal))
                    : content;

                // Get the query tool stream
                var contentPath = Path.Combine(Path.GetDirectoryName(typeof(QueryToolBehavior).Assembly.Location), "QueryTool", filename);

                if (!File.Exists(contentPath))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                    return null;
                }
                else
                {

                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    WebOperationContext.Current.OutgoingResponse.ContentLength = new FileInfo(contentPath).Length;
                    WebOperationContext.Current.OutgoingResponse.ContentType = this.GetContentType(contentPath);

                    return File.OpenRead(contentPath);
                }
            }
            catch(Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                return null;
            }

        }

        /// <summary>
        /// Get the content type of the file
        /// </summary>
        private string GetContentType(string filename)
        {


            string extension = Path.GetExtension(filename);
            switch (extension.Substring(1).ToLower())
            {
                case "htm":
                case "html":
                    return "text/html";
                case "js":
                    return "application/javascript";
                case "css":
                    return "text/css";
                case "svg":
                    return "image/svg+xml";
                case "ttf":
                    return "application/x-font-ttf";
                case "eot":
                    return "application/vnd.ms-fontobject";
                case "woff":
                    return "application/font-woff";
                case "woff2":
                    return "application/font-woff2";
                case "gif":
                    return "image/gif";
                case "ico":
                    return "image/x-icon";
                case "png":
                    return "image/png";
                default:
                    return "application/x-octet-stream";
            }

        }
    }
}
