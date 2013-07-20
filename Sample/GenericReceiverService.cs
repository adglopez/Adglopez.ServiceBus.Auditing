using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace Adglopez.Samples.ServiceBus.Auditing.Sample
{
    [ServiceBehavior(Namespace = "http://windowsazure.cat.microsoft.com/samples/servicebus")]
    public class GenericReceiverService : IAuditService
    {
        #region Private Constants
        //***************************
        // Formats
        //***************************
        private const string MessageSuccessfullyReceived = "Request Message Received:\n - EndpointUrl:[{0}]\n - CorrelationId=[{1}]\n - MessageId=[{2}]\n - Label=[{3}]";
        private const string ReceivedMessagePropertiesHeader = "Custom Properties:";
        private const string PayloadFormat = "Payload:";
        private const string MessagePropertyFormat = " - Key=[{0}] Value=[{1}]";
        private const string ExceptionMessage = "Receiver is in peek lock mode but receive context is not available!";

        //***************************
        // Constants
        //***************************
        private const string Empty = "EMPTY";
        #endregion

        #region Public Operations
        [OperationBehavior]
        public void GetMessage(Message message)
        {
            try
            {
                // Get the message properties
                var incomingProperties = OperationContext.Current.IncomingMessageProperties;

                if (message != null)
                {
                    var reader = message.GetReaderAtBodyContents();
                    var content = reader.ReadOuterXml();

                    var brokeredMessageProperty = incomingProperties[BrokeredMessageProperty.Name] as BrokeredMessageProperty;

                    // Trace the incoming message
                    var builder = new StringBuilder(new string('-', 99));
                    builder.Append("\n\r");
                    if (brokeredMessageProperty != null)
                    {
                        builder.AppendLine(string.Format(MessageSuccessfullyReceived,
                                                         OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri,
                                                         brokeredMessageProperty.CorrelationId ?? Empty,
                                                         brokeredMessageProperty.MessageId ?? Empty,
                                                         brokeredMessageProperty.Label ?? Empty));
                    }
                    builder.AppendLine(PayloadFormat);
                    builder.AppendLine(content);
                    builder.AppendLine(ReceivedMessagePropertiesHeader);
                    if (brokeredMessageProperty != null)
                    {
                        foreach (var property in brokeredMessageProperty.Properties)
                        {
                            builder.AppendLine(string.Format(MessagePropertyFormat,
                                                             property.Key,
                                                             property.Value));
                        }
                    }
                    var traceMessage = builder.ToString();
                    Console.WriteLine(traceMessage.Substring(0, traceMessage.Length - 1));
                }

                // Complete the Message
                ReceiveContext receiveContext;
                if (ReceiveContext.TryGet(incomingProperties, out receiveContext))
                {
                    receiveContext.Complete(TimeSpan.FromSeconds(10.0d));
                }
                else
                {
                    receiveContext.Abandon(TimeSpan.FromSeconds(10.0d));
                    throw new InvalidOperationException(ExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}