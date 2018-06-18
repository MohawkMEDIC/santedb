/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: khannan
 * Date: 2017-9-1
 */

using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.Util.CertificateTools;
using SanteDB.Core.Model.AMI.Security;
using SanteDB.Core.Security;
using SanteDB.Core.Security.Attribute;
using SanteDB.Messaging.AMI.Configuration;
using System;
using System.Reflection;
using System.Security.Cryptography.Pkcs;
using System.Security.Permissions;
using System.ServiceModel.Web;
using System.Text;

namespace SanteDB.Messaging.AMI.Wcf
{
	/// <summary>
	/// Represents the administrative contract interface.
	/// </summary>
	public partial class AmiBehavior
	{

		/// <summary>
		/// Deletes a specified certificate.
		/// </summary>
		/// <param name="rawId">The raw identifier.</param>
		/// <param name="reason">The reason the certificate is to be deleted.</param>
		/// <returns>Returns the deletion result.</returns>
		/// <exception cref="System.InvalidOperationException">Cannot revoke an un-issued certificate</exception>
		[PolicyPermission(SecurityAction.Demand, PolicyId = PermissionPolicyIdentifiers.UnrestrictedAdministration)]
		public SubmissionResult DeleteCertificate(string rawId, String strReason)
		{
			// Revoke reason
			var reason = (SanteDB.Core.Model.AMI.Security.RevokeReason)Enum.Parse(typeof(SanteDB.Core.Model.AMI.Security.RevokeReason), strReason);
			int id = Int32.Parse(rawId);
			var result = this.certTool.GetRequestStatus(id);

			if (String.IsNullOrEmpty(result.AuthorityResponse))
				throw new InvalidOperationException("Cannot revoke an un-issued certificate");
			// Now get the serial key
			SignedCms importer = new SignedCms();
			importer.Decode(Convert.FromBase64String(result.AuthorityResponse));

			foreach (var cert in importer.Certificates)
				if (cert.Subject != cert.Issuer)
					this.certTool.RevokeCertificate(cert.SerialNumber, (MARC.Util.CertificateTools.RevokeReason)reason);

			result.Outcome = SubmitOutcome.Revoked;
			result.AuthorityResponse = null;
			return new SubmissionResult(result.Message, result.RequestId, (SubmissionStatus)result.Outcome, result.AuthorityResponse);
		}

		/// <summary>
		/// Gets a specific certificate.
		/// </summary>
		/// <param name="rawId">The raw identifier.</param>
		/// <returns>Returns the certificate.</returns>
		public byte[] GetCertificate(string rawId)
		{
			var id = int.Parse(rawId);

			WebOperationContext.Current.OutgoingResponse.ContentType = "application/x-pkcs12";
			WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", $"attachment; filename=\"crt-{id}.p12\"");

			var result = this.certTool.GetRequestStatus(id);

			return Encoding.UTF8.GetBytes(result.AuthorityResponse);
		}

		public AmiCollection<X509Certificate2Info> GetCertificates()
		{
			var collection = new AmiCollection<X509Certificate2Info>();

			var certs = this.certTool.GetCertificates();

			foreach (var cert in certs)
			{
				collection.CollectionItem.Add(new X509Certificate2Info(cert.Attribute));
			}

			return collection;
		}

		/// <summary>
		/// Gets the certificate revocation list.
		/// </summary>
		/// <returns>Returns the certificate revocation list.</returns>
		public byte[] GetCrl()
		{
			WebOperationContext.Current.OutgoingResponse.ContentType = "application/x-pkcs7-crl";
			WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "attachment; filename=\"SanteDB.crl\"");
			return Encoding.UTF8.GetBytes(this.certTool.GetCRL());
		}

	}
}