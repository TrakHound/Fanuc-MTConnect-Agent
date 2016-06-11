// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Fanuc_MTConnect_Agent_Configurator.Components;
using Fanuc_MTConnect_Agent_Configurator.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using TH_Global.Functions;

namespace Fanuc_MTConnect_Agent_Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //AgentServiceName = AgentConfigurationFile.GetServiceName();
            AgentPort = AgentConfigurationFile.GetPort();

            ServerMonitor_Initialize();

            ReadAdapters();

            usedIds = AgentDevicesFile.FindDeviceIds();
        }

        #region "Dependency Properties"

        public bool IsAgentStarted
        {
            get { return (bool)GetValue(IsAgentStartedProperty); }
            set { SetValue(IsAgentStartedProperty, value); }
        }

        public static readonly DependencyProperty IsAgentStartedProperty =
            DependencyProperty.Register("IsAgentStarted", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public string AgentServiceStatus
        {
            get { return (string)GetValue(AgentServiceStatusProperty); }
            set { SetValue(AgentServiceStatusProperty, value); }
        }

        public static readonly DependencyProperty AgentServiceStatusProperty =
            DependencyProperty.Register("AgentServiceStatus", typeof(string), typeof(MainWindow), new PropertyMetadata(null));



        //public string AgentServiceName
        //{
        //    get { return (string)GetValue(AgentServiceNameProperty); }
        //    set { SetValue(AgentServiceNameProperty, value); }
        //}

        //public static readonly DependencyProperty AgentServiceNameProperty =
        //    DependencyProperty.Register("AgentServiceName", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public int AgentPort
        {
            get { return (int)GetValue(AgentPortProperty); }
            set { SetValue(AgentPortProperty, value); }
        }

        public static readonly DependencyProperty AgentPortProperty =
            DependencyProperty.Register("AgentPort", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        #endregion

        #region "Agent Service Monitor"

        private void ServerMonitor_Initialize()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Elapsed += ServerMonitor_Timer_Elapsed;
            timer.Enabled = true;
        }

        private void ServerMonitor_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;
            timer.Interval = 2000;

            Dispatcher.BeginInvoke(new Action(ServerMonitor_GUI), UI_Functions.PRIORITY_BACKGROUND, new object[] { });
        }

        private void ServerMonitor_GUI()
        {
            AgentServiceStatus = Service_Functions.GetServiceStatus(Names.AGENT_SERVICE_NAME).ToString();
            IsAgentStarted = Service_Functions.IsServiceRunning(Names.AGENT_SERVICE_NAME);
        }

        #endregion

        private ObservableCollection<Controls.AdapterItem> _adapterItems;
        public ObservableCollection<Controls.AdapterItem> AdapterItems
        {
            get
            {
                if (_adapterItems == null)
                    _adapterItems = new ObservableCollection<Controls.AdapterItem>();
                return _adapterItems;
            }

            set
            {
                _adapterItems = value;
            }
        }

        private List<int> usedPorts = new List<int>();
        private List<string> usedIds = new List<string>();
        private List<string> usedNames = new List<string>();

        private void ReadAdapters()
        {
            ReadAgentAdapters();

            ReadAdapterFiles();
        }

        private void ReadAdapterFiles()
        {
            try
            {
                string path = Paths.ADAPTERS;

                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, Files.ADAPTER_INI, SearchOption.AllDirectories);
                    if (files != null)
                    {
                        AdapterItems.Clear();

                        foreach (var file in files)
                        {
                            var info = AdapterInfo.Read(file);

                            var item = new Controls.AdapterItem(info);
                            item.RemoveClicked += AdapterItem_RemoveClicked;
                            AdapterItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private List<AgentAdapterInfo> ReadAgentAdapters()
        {
            var result = new List<AgentAdapterInfo>();

            usedNames.Clear();
            usedPorts.Clear();

            result = AgentConfigurationFile.GetAdapters();
            foreach (var agentAdapter in result)
            {
                usedNames.Add(agentAdapter.DeviceName);
                usedPorts.Add(agentAdapter.Port);
            }

            return result;
        }

        
        private void AgentStart_Clicked(TH_WPF.Button bt)
        {
            AgentManagement.Start();
        }

        private void AgentStop_Clicked(TH_WPF.Button bt)
        {
            AgentManagement.Stop();
        }

        private void AddDevice_Clicked(TH_WPF.Button bt)
        {
            var deviceInfo = Windows.AddDevice.Open();
            if (deviceInfo != null)
            {
                InstallAdapter(deviceInfo); 
            }
        }

        private void InstallAdapter(DeviceInfo deviceInfo)
        {
            Cursor = System.Windows.Input.Cursors.Wait;

            try
            {
                var adapterInfos = ReadAgentAdapters();

                string id = GetAvailableId();
                string deviceName = GetAvailableName(deviceInfo.Adapter);
                int port = GetAvailablePort();

                deviceInfo.DeviceName = deviceName;
                deviceInfo.DeviceId = id;
                deviceInfo.Uuid = id;
                deviceInfo.AdapterPort = port;

                usedIds.Add(id);
                usedNames.Add(deviceName);
                usedPorts.Add(port);

                var adapterInfo = new AgentAdapterInfo();
                adapterInfo.DeviceName = deviceInfo.DeviceName;
                adapterInfo.Port = port;

                adapterInfos.Add(adapterInfo);

                AgentConfigurationFile.WriteAdapters(adapterInfos);

                var node = AgentDevicesFile.CreateDeviceNode(deviceInfo);
                if (node != null) AgentDevicesFile.WriteDeviceNode(node);

                CreateAdapterFiles(deviceInfo);

                ReadAdapters();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void CreateAdapterFiles(DeviceInfo deviceInfo)
        {
            string path = Paths.ADAPTERS;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string newPath = Path.Combine(path, deviceInfo.DeviceName);

            Directory.CreateDirectory(newPath);

            foreach (var file in Directory.GetFiles(deviceInfo.AdapterPath))
            {
                string filename = Path.GetFileName(file);

                File.Copy(file, Path.Combine(newPath, filename), true);
            }

            string iniPath = Path.Combine(newPath, "adapter.ini");
            string serviceName = deviceInfo.DeviceName + "-Adapter";

            AdapterPort.Set(iniPath, deviceInfo.AdapterPort);
            AdapterFocusHost.Set(iniPath, deviceInfo.AdapterFocasIp);
            AdapterSeviceName.Set(iniPath, serviceName);

            string exePath = Path.Combine(newPath, "adapter.exe");

            AdapterManagement.Install(exePath);
            AdapterManagement.Start(serviceName);
        }


        private void AdapterItem_RemoveClicked(AdapterInfo info)
        {
            if (info != null) UninstallAdapter(info);

            int i = AdapterItems.ToList().FindIndex(x => x.ServiceName == info.ServiceName);
            if (i >= 0) AdapterItems.RemoveAt(i);
        }

        private void UninstallAdapter(AdapterInfo info)
        {
            Cursor = System.Windows.Input.Cursors.Wait;

            try
            {
                AdapterManagement.Stop(info.ServiceName);
                AdapterManagement.Uninstall(info.ServiceName);

                // Remove adapter from Agent Configuration File (agent.cfg);
                var adapterInfos = ReadAgentAdapters();
                var match1 = adapterInfos.Find(x => x.DeviceName == info.DeviceName);
                if (match1 != null) adapterInfos.Remove(match1);

                AgentConfigurationFile.WriteAdapters(adapterInfos);

                // Remove Device node from Agent's devices.xml file
                var deviceInfos = AgentDevicesFile.ReadDeviceInfos();
                var match2 = deviceInfos.Find(x => x.DeviceName == info.DeviceName);
                if (match2 != null)
                {
                    string id = match2.DeviceId;

                    AgentDevicesFile.DeleteDeviceNode("//Device[@id=\"" + id + "\"]");
                }

                // Delete adapter folder in "Adapters"
                string path = Path.GetDirectoryName(info.Path);
                Directory.Delete(path, true);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private string GetAvailableId()
        {
            string s = "dev_";
            int i = 1;

            string t = s + i.ToString("00");

            while (usedIds.Contains(t))
            {
                i++;
                t = s + i.ToString("00");
            }

            return t;
        }

        private string GetAvailableName(string adapter)
        {
            string s = "Fanuc-" + adapter + "-";
            int i = 1;

            string t = s + i.ToString("00");

            while (usedNames.Contains(t))
            {
                i++;
                t = s + i.ToString("00");
            }

            return t;
        }

        private int GetAvailablePort()
        {
            int port = 7878;
            while (usedPorts.Contains(port)) port++;

            return port;
        }

    }
}
