// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Diagnostics;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    public static class AgentManagement
    {

        public static void Start()
        {
            var info = new ProcessStartInfo();
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.FileName = "cmd";
            info.Arguments = "cmd /c sc start \"" + Names.AGENT_SERVICE_NAME + "\"";
            info.Verb = "runas";

            var process = new Process();
            process.StartInfo = info;

            process.Start();
            process.WaitForExit();
        }

        public static void Stop()
        {
            var info = new ProcessStartInfo();
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.FileName = "cmd";
            info.Arguments = "cmd /c sc stop \"" + Names.AGENT_SERVICE_NAME + "\"";
            info.Verb = "runas";

            var process = new Process();
            process.StartInfo = info;

            process.Start();
            process.WaitForExit();
        }

        public static void Restart()
        {
            AgentManagement.Stop();
            AgentManagement.Start();
        }

    }
}
