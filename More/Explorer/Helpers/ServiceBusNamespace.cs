#region Copyright
//=======================================================================================
// Microsoft Business Platform Division Customer Advisory Team  
//
// This sample is supplemental to the technical guidance published on the community
// blog at http://www.appfabriccat.com/. 
// 
// Author: Paolo Salvatori
//=======================================================================================
// Copyright © 2011 Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
//=======================================================================================
#endregion

#region Using Directives



#endregion

using System;
using Microsoft.ServiceBus;

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    public enum ServiceBusNamespaceType
    {
        Custom,
        Cloud,
        OnPremises
    }

    /// <summary>
    /// This class represents a service bus namespace address and authentication credentials
    /// </summary>
    public class ServiceBusNamespace
    {
        #region Public Constructors
        /// <summary>
        /// Initializes a new instance of the ServiceBusHelper class.
        /// </summary>
        public ServiceBusNamespace()
        {
            ConnectionStringType = ServiceBusNamespaceType.Cloud;
            ConnectionString = default(string);
            Uri = default(string);
            Namespace = default(string);
            IssuerName = default(string);
            IssuerSecret = default(string);
            ServicePath = default(string);
            StsEndpoint = default(string);
            RuntimePort = default(string);
            ManagementPort = default(string);
            WindowsDomain = default(string);
            WindowsUserName = default(string);
            WindowsPassword = default(string);
        }

        /// <summary>
        /// Initializes a new instance of the ServiceBusNamespace class.
        /// </summary>
        /// <param name="connectionStringType">The service bus namespace connection string type.</param>
        /// <param name="connectionString">The service bus namespace connection string.</param>
        /// <param name="uri">The full address of the service bus namespace.</param>
        /// <param name="ns">The service bus namespace.</param>
        /// <param name="issuerName">The issuer name of the shared secret credentials.</param>
        /// <param name="issuerSecret">The issuer secret of the shared secret credentials.</param>
        /// <param name="servicePath">The service path that follows the host name section of the URI.</param>
        /// <param name="transportType">The transport type to use to access the namespace.</param>
        public ServiceBusNamespace(ServiceBusNamespaceType connectionStringType,
                                   string connectionString,
                                   string uri,
                                   string ns,
                                   string servicePath,
                                   string issuerName,
                                   string issuerSecret,
                                   string transportType)
        {
            ConnectionStringType = connectionStringType;
            ConnectionString = connectionString;
            Uri = uri;
            if (string.IsNullOrEmpty(uri))
            {
                Uri = ServiceBusEnvironment.CreateServiceUri(transportType, ns, servicePath).ToString();
            }
            Namespace = ns;
            IssuerName = issuerName;
            IssuerSecret = issuerSecret;
            ServicePath = servicePath;
            TransportType = transportType;
            StsEndpoint = default(string);
            RuntimePort = default(string);
            ManagementPort = default(string);
            WindowsDomain = default(string);
            WindowsUserName = default(string);
            WindowsPassword = default(string);
        }

        /// <summary>
        /// Initializes a new instance of the ServiceBusNamespace class.
        /// </summary>
        /// <param name="connectionString">The service bus namespace connection string.</param>
        /// <param name="endpoint">The endpoint of the service bus namespace.</param>
        /// <param name="stsEndpoint">The sts endpoint of the service bus namespace.</param>
        /// <param name="runtimePort">The runtime port.</param>
        /// <param name="managementPort">The management port.</param>
        /// <param name="windowsDomain">The Windows domain or machine name.</param>
        /// <param name="windowsUsername">The Windows user name.</param>
        /// <param name="windowsPassword">The Windows user password.</param>
        /// <param name="ns">The service bus namespace.</param>
        public ServiceBusNamespace(string connectionString,
                                   string endpoint,
                                   string stsEndpoint,
                                   string runtimePort,
                                   string managementPort,
                                   string windowsDomain,
                                   string windowsUsername,
                                   string windowsPassword,
                                   string ns)
        {
            ConnectionStringType = ServiceBusNamespaceType.OnPremises;
            ConnectionString = connectionString;
            Uri = endpoint;
            var uri = new Uri(endpoint);
            if (string.IsNullOrEmpty(endpoint))
            {
                Uri = ServiceBusEnvironment.CreateServiceUri(uri.Scheme, ns, null).ToString();
            }
            Namespace = ns;
            IssuerName = default(string);
            IssuerSecret = default(string);
            TransportType = default(string);
            StsEndpoint = stsEndpoint;
            RuntimePort = runtimePort;
            ManagementPort = managementPort;
            WindowsDomain = windowsDomain;
            WindowsUserName = windowsUsername;
            WindowsPassword = windowsPassword;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the service bus namespace type.
        /// </summary>
        public ServiceBusNamespaceType ConnectionStringType { get; set; }

        /// <summary>
        /// Gets or sets the service bus namespace connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the full address of the service bus namespace.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the service bus namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the issuer name of the shared secret credentials.
        /// </summary>
        public string IssuerName { get; set; }

        /// <summary>
        /// Gets or sets the issuer secret of the shared secret credentials.
        /// </summary>
        public string IssuerSecret { get; set; }

        /// <summary>
        /// Gets or sets the service path that follows the host name section of the URI.
        /// </summary>
        public string ServicePath { get; set; }

        /// <summary>
        /// Gets or sets the transport type to use to access the namespace.
        /// </summary>
        public string TransportType { get; set; }

        /// <summary>
        /// Gets or sets the URL of the sts endpoint.
        /// </summary>
        public string StsEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the runtime port.
        /// </summary>
        public string RuntimePort { get; set; }

        /// <summary>
        /// Gets or sets the management port.
        /// </summary>
        public string ManagementPort { get; set; }

        /// <summary>
        /// Gets or sets the windows domain.
        /// </summary>
        public string WindowsDomain { get; set; }

        /// <summary>
        /// Gets or sets the Windows user name.
        /// </summary>
        public string WindowsUserName { get; set; }

        /// <summary>
        /// Gets or sets the Windows user password.
        /// </summary>
        public string WindowsPassword { get; set; }
        #endregion
    }
}
