using System;
using System.IO;
using System.Xml.Serialization;
using Adglopez.Samples.ServiceBus.Auditing.Client.AuditProxy;

namespace Adglopez.Samples.ServiceBus.Auditing.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var client = new Client.AuditProxy.AuditServiceClient("DefaultEndpoint");

            var request = new CompositeType {BusinessProcess = "Test Process", Content = "<Hello/>"};

            Console.WriteLine("Request:\n{0}", request.SerializeObject());

            client.Audit(request);

            Console.WriteLine("Message sent!");
            Console.ReadLine();
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    }
}
