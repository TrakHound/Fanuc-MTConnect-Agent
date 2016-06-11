// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    static class AdapterFocusHost
    {
        public static void Set(string path, string host)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                ini.Write("host", " " + host, "focus");
            }
        }

        public static string Get(string path)
        {
            if (File.Exists(path))
            {
                var ini = new IniFile(path);
                return ini.Read("host", "focus");
            }

            return null;
        }
    }
}
