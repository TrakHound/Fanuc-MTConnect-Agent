// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    static class AdapterPort
    {
        public static void Set(string path, int port)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                ini.Write("port", " " + port, "adapter");
            }
        }

        public static int Get(string path)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                string s = ini.Read("port", "adapter");
                if (s != null)
                {
                    int port = -1;
                    int.TryParse(s, out port);
                    return port;
                }
            }

            return -1;
        }
    }
}
