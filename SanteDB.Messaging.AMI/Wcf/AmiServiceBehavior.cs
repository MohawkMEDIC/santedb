using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using SanteDB.Core.Interop;
using SanteDB.Core.Model.Patch;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.Attributes;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Exceptions;
using SanteDB.Core.Diagnostics;
using System.IO;
using SanteDB.Core.Model.Interfaces;
using System.ServiceModel.Web;
using System.Net;
using System.ServiceModel.Channels;

namespace SanteDB.Messaging.AMI.Wcf
{
    /// <summary>
    /// Implementation of the AMI service behavior
    /// </summary>
    [ServiceBehavior(ConfigurationName = "AMI", InstanceContextMode = InstanceContextMode.PerCall)]
    [Description("Administrative Management Interface")]
    [TraceSource(AmiConstants.TraceSourceName)]
    public class AmiServiceBehavior : IAmiServiceContract
    {

        // The trace source for logging
        private TraceSource m_traceSource = new TraceSource(AmiConstants.TraceSourceName);

        /// <summary>
        /// Creates the specified resource for the AMI service 
        /// </summary>
        /// <param name="resourceType">The type of resource being created</param>
        /// <param name="data">The resource data being created</param>
        /// <returns>The created the data</returns>
        public object Create(string resourceType, object data)
        {
            this.ThrowIfNotReady();

            try
            {

                //object handler = ResourceHandlerUtil.Current.GetResourceHandler(resourceType);
                //if (handler != null)
                //{
                //    var retVal = handler.Create(data, false);

                //    var versioned = retVal as IVersionedEntity;
                //    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
                //    WebOperationContext.Current.OutgoingResponse.ETag = retVal.Tag;
                //    if (versioned != null)
                //        WebOperationContext.Current.OutgoingResponse.Headers.Add(HttpResponseHeader.ContentLocation, String.Format("{0}/{1}/{2}/history/{3}",
                //           WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri,
                //           resourceType,
                //           retVal.Key,
                //           versioned.Key));
                //    else
                //        WebOperationContext.Current.OutgoingResponse.Headers.Add(HttpResponseHeader.ContentLocation, String.Format("{0}/{1}/{2}",
                //            WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri,
                //            resourceType,
                //            retVal.Key));
                //}
                //else
                //    throw new FileNotFoundException(resourceType);
                return null;
            }
            catch (Exception e)
            {
                var remoteEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, String.Format("{0} - {1}", remoteEndpoint?.Address, e.ToString()));
                throw;

            }
        }

        public object CreateUpdate(string resourceType, string key, object data)
        {
            throw new NotImplementedException();
        }

        public object Delete(string resourceType, string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string resourceType, string key)
        {
            throw new NotImplementedException();
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public object GetVersion(string resourceType, string key, string versionKey)
        {
            throw new NotImplementedException();
        }

        public object History(string resourceType, string key)
        {
            throw new NotImplementedException();
        }

        public ServiceOptions Options()
        {
            throw new NotImplementedException();
        }

        public ServiceResourceOptions Options(string resourceType)
        {
            throw new NotImplementedException();
        }

        public void Patch(string resourceType, string key, Patch patch)
        {
            throw new NotImplementedException();
        }

        public object Search(string resourceType)
        {
            throw new NotImplementedException();
        }

        public object Update(string resourceType, string key, object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Service is not ready
        /// </summary>
        private void ThrowIfNotReady()
        {
            if (ApplicationContext.Current.IsRunning)
                throw new DomainStateException();
        }
    }
}
