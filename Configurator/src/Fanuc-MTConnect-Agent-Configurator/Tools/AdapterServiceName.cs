// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    static class AdapterSeviceName
    {
        public static void Set(string path, string serviceName)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                ini.Write("service", " " + serviceName, "adapter");
            }
        }

        public static string Get(string path)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                return ini.Read("service", "adapter");
            }

            return null;
        }
    }
}
