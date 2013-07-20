#region Copyright
//=======================================================================================
//Microsoft Windows Server AppFabric Customer Advisory Team (CAT)  
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace Adglopez.Samples.ServiceBus.Auditing.Sample
{
    public class Program
    {
        #region Private Constants
        //***************************
        // Formats
        //***************************
        private const string Namespace = "WorkflowDefaultNamespace"; // the local service bus name
        private const string QueueName = "ServiceBusQueueSample"; // the queue name
        private const string WindowsDomain = "Penny"; // the domain or machine name
        private const string WindowsUsername = "Adrian"; // the domain or machine user name
        private const string WindowsPassword = "Password11"; // the user password
        private const string MessageText = "Lorem ipsum dolor sit amet"; // The message payload
        private const string MessageLabel = "Service Bus for Windows Server";// the message label
        private const string MethodAction = "GetMessage"; // The method action

        //***************************
        // Formats
        //***************************
        private const string OpenServiceEndpointUriFormat = "The following WCF service endpoint was opened to receive messages from  the [{0}] queue:";
        private const string CreatedProxyFormat = "A WCF client channel was created to send a message to  the [{0}] queue.";
        private const string MessageSuccessfullySent = "Request Message Sent:\n - EndpointUrl:[{0}]\n - CorrelationId=[{1}]\n - MessageId=[{2}]\n - Label=[{3}]";
        private const string ReceivedMessagePropertiesHeader = "Custom Properties:";
        private const string PayloadFormat = "Payload:";
        private const string MessagePropertyFormat = " - Key=[{0}] Value=[{1}]";

        //***************************
        // Constants
        //***************************
        private const string Empty = "EMPTY";
        #endregion

        public static void Main()
        {
            try
            {
                // Set console app window size
                Console.WindowWidth = 100;
                Console.WindowHeight = 50;

                // Machine name
                var machineName = Dns.GetHostEntry(string.Empty).HostName;
                var networkCredential = new NetworkCredential(WindowsUsername, WindowsPassword, WindowsDomain);

                // The TransportClientEndpointBehavior specifies the Service Bus credentials for a particular endpoint
                var stsUris = new List<Uri> { new Uri(string.Format(CultureInfo.InvariantCulture, "sb://{0}:9355/", machineName)) };

                var transportClientEndpointBehavior = new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateOAuthTokenProvider(stsUris, networkCredential)
                };
                

                // Create the URI of the 2 endpoints exposed by the service. 
                // The URI includes your service namespace name and schema type.
                var serviceUri = new Uri(string.Format("sb://{0}/{1}/{2}", machineName, Namespace, QueueName));

                // Instantiate the host with the contract and URI.
                var host = new ServiceHost(typeof(GenericReceiverService), serviceUri);

                // Create two different endpoints, one based on the NetTcpRelayBinding and the other based on the BasicHttpRelayBinding.
                var netMessagingBinding = new NetMessagingBinding
                    {
                        PrefetchCount = 10, 
                        TransportSettings = new NetMessagingTransportSettings
                            {
                                BatchFlushInterval = TimeSpan.FromSeconds(0)
                            }
                    };

                // Add the service endpoints to the service host
                host.AddServiceEndpoint(typeof(IAuditService), netMessagingBinding, string.Empty);

                Console.WriteLine(OpenServiceEndpointUriFormat, QueueName);
                Console.Write(" - ");
                Console.ResetColor();
                Console.WriteLine(host.Description.Endpoints[0].Address.Uri.AbsoluteUri);

                foreach (var endpoint in host.Description.Endpoints)
                {
                    endpoint.Behaviors.Add(transportClientEndpointBehavior);
                }

                host.Open();

                // Create WCF client channel
                var serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof (IAuditService)), netMessagingBinding, new EndpointAddress(serviceUri));
                serviceEndpoint.Behaviors.Add(transportClientEndpointBehavior);
                var channelFactory = new ChannelFactory<IAuditService>(serviceEndpoint);
                var channel = channelFactory.CreateChannel();
                
                Console.WriteLine(CreatedProxyFormat, QueueName);

                // Create request message
                var message = Message.CreateMessage(MessageVersion.Default, MethodAction, MessageText);
                
                // Define BrokeredMessageProperty object as a property of the outgoing WCF message
                using (new OperationContextScope(channel as IContextChannel))
                {
                    var brokeredMessageProperty = new BrokeredMessageProperty
                        {
                            Label = MessageLabel,
                            MessageId = Guid.NewGuid().ToString()
                        };

                    brokeredMessageProperty.Properties.Add("Author", "Paolo Salvatori");
                    brokeredMessageProperty.Properties.Add("Email", "paolos@microsoft.com");
                    brokeredMessageProperty.Properties.Add("Country", "Italy");
                    brokeredMessageProperty.Properties.Add("Priority", 1);

                    OperationContext.Current.OutgoingMessageProperties.Add(BrokeredMessageProperty.Name, brokeredMessageProperty);

                    channel.GetMessage(message);

                    // Trace the incoming message
                    var builder = new StringBuilder(new string('-', 99));
                    builder.Append("\n\r");

                    builder.AppendLine(string.Format(MessageSuccessfullySent,
                                                     serviceEndpoint.ListenUri.AbsoluteUri,
                                                     brokeredMessageProperty.CorrelationId ?? Empty,
                                                     brokeredMessageProperty.MessageId ?? Empty,
                                                     brokeredMessageProperty.Label ?? Empty));
                    
                    builder.AppendLine(PayloadFormat);
                    builder.AppendLine(MessageText);
                    builder.AppendLine(ReceivedMessagePropertiesHeader);
                    
                    foreach (var property in brokeredMessageProperty.Properties)
                    {
                        builder.AppendLine(string.Format(MessagePropertyFormat, property.Key, property.Value));
                    }

                    var traceMessage = builder.ToString();
                    Console.WriteLine(traceMessage.Substring(0, traceMessage.Length - 1));
                }

                Console.WriteLine("Press [Enter] to exit");
                Console.ReadLine();

                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press [Enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
