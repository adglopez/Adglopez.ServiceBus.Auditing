using System.ServiceModel;

namespace Adglopez.Samples.ServiceBus.Auditing.Services
{
    [ServiceContract(Namespace = "http://adglopez.samples/servicebus/auditing", SessionMode = SessionMode.Allowed)]
    public interface IAuditService
    {
        [OperationContract(Action = "Audit", IsOneWay = true)]
        void Audit(CompositeType composite);
    }
}
