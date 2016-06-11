// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Fanuc_MTConnect_Agent_Configurator.Components
{
    public class DeviceInfo
    {
        /// <summary>
        /// Type of Adapter (0id, 30i, etc.);
        /// </summary>
        public string Adapter { get; set; }

        /// <summary>
        /// Path to Adapter files
        /// </summary>
        public string AdapterPath { get; set; }

        /// <summary>
        /// IP address of Fanuc Focas
        /// </summary>
        public string AdapterFocasIp { get; set; }

        public int AdapterPort { get; set; }

        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public string Uuid { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }

    }
}
