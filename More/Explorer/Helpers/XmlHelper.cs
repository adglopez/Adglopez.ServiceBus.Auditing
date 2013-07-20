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
using System.Text;
using System.Xml;
#endregion

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    static class XmlHelper
    {
        /// <summary>
        /// Checks whether the input text is a valid xml string.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>Yes if the text is a valid xml string, false otherwise.</returns>
        public static bool IsXml(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return false;
                }
                using (var stringReader = new StringReader(text))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        while (xmlReader.Read())
                        {
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Indents the xml contained in the XmlDocument object.
        /// </summary>
        /// <param name="document">The XmlDocument containing the Xml to indent.</param>
        /// <returns>Xml indented</returns>
        public static string Indent(XmlDocument document)
        {
            if (document == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                document.Save(writer);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Indents the xml contained in the string object.
        /// </summary>
        /// <param name="xml">The string containing the Xml to indent.</param>
        /// <returns>Xml indented</returns>
        public static string Indent(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            if (!IsXml(xml))
            {
                return xml;
            }
            var document = new XmlDocument();
            document.LoadXml(xml);
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                document.Save(writer);
            }
            return sb.ToString();
        }
    }
}
