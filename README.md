# 🌡️ Environment Real-Time Temperature & Humidity Monitoring System

A simple IoT-based project that uses the **M5Core2 ESP32 device** to monitor and transmit real-time temperature and humidity data over a shared network. The data is received and displayed by a desktop application.

---

## 📋 Requirements

### 🛠️ Hardware
- M5Core2 (ESP32-based) device

### 📚 Software
- Required C++ libraries (ESP32, M5Stack, WiFi, etc.)
- Desktop Receiver App (C# or other)
- Ensure M5Core2 and the Receiver App are connected to the **same local network** as the Server App

---

## 🔧 Setup Instructions

1. **Install Required Libraries**  
   Make sure your development environment (e.g., Arduino IDE) has the necessary libraries installed to compile and upload the C++ code to the M5Core2 device.

2. **Edit the C++ Code**  
   Update the Wi-Fi SSID and password to connect the M5Core2 device to the same network as the Receiver App and Server.  
   ![Code Snippet](https://github.com/user-attachments/assets/49689a85-a2df-4f3e-98d2-3400bcd22a33)

3. **Start the Receiver App First**  
   Always launch the Receiver App **before** turning on the M5Core2 device to ensure the connection is properly established.

4. **Upload the Program**  
   Upload the updated C++ code to the M5Core2 device using your development tool (e.g., Arduino IDE).

5. **Modify App.config**  
   Open the `App.config` file in the Receiver App and update the connection string to match your local network or server configuration.

---

## 👨‍💻 Authors

- [@vincent-capistrano](https://www.github.com/vincent-capistrano)
