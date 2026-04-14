# 🎙️ Enchantress System: GTA Edition

A zero-latency, hardware-level voice command engine built in C#. 

This project is a custom macro system designed to completely bypass standard Windows API restrictions and inject physical hardware scan codes directly into DirectX/DirectInput game engines like *Grand Theft Auto: Vice City*.

## 🎥 See it in Action

> **Note:** https://youtu.be/UmVhnDpvnFQ

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
   
## 🎤 How to Play (Hands-Free Mode)
**CRITICAL:** You must run your terminal (Command Prompt or PowerShell) as an Administrator. Without elevated privileges, Windows will block the simulated hardware inputs from reaching the game window.
Once the terminal in rootfolder, run : **dotnet run**, Once sucessfull, it will state: **Enchantress is listening...** , leave it running in the background and launch GTA Vice City. You do not need to press any manual keys or use a controller. The system continuously monitors your microphone. **Simply play the game and speak the commands out loud.** The background engine will automatically recognize your voice and translate it into hardware keystrokes in real-time, controlling Tommy hands-free.

## 🎮 Command List
Make sure the GTA game window is currently active/in-focus, and speak clearly:
| Voice Command | Action | Key/Macro |
| :--- | :--- | :--- |
| **"Walk forward"** | Holds W key continuously | `DIK_W` |
| **"Walk back"** | Holds S key continuously | `DIK_S` |
| **"Turn left"** | Pans camera left | Mouse `dx -400` |
| **"Turn right"** | Pans camera right | Mouse `dx 400` |
| **"Stop"** | Releases all active keys | - |
| **"Jump"** | Taps Spacebar | `DIK_SPACE` |
| **"Get in" / "Drive"** | Taps F (Enter/Exit vehicle) | `DIK_F` |
| **"Panzer"** | Spawns a Rhino Tank | Types `PANZER` |
| **"Aspirin"** | Full Health | Types `ASPIRIN` |
| **"Leave me alone"**| Clears Wanted Level | Types `LEAVEMEALONE` |
