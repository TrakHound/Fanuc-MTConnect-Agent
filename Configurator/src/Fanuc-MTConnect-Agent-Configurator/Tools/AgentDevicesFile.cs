// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Fanuc_MTConnect_Agent_Configurator.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Fanuc_MTConnect_Agent_Configurator.Tools
{
    public static class AgentDevicesFile
    {

        /// <summary>
        /// Find Used Device Ids in devices.xml
        /// </summary>
        /// <returns></returns>
        public static List<string> FindDeviceIds()
        {
            var result = new List<string>();

            string path = Paths.AGENT_DEVICES;

            if (File.Exists(path))
            {
                var doc = XML.Files.ReadDocument(path);
                if (doc != null)
                {
                    var nodes = doc.SelectNodes("//Device");
                    foreach (XmlNode node in nodes)
                    {
                        var id = XML.Attributes.Get(node, "id");
                        result.Add(id);
                    }
                }
            }

            return result;
        }

        public static void WriteDeviceNode(XmlNode node)
        {
            string path = Paths.AGENT_DEVICES;

            try
            {
                if (File.Exists(path))
                {
                    var doc = XML.Files.ReadDocument(path);
                    if (doc != null)
                    {
                        var root = doc.DocumentElement;

                        var devices = root.SelectSingleNode("Devices");

                        var importNode = doc.ImportNode(node, true);
                        devices.AppendChild(importNode);

                        var dummyNode = doc.SelectSingleNode("//Device[@id=\"dummy\"]");
                        if (dummyNode != null)
                        {
                            dummyNode.InnerText = null;
                            dummyNode.InnerXml = null;

                            dummyNode.RemoveAll();
                            devices.RemoveChild(dummyNode);
                        }

                        XML.Files.WriteDocument(doc, path);

                        AgentManagement.Restart();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static void DeleteDeviceNode(string xPath)
        {
            string path = Paths.AGENT_DEVICES;

            if (File.Exists(path))
            {
                var doc = XML.Files.ReadDocument(path);
                if (doc != null)
                {
                    var devicesNode = doc.SelectSingleNode("//Devices");

                    var node = doc.SelectSingleNode(xPath);
                    if (node != null && devicesNode != null)
                    {
                        node.InnerText = null;
                        node.InnerXml = null;

                        node.RemoveAll();
                        devicesNode.RemoveChild(node);

                        // Check if node was only device
                        if (devicesNode.ChildNodes.Count == 0)
                        {
                            XML.Nodes.Add(doc, "//Devices/Device");
                            XML.Attributes.Set(doc, "//Devices/Device", "id", "dummy");
                            XML.Attributes.Set(doc, "//Devices/Device", "uuid", "dummy");
                            XML.Attributes.Set(doc, "//Devices/Device", "name", "dummy");
                        }
                    
                        XML.Files.WriteDocument(doc, path, true);

                        AgentManagement.Restart();
                    }
                }
            }
        }

        public static XmlNode CreateDeviceNode(DeviceInfo info)
        {
            string path = Paths.DEVICE_TEMPLATE;

            if (File.Exists(path))
            {
                var doc = XML.Files.ReadDocument(path);
                if (doc != null)
                {
                    XML.Attributes.Set(doc, "/Device", "name", info.DeviceName);
                    XML.Attributes.Set(doc, "/Device", "id", info.DeviceId);
                    XML.Attributes.Set(doc, "/Device", "uuid", info.Uuid);
                    XML.Attributes.Set(doc, "/Device/Description", "manufacturer", info.Manufacturer);
                    XML.Attributes.Set(doc, "/Device/Description", "model", info.Model);
                    XML.Attributes.Set(doc, "/Device/Description", "serial", info.Serial);
                    XML.InnerText.Set(doc, "/Device/Description", info.Description);

                    var nodes = doc.SelectNodes("//*[@id]");
                    foreach (XmlNode node in nodes)
                    {
                        var id = XML.Attributes.Get(node, "id");

                        if (id != null) id = id.Replace("[dev_id]", info.DeviceId);

                        XML.Attributes.Set(doc, node, "id", id);
                    }
                }

                return doc.DocumentElement;
            }

            return null;
        }

        public static List<DeviceInfo> ReadDeviceInfos()
        {
            var result = new List<DeviceInfo>();

            string path = Paths.AGENT_DEVICES;

            if (File.Exists(path))
            {
                var doc = XML.Files.ReadDocument(path);
                if (doc != null)
                {
                    var nodes = doc.SelectNodes("//Device");
                    foreach (XmlNode node in nodes)
                    {
                        var info = new DeviceInfo();

                        info.DeviceName = XML.Attributes.Get(node, "name");
                        info.DeviceId = XML.Attributes.Get(node, "id");
                        info.Uuid = XML.Attributes.Get(node, "uuid");

                        result.Add(info);
                    }
                }
            }

            return result;
        }


    }
}
