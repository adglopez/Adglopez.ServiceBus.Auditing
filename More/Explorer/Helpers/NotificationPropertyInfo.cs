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
using System.Collections.Generic;
#endregion

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    public class NotificationPropertyInfo
    {
        #region Public Constructors
        public NotificationPropertyInfo()
        {
            Name = null;
            Value = null;
        }

        public NotificationPropertyInfo(string key, string value)
        {
            Name = key;
            Value = value;
        }
        #endregion

        #region Static Constructor
        static NotificationPropertyInfo()
        {
            Properties = new List<NotificationPropertyInfo>
                             {
                                 new NotificationPropertyInfo("title", "New Message"),
                                 new NotificationPropertyInfo("msg", "The things you own end up owning you.")
                             };
            AdditionalHeaders = new List<NotificationPropertyInfo>();
        }
        #endregion

        #region Public Instance Properties
        public string Name { get; set; }
        public string Value { get; set; }
        #endregion

        #region Public Static Properties
        public static List<NotificationPropertyInfo> Properties { get; private set; }
        public static List<NotificationPropertyInfo> AdditionalHeaders { get; private set; }
        #endregion
    }
}