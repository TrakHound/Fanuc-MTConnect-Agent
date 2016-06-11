// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Fanuc_MTConnect_Agent_Configurator.Components;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    static class AgentConfigurationFile
    {

        public static int GetPort()
        {
            int result = 5000;

            string path = Paths.AGENT_CONFIG;

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    string cfg = reader.ReadToEnd();
                    string s = GetValue("Port", cfg);
                    if (!string.IsNullOrEmpty(s))
                    {
                        int port = -1;
                        if (int.TryParse(s, out port)) return port;
                    }
                }
            }

            return result;
        }

        public static string GetServiceName()
        {
            string result = null;

            string path = Paths.AGENT_CONFIG;

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {

                    string cfg = reader.ReadToEnd();
                    return GetValue("ServiceName", cfg);
                }
            }

            return result;
        }

        private static string GetValue(string key, string cfg)
        {
            // Find start of 'Key'
            int i = cfg.IndexOf(key);
            if (i >= 0)
            {
                i = cfg.IndexOf('=', i);
                int z = cfg.IndexOf(Environment.NewLine, i + 1);

                // Get only 'Value' text
                return cfg.Substring(i + 1, z - i).Trim();
            }

            return null;
        }

        public static List<AgentAdapterInfo> GetAdapters()
        {
            var result = new List<AgentAdapterInfo>();

            string path = Paths.AGENT_CONFIG;

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    string cfg = "";

                    // Remove All Whitespace and linebreaks
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0) cfg += line.Trim();
                    }

                    // Find start of 'Adapters' section
                    int i = cfg.IndexOf("Adapters");
                    i = cfg.IndexOf('{', i);
                    int z = cfg.IndexOf("}", i + 1);

                    // Check if empty
                    if (z > i + 1)
                    {
                        z = cfg.IndexOf("}}", i + 1) + 2;

                        // Get only 'Adapters' section text
                        cfg = cfg.Substring(i, z - i);

                        i = cfg.IndexOf('{');

                        int x = 0;
                        while (x < cfg.Length - 1)
                        {
                            int j = cfg.IndexOf('{', i + 1);

                            string deviceName = cfg.Substring(i + 1, j - i - 1).Trim();

                            i = cfg.IndexOf("Port", j);
                            i = cfg.IndexOf('=', i);
                            j = cfg.IndexOf('}', i + 1);

                            string p = cfg.Substring(i + 1, j - i - 1).Trim();
                            int port = -1;
                            int.TryParse(p, out port);

                            if (!string.IsNullOrEmpty(deviceName) && port >= 0)
                            {
                                var info = new AgentAdapterInfo();
                                info.DeviceName = deviceName;
                                info.Port = port;
                                result.Add(info);
                            }

                            x = j + 1;
                            i = j;
                        }
                    }
                }
            }

            return result;
        }

        public static void WriteAdapters(List<AgentAdapterInfo> infos)
        {
            string path = Paths.AGENT_CONFIG;

            if (File.Exists(path))
            {
                string cfg = "";

                using (var reader = new StreamReader(path))
                {
                    cfg = reader.ReadToEnd();
                }

                // Find start of 'Adapters' section
                int x = cfg.IndexOf("Adapters");
                int i = cfg.IndexOf('{', x);
                int j = i + 1;

                char c1 = cfg[i];
                char c2 = cfg[j];

                // Look for two successive '}'
                while (j < cfg.Length && (!(c1 == '}' && c2 == '}') && !(c1 =='{' && c2 == '}')))
                {
                    var tmp = cfg[j];

                    if (!char.IsWhiteSpace(tmp))
                    {
                        c1 = c2;
                        c2 = tmp;
                    }

                    j++;
                }

                // Remove entire Adapters Section
                cfg = cfg.Remove(x, j - x);
                cfg = cfg.TrimEnd();

                string n = Environment.NewLine;
                string adapters = "Adapters {" + n + n;

                foreach (var info in infos)
                {
                    adapters += info.DeviceName + " {" + n;
                    adapters += "Port = " + info.Port + n;
                    adapters += "}" + n + n;
                }

                adapters += "}" + n + n;

                cfg += n + n + adapters;

                File.WriteAllText(path, cfg);
            }
        }

    }
}
