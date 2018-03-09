﻿using SanteDB.Core.Alert.Alerting;
using SanteDB.Core.Applets.Model;
using SanteDB.Core.Interop;
using SanteDB.Core.Model.AMI.Alerting;
using SanteDB.Core.Model.AMI.Applet;
using SanteDB.Core.Model.AMI.Auth;
using SanteDB.Core.Model.AMI.BusinessRules;
using SanteDB.Core.Model.AMI.DataTypes;
using SanteDB.Core.Model.AMI.Diagnostics;
using SanteDB.Core.Model.AMI.Logging;
using SanteDB.Core.Model.AMI.Security;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Patch;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SanteDB.Messaging.AMI.Wcf
{
    /// <summary>
    /// Represents a service contract for the AMI 
    /// </summary>
    [ServiceContract(ConfigurationName = "AMI_2.0", Name = "AMI"), XmlSerializerFormat]
    [ServiceKnownType(typeof(Entity))]
    [ServiceKnownType(typeof(ExtensionType))]
    [ServiceKnownType(typeof(AlertMessage))]
    [ServiceKnownType(typeof(AlertMessageInfo))]
    [ServiceKnownType(typeof(SecurityApplication))]
    [ServiceKnownType(typeof(SecurityApplicationInfo))]
    [ServiceKnownType(typeof(TfaRequestInfo))]
    [ServiceKnownType(typeof(SecurityDevice))]
    [ServiceKnownType(typeof(SecurityDeviceInfo))]
    [ServiceKnownType(typeof(SecurityPolicy))]
    [ServiceKnownType(typeof(SecurityPolicyInfo))]
    [ServiceKnownType(typeof(SecurityRole))]
    [ServiceKnownType(typeof(SecurityRoleInfo))]
    [ServiceKnownType(typeof(SecurityUser))]
    [ServiceKnownType(typeof(AuditInfo))]
    [ServiceKnownType(typeof(SecurityUserInfo))]
    [ServiceKnownType(typeof(AppletManifest))]
    [ServiceKnownType(typeof(AppletManifestInfo))]
    [ServiceKnownType(typeof(DeviceEntity))]
    [ServiceKnownType(typeof(DiagnosticApplicationInfo))]
    [ServiceKnownType(typeof(DiagnosticAttachmentInfo))]
    [ServiceKnownType(typeof(DiagnosticBinaryAttachment))]
    [ServiceKnownType(typeof(DiagnosticTextAttachment))]
    [ServiceKnownType(typeof(DiagnosticEnvironmentInfo))]
    [ServiceKnownType(typeof(DiagnosticReport))]
    [ServiceKnownType(typeof(DiagnosticSyncInfo))]
    [ServiceKnownType(typeof(DiagnosticVersionInfo))]
    [ServiceKnownType(typeof(SubmissionInfo))]
    [ServiceKnownType(typeof(SubmissionResult))]
    [ServiceKnownType(typeof(ApplicationEntity))]
    [ServiceKnownType(typeof(SubmissionRequest))]
    [ServiceKnownType(typeof(ServiceOptions))]
    [ServiceKnownType(typeof(X509Certificate2Info))]
    [ServiceKnownType(typeof(AssigningAuthorityInfo))]
    [ServiceKnownType(typeof(CodeSystem))]
    [ServiceKnownType(typeof(LogFileInfo))]
    [ServiceKnownType(typeof(AmiCollection<SubmissionInfo>))]
    [ServiceKnownType(typeof(AmiCollection<ExtensionType>))]
    [ServiceKnownType(typeof(AmiCollection<AppletManifestInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityApplicationInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityDeviceInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityRoleInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityPolicyInfo>))]
    [ServiceKnownType(typeof(AmiCollection<TfaMechanismInfo>))]
    [ServiceKnownType(typeof(AmiCollection<TfaRequestInfo>))]
    [ServiceKnownType(typeof(AmiCollection<BusinessRuleInfo>))]
    [ServiceKnownType(typeof(AmiCollection<AssigningAuthorityInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityDevice>))]
    [ServiceKnownType(typeof(AmiCollection<AlertMessageInfo>))]
    [ServiceKnownType(typeof(AmiCollection<SecurityUserInfo>))]
    [ServiceKnownType(typeof(AmiCollection<LogFileInfo>))]
    [ServiceKnownType(typeof(AmiCollection<CodeSystem>))]
    [ServiceKnownType(typeof(AmiCollection<X509Certificate2Info>))]
    public interface IAmiServiceContract
    {
        /// <summary>
        /// Get the schema for this service
        /// </summary>
        [WebInvoke(Method = "GET", UriTemplate = "/?xsd", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        XmlSchema GetSchema();

        /// <summary>
        /// Creates the specified resource 
        /// </summary>
        /// <param name="resourceType">The type of resource to be created</param>
        /// <param name="data">The resource data to be created</param>
        /// <returns>The stored resource</returns>
        [WebInvoke(Method = "POST", UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        object Create(String resourceType, object data);

        /// <summary>
        /// Creates the specified resource if it does not exist, otherwise updates it
        /// </summary>
        /// <param name="resourceType">The type of resource to be created</param>
        /// <param name="key">The key of the resource </param>
        /// <param name="data">The resource itself</param>
        /// <returns>The updated or created resource</returns>
        [WebInvoke(Method = "POST", UriTemplate = "/{resourceType}/{key}", BodyStyle = WebMessageBodyStyle.Bare)]
        object CreateUpdate(String resourceType, String key, object data);

        /// <summary>
        /// Updates the specified resource
        /// </summary>
        /// <param name="resourceType">The type of resource to be updated</param>
        /// <param name="key">The key of the resource</param>
        /// <param name="data">The resource data to be updated</param>
        /// <returns>The updated resource</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "/{resourceType}/{key}", BodyStyle = WebMessageBodyStyle.Bare)]
        object Update(String resourceType, String key, object data);

        /// <summary>
        /// Deletes the specified resource
        /// </summary>
        /// <param name="resourceType">The type of resource being deleted</param>
        /// <param name="key">The key of the resource being deleted</param>
        /// <returns>The last version of the deleted resource</returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "/{resourceType}/{key}", BodyStyle = WebMessageBodyStyle.Bare)]
        object Delete(String resourceType, String key);

        /// <summary>
        /// Gets the specified resource from the service
        /// </summary>
        /// <param name="resourceType">The type of resource to be fetched</param>
        /// <param name="key">The key of the resource</param>
        /// <returns>The retrieved resource</returns>
        [WebInvoke(Method = "GET", UriTemplate = "/{resourceType}/{key}", BodyStyle = WebMessageBodyStyle.Bare)]
        object Get(String resourceType, String key);

        /// <summary>
        /// Gets the specified versioned copy of the data
        /// </summary>
        /// <param name="resourceType">The type of resource</param>
        /// <param name="key">The key of the resource</param>
        /// <param name="versionKey">The version key to retrieve</param>
        /// <returns>The object as it existed at that version</returns>
        [WebInvoke(Method = "GET", UriTemplate = "/{resourceType}/{key}/history/{versionKey}", BodyStyle = WebMessageBodyStyle.Bare)]
        object GetVersion(String resourceType, String key, String versionKey);

        /// <summary>
        /// Gets a complete history of changes made to the object (if supported)
        /// </summary>
        /// <param name="resourceType">The type of resource</param>
        /// <param name="key">The key of the object to retrieve the history for</param>
        /// <returns>The history</returns>
        [WebInvoke(Method = "GET", UriTemplate = "/{resourceType}/{key}/history", BodyStyle = WebMessageBodyStyle.Bare)]
        object History(String resourceType, String key);

        /// <summary>
        /// Searches the specified resource type for matches
        /// </summary>
        /// <param name="resourceType">The resource type to be searched</param>
        /// <returns>The results of the search</returns>
        [WebInvoke(Method = "GET", UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        object Search(String resourceType);

        /// <summary>
        /// Patches the specified resource with the provided patch
        /// </summary>
        /// <param name="resourceType">The type of resource to be patched</param>
        /// <param name="key">The key of the resource to be patched</param>
        /// <param name="patch">The patch data</param>
        [WebInvoke(Method = "PATCH", UriTemplate = "/{resourceType}/{key}", BodyStyle = WebMessageBodyStyle.Bare)]
        void Patch(String resourceType, String key, Patch patch);

        /// <summary>
        /// Get the service options
        /// </summary>
        /// <returns>The options of the server</returns>
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceOptions Options();

        /// <summary>
        /// Get the specific options supported for the 
        /// </summary>
        /// <param name="resourceType">The type of resource to get service options</param>
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResourceOptions Options(String resourceType);
    }
}
