# 🎙️ Enchantress System: GTA Vice City Edition

A zero-latency, hardware-level voice command engine built in C#. 

This project is a custom macro system designed to completely bypass standard Windows API restrictions and inject physical hardware scan codes directly into DirectX/DirectInput game engines like *Grand Theft Auto: Vice City*.

## 🎥 See it in Action

> **Note:** Video demonstration coming soon!

---

## 🚀 The Engineering Problem & Solution
Standard macro applications (using `SendKeys` or standard virtual key events) fail in full-screen games because 3D engines read directly from the hardware ports to reduce latency. 

**Enchantress** solves this by:
1. Utilizing `user32.dll` via **P/Invoke** to send true **Hardware Scan Codes** (`KEYEVENTF_SCANCODE`).
2. Implementing proper 64-bit memory struct Unions (`MOUSEINPUT`, `KEYBDINPUT`, `HARDWAREINPUT`) to ensure the Windows Kernel accepts the payloads without silent failures on modern architectures.
3. Using an **Interrupt State Manager** to instantly release held keys before executing complex macros (preventing cheat codes from failing due to overlapping movement inputs).

## ✨ Features
* **Zero-Latency Offline Recognition:** Powered by `System.Speech`, the engine runs locally without cloud delays.
* **DirectInput Bypassing:** Game engines cannot distinguish these commands from a physical mechanical keyboard.
* **Smooth Camera Panning:** Simulated horizontal mouse flicks for in-game camera steering.
* **Flawless Cheat Code Execution:** Rapid-fire keystroke arrays that drop tanks instantly.
* **Dynamic Sensitivity:** Built-in confidence thresholds to ignore background game audio and explosions.

## 🛠️ Prerequisites
* **OS:** Windows 10 / 11
* **Environment:** .NET SDK (7.0 or higher)
* **Hardware:** A working microphone

## ⚙️ Installation & Setup
1. Clone this repository:
   ```bash
   git clone [https://github.com/ajinkyasatuse/Enchantress_GTA.git](https://github.com/ajinkyasatuse/Enchantress_GTA.git)
