# Fanuc MTConnect Agent
MTConnect Agent for Fanuc CNC Equipment using Focas over Ethernet

![Configurator] (http://feenux.com/github/images/configurator_screenshot_01.png)

### Configurator
The Configurator provides a user interface to configure and manage the MTConnect Agent and Adapters. The Agent is setup during install and each Adapter can be added/removed using the Configurator. When a device is added, the appropriate adapter is installed, the adapter is added to the agent.cfg file, and the Device is added to the devices.xml file.

### Agent
The agent is based on the cppagent project available at https://github.com/mtconnect/cppagent.

### Adapters
The adapters are based on the Fanuc adapter that is part of the project available at https://github.com/mtconnect/adapter/tree/master/fanuc.
