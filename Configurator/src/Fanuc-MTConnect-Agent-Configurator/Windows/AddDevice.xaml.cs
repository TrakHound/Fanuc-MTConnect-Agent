// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Fanuc_MTConnect_Agent_Configurator.Components;
using Fanuc_MTConnect_Agent_Configurator.Tools;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TH_WPF;

namespace Fanuc_MTConnect_Agent_Configurator.Windows
{
    /// <summary>
    /// Interaction logic for AddAdapter.xaml
    /// </summary>
    public partial class AddDevice : Window
    {
        public AddDevice()
        {
            Init();
        }

        public AddDevice(DeviceInfo info)
        {
            Init();

            DeviceInfo = info;
        }

        private void Init()
        {
            InitializeComponent();
            DataContext = this;

            ReadAdapterFiles();
        }

        public static DeviceInfo Open()
        {
            var dialog = new AddDevice();
            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true) return dialog.DeviceInfo;
            else return null;
        }

        public DeviceInfo DeviceInfo
        {
            get { return (DeviceInfo)GetValue(DeviceInfoProperty); }
            set { SetValue(DeviceInfoProperty, value); }
        }

        public static readonly DependencyProperty DeviceInfoProperty =
            DependencyProperty.Register("DeviceInfo", typeof(DeviceInfo), typeof(AddDevice), new PropertyMetadata(null));


        public bool Verified
        {
            get { return (bool)GetValue(VerifiedProperty); }
            set { SetValue(VerifiedProperty, value); }
        }

        public static readonly DependencyProperty VerifiedProperty =
            DependencyProperty.Register("Verified", typeof(bool), typeof(AddDevice), new PropertyMetadata(false));



        private ObservableCollection<ListButton> _adapters;
        public ObservableCollection<ListButton> Adapters
        {
            get
            {
                if (_adapters == null)
                    _adapters = new ObservableCollection<ListButton>();
                return _adapters;
            }

            set
            {
                _adapters = value;
            }
        }


        private void ReadAdapterFiles()
        {
            string path = Paths.ADAPTER_TEMPLATES;

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, Files.ADAPTER_INI, SearchOption.AllDirectories);
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string type = Directory.GetParent(file).Name;

                        var info = AdapterInfo.Read(file);

                        var bt = new ListButton();
                        bt.Selected += Adapter_Selected;
                        bt.Text = type;
                        bt.DataObject = Directory.GetParent(file).FullName;
                        Adapters.Add(bt);
                    }
                }
            }
        }

        private void Adapter_Selected(ListButton bt)
        {
            foreach (var obt in Adapters) if (obt != bt) obt.IsSelected = false;
            bt.IsSelected = true;

            DeviceInfo = new DeviceInfo();
            DeviceInfo.Adapter = bt.Text;
            DeviceInfo.AdapterPath = bt.DataObject.ToString();
        }

        private void IPAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Verify IP Address
            var regex = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            var match = regex.Match(DeviceInfo.AdapterFocasIp);

            Verified = match.Success;
        }

        private void Add_Clicked(TH_WPF.Button bt)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Clicked(TH_WPF.Button bt)
        {
            Close();
        }
    }
}
