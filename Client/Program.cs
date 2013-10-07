using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Adglopez.ServiceBus.Auditing.Client.AuditProxy;

namespace Adglopez.ServiceBus.Auditing.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            int messagesToSend;

            do
            {
                Console.Clear();
                Console.Write("Numer of messages to send: ");

            } while (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out messagesToSend));


            var sendMessageTask = new Action<object>(i =>
                                            {
                                                var client = new Client.AuditProxy.AuditServiceClient("AuditService");

                                                var request = new CompositeType { BusinessProcess = "Test Process", Content = "<Hello/>" };

                                                if (messagesToSend == 0)
                                                    Console.WriteLine("Request {0}:\n{1}", i, request.SerializeObject());

                                                client.Audit(request);
                                            });

            var tasks = new Task[messagesToSend];

            for (int i = 0; i < messagesToSend; i++)
            {
                tasks[i] = new Task(sendMessageTask, null);
                tasks[i].Start();
            }

            Console.WriteLine("Sending messages...");

            Task.WaitAll(tasks);

            Console.WriteLine("{0} messages were sent.", messagesToSend);

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
