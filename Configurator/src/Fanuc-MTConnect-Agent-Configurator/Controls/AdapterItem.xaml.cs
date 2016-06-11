// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Fanuc_MTConnect_Agent_Configurator.Components;
using System;
using System.Windows;
using System.Windows.Controls;
using TH_Global.Functions;

namespace Fanuc_MTConnect_Agent_Configurator.Controls
{
    /// <summary>
    /// Interaction logic for AdapterItem.xaml
    /// </summary>
    public partial class AdapterItem : UserControl
    {
        public AdapterItem()
        {
            Init();
        }

        public AdapterItem(AdapterInfo info)
        {
            Init();

            adapterInfo = info;

            ServiceName = info.ServiceName;
            FocusHost = info.FocusHost;
            Port = info.Port;

            ServerMonitor_Initialize();
        }

        private void Init()
        {
            InitializeComponent();
            root.DataContext = this;
        }

        private AdapterInfo adapterInfo;

        #region "Dependency Properties"

        public bool IsStarted
        {
            get { return (bool)GetValue(IsStartedProperty); }
            set { SetValue(IsStartedProperty, value); }
        }

        public static readonly DependencyProperty IsStartedProperty =
            DependencyProperty.Register("IsStarted", typeof(bool), typeof(AdapterItem), new PropertyMetadata(false));


        public string ServiceName
        {
            get { return (string)GetValue(ServiceNameProperty); }
            set { SetValue(ServiceNameProperty, value); }
        }

        public static readonly DependencyProperty ServiceNameProperty =
            DependencyProperty.Register("ServiceName", typeof(string), typeof(AdapterItem), new PropertyMetadata(null));


        public string ServiceStatus
        {
            get { return (string)GetValue(ServiceStatusProperty); }
            set { SetValue(ServiceStatusProperty, value); }
        }

        public static readonly DependencyProperty ServiceStatusProperty =
            DependencyProperty.Register("ServiceStatus", typeof(string), typeof(AdapterItem), new PropertyMetadata(null));


        public string FocusHost
        {
            get { return (string)GetValue(FocusHostProperty); }
            set { SetValue(FocusHostProperty, value); }
        }

        public static readonly DependencyProperty FocusHostProperty =
            DependencyProperty.Register("FocusHost", typeof(string), typeof(AdapterItem), new PropertyMetadata(null));


        public int Port
        {
            get { return (int)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(int), typeof(AdapterItem), new PropertyMetadata(7878));

        #endregion

        #region "Service Monitor"

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
            ServiceStatus = Service_Functions.GetServiceStatus(ServiceName).ToString();
            IsStarted = Service_Functions.IsServiceRunning(ServiceName);
        }

        #endregion

        private void Start_Clicked(TH_WPF.Button bt)
        {
            Tools.AdapterManagement.Start(ServiceName);
        }

        private void Stop_Clicked(TH_WPF.Button bt)
        {
            Tools.AdapterManagement.Stop(ServiceName);
        }

        public delegate void ClickHandler(AdapterInfo info);
        public event ClickHandler RemoveClicked;

        private void Remove_Clicked(TH_WPF.Button bt)
        {
            if (RemoveClicked != null) RemoveClicked(adapterInfo);
        }
    }
}
