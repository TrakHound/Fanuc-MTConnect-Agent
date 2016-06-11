// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;
using Fanuc_MTConnect_Agent_Configurator.Tools;

namespace Fanuc_MTConnect_Agent_Configurator.Components
{
    public class AdapterInfo
    {
        public string Path { get; set; }

        public string DeviceName { get; set; }
        public string ServiceName { get; set; }
        public int Port { get; set; }

        public string FocusHost { get; set; }

        public static AdapterInfo Read(string path)
        {
            var info = new AdapterInfo();
            info.Path = path;
            info.DeviceName = Directory.GetParent(path).Name;
            info.ServiceName = AdapterSeviceName.Get(path);
            info.Port = AdapterPort.Get(path);
            info.FocusHost = AdapterFocusHost.Get(path);
            return info;
        }
    }
}
