// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    public static class Names
    {
        public const string AGENT_SERVICE_NAME = "Fanuc MTConnect Agent";
    }

    public static class FolderNames
    {
        public const string AGENT = "Agent";
        public const string ADAPTERS = "Adapters";
        public const string ADAPTER_TEMPLATES = "Adapter Templates";
        public const string DEVICE_TEMPLATES = "Device Templates";
    }

    public static class Files
    {
        public const string ADAPTER_INI = "adapter.ini";
        public const string AGENT_CONFIG = "agent.cfg";
        public const string ADAPTER_EXE = "adapter.exe";
        public const string AGENT_DEVICES = "devices.xml";
        public const string DEVICE_TEMPLATE = "device.xml";
    }

    public static class Paths
    {
        //public static string AGENT_BIN = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, FolderNames.AGENT, "bin");

        //public static string AGENT_DEVICES = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, FolderNames.AGENT, "bin", Files.AGENT_DEVICES);
        //public static string AGENT_CONFIG = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, FolderNames.AGENT, "bin", Files.AGENT_CONFIG);

        //public static string ADAPTERS = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, FolderNames.ADAPTERS);
        //public static string ADAPTER_TEMPLATES = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, FolderNames.ADAPTER_TEMPLATES);
        //public static string DEVICE_TEMPLATE = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Files.DEVICE_TEMPLATE);

        public static string AGENT_BIN = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, FolderNames.AGENT, "bin");

        public static string AGENT_DEVICES = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, FolderNames.AGENT, "bin", Files.AGENT_DEVICES);
        public static string AGENT_CONFIG = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, FolderNames.AGENT, "bin", Files.AGENT_CONFIG);

        public static string ADAPTERS = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, FolderNames.ADAPTERS);
        public static string ADAPTER_TEMPLATES = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, FolderNames.ADAPTER_TEMPLATES);
        public static string DEVICE_TEMPLATE = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Files.DEVICE_TEMPLATE);
    }
}
