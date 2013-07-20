using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Adglopez.Samples.ServiceBus.Auditing.Sample
{
    [ServiceContract(Namespace = "http://windowsazure.cat.microsoft.com/samples/servicebus", SessionMode = SessionMode.Allowed)]
    public interface IAuditService
    {
        [OperationContract(Action = "GetMessage", IsOneWay = true)]
        [ReceiveContextEnabled(ManualControl = true)]
        void GetMessage(Message message);
    }
}