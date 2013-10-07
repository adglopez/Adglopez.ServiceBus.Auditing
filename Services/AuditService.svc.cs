using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Adglopez.Samples.ServiceBus.Auditing.Services
{
    public class AuditService : IAuditService
    {
        const  string Path = @"C:\Temp\";

        public void Audit(CompositeType message)
        {
            try
            {
                File.WriteAllText(Path + DateTime.Now.ToString("ddMMyyyyHHmmss.fff") + ".xml", SerializeObject(message));
            }
            catch (Exception ex)
            {   
                Debug.WriteLine(ex.ToString());
            }            
        }

        public static string SerializeObject<T>(T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    }
}
