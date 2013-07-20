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
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
#endregion

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    public static class JsonSerializerHelper
    {
        /// <summary>
        /// Serialize an object using the DataContractJsonSerializer.
        /// </summary>
        /// <param name="item">The object that must be serialized</param>
        /// <returns>A Json representation of the object passed as an argument.</returns>
        public static string Serialize(object item)
        {
            if (item == null)
            {
                throw new ArgumentException("The item argument cannot be null.");
            }

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(item.GetType());
                serializer.WriteObject(memoryStream, item);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Deserialize an Json string into an object using the DataContractJsonSerializer.
        /// </summary>
        /// <param name="item">The string that must be deserialized.</param>
        /// <param name="type">The type of the serialized object.</param>
        /// <returns>The object deserialized.</returns>
        public static object Deserialize(string item, Type type)
        {
            if (item == null)
            {
                throw new ArgumentException("The item argument cannot be null.");
            }

            if (type == null)
            {
                throw new ArgumentException("The type argument cannot be null.");
            }
            var serializer = new DataContractJsonSerializer(type);
            var byteArray = Encoding.UTF8.GetBytes(item);
            using (var memoryStream = new MemoryStream(byteArray))
            {
                return serializer.ReadObject(memoryStream);
            }
        }

        /// <summary>
        /// Deserialize an Json string into an object using the DataContractJsonSerializer.
        /// </summary>
        /// <param name="stream">The stream that must be deserialized.</param>
        /// <param name="type">The type of the serialized object.</param>
        /// <returns>The object deserialized.</returns>
        public static object Deserialize(Stream stream, Type type)
        {
            if (stream == null)
            {
                throw new ArgumentException("The stream argument cannot be null.");
            }

            if (type == null)
            {
                throw new ArgumentException("The type argument cannot be null.");
            }
            var serializer = new DataContractJsonSerializer(type);
            //using (var reader = new StreamReader(stream))
            //{
            //    System.Diagnostics.Trace.WriteLine(reader.ReadToEnd());
            //}
            //return null;
            return serializer.ReadObject(stream);
        }
    }
}
