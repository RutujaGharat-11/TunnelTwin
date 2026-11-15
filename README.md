# 🚇 TunnelTwin
### IoT-Powered 3D Tunnel Monitoring & Visualization

TunnelTwin is a Unity-based 3D tunnel monitoring and visualization system that integrates IoT sensor data with a virtual tunnel environment. It demonstrates how physical environmental conditions can be collected through sensors and represented within an interactive 3D simulation.

The project aims to bridge the gap between physical sensor monitoring and 3D visualization by creating a virtual representation of environmental conditions inside a tunnel.

## 🎯 Project Objectives

The primary objective of TunnelTwin is to demonstrate the integration of IoT hardware and 3D visualization for environmental monitoring.

The system focuses on:

- Monitoring environmental parameters such as temperature and humidity.
- Measuring ambient light intensity.
- Monitoring soil moisture conditions.
- Collecting sensor readings through an ESP8266 microcontroller.
- Visualizing sensor data within a Unity-based 3D tunnel environment.
- Mapping physical sensor conditions to their corresponding virtual representations.

## 🛠️ Technology Stack

| Technology | Purpose |
|---|---|
| Unity | 3D environment development and visualization |
| C# | Unity scripting and application logic |
| ESP8266 | Sensor data acquisition and communication |
| DHT11 | Temperature and humidity measurement |
| LDR | Ambient light intensity measurement |
| Soil Moisture Sensor | Soil moisture measurement |
| IoT Communication | Transfer of sensor readings to the visualization system |

## 🏗️ System Architecture

TunnelTwin follows a sensor-to-visualization workflow in which physical environmental data is collected and reflected in a virtual tunnel environment.

```text
    PHYSICAL ENVIRONMENT
             |
             ▼
       IoT SENSORS
   ┌─────────┼──────────┐
   ▼         ▼          ▼
 DHT11      LDR     Soil Moisture
   └─────────┼──────────┘
             ▼
          ESP8266
             |
             ▼
      SENSOR DATA
             |
             ▼
      UNITY ENGINE
             |
             ▼
    3D TUNNEL ENVIRONMENT
             |
             ▼
   VISUAL REPRESENTATION
   OF SENSOR CONDITIONS
```

## ✨ Key Features

- **3D Tunnel Visualization:** Interactive virtual representation of a tunnel environment developed using Unity.
- **Environmental Monitoring:** Collection and visualization of temperature and humidity readings.
- **Light Intensity Monitoring:** Monitoring ambient light conditions using an LDR sensor.
- **Moisture Monitoring:** Integration of a soil moisture sensor for monitoring moisture conditions.
- **IoT Integration:** ESP8266-based acquisition and transfer of physical sensor readings.
- **Sensor-to-Environment Mapping:** Representation of physical sensor conditions within the virtual 3D environment.

## 🔌 Hardware Components

| Component | Function |
|---|---|
| ESP8266 | Microcontroller responsible for sensor data acquisition |
| DHT11 | Measures temperature and relative humidity |
| LDR | Detects changes in ambient light intensity |
| Soil Moisture Sensor | Measures soil moisture levels |

## 📂 Project Structure

```text
TunnelTwin/
│
├── Assets/
│   ├── Scenes/          # Unity scenes
│   ├── Scripts/         # C# scripts
│   ├── Materials/       # Materials and textures
│   └── Models/          # 3D models and assets
│
├── Packages/            # Unity package dependencies
├── ProjectSettings/     # Unity project configuration
├── .gitignore
└── README.md
```

## 🚀 Getting Started

Follow these steps to run the project locally.

### Prerequisites

- Unity Hub
- Compatible Unity Editor version
- ESP8266 microcontroller
- DHT11, LDR, and Soil Moisture sensors
- Required USB drivers and hardware connections

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/USERNAME/TunnelTwin.git
```

**2. Open Unity Hub.**

**3. Add the project**

- Select `Add` → `Add project from disk`.
- Navigate to the cloned `TunnelTwin` directory.
- Select the project folder.

**4. Open the project**

Launch the project using its compatible Unity Editor version.

**5. Open the main scene**

Navigate to the `Assets/Scenes/` directory and open the main tunnel scene.

**6. Run the simulation**

Press the Play button in Unity to launch the 3D tunnel visualization.

> **Note:** For hardware-based monitoring, ensure that the ESP8266 is properly connected to the sensors and that the sensor data communication mechanism is configured correctly.

## 🔮 Future Enhancements

Potential improvements to TunnelTwin include:

- Cloud-based IoT data integration.
- Integration of additional environmental and safety sensors.
- Real-time alerts for abnormal environmental conditions.
- Historical sensor data storage and analysis.
- Enhanced 3D visualization and environmental simulation.
- Web-based monitoring and remote access.
- Advanced tunnel condition monitoring capabilities.

## 👩‍💻 Project Information

| Attribute | Details |
|---|---|
| Project Name | TunnelTwin |
| Domain | Internet of Things (IoT) |
| Visualization Platform | Unity 3D |
| Programming Language | C# |
| Microcontroller | ESP8266 |
| Application Area | Environmental Monitoring |

---

**TunnelTwin explores the integration of IoT-based environmental sensing and interactive 3D visualization, demonstrating how physical-world data can be represented within a virtual environment.**
