using System;
using System.Speech.Recognition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Enchantress
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    class Program
    {
        // --- 1. P/INVOKE & DIRECTINPUT SETUP ---
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT { public int type; public InputUnion u; }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion 
        { 
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT { public uint uMsg; public ushort wParamL; public ushort wParamH; }

        const int INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_SCANCODE = 0x0008;
        const uint KEYEVENTF_KEYUP = 0x0002;

        // Mouse Constants
        const int INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_MOVE = 0x0001;

        // GTA Vice City DirectInput Scan Codes
        const ushort DIK_W = 0x11;
        const ushort DIK_A = 0x1E;
        const ushort DIK_S = 0x1F;
        const ushort DIK_D = 0x20;
        const ushort DIK_F = 0x21;     
        const ushort DIK_SPACE = 0x39; 
        const ushort DIK_LCONTROL = 0x1D; 

        // STATE MANAGEMENT: Track currently held keys
        static HashSet<ushort> activeHeldKeys = [];

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing Enchantress System Engine...");

            SpeechRecognitionEngine recognizer;
            try
            {
                recognizer = new SpeechRecognitionEngine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Could not initialize Speech Engine. {ex.Message}");
                return;
            }

            using (recognizer)
            {
                Choices commands = new();
                commands.Add(new string[] { 
                    "walk forward", "walk back", "turn left", "turn right", 
                    "stop", "jump", "fight", "get in", "drive", 
                    "panzer", "leave me alone", "aspirin" 
                });

                GrammarBuilder gb = new();
                gb.Append(commands);
                Grammar g = new(gb);

                recognizer.LoadGrammarAsync(g);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                Console.WriteLine("Enchantress is listening... Ready for commands.");
                Console.ReadLine(); 
            }
        }

        // --- 2. COMMAND ROUTER & INTERRUPT LOGIC ---
        private static void Recognizer_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.7f) return;

            Console.WriteLine($"\n[Command Recognized]: {e.Result.Text}");

            // INTERRUPT: Release everything before starting a new command
            ReleaseAllActiveKeys(); 

            switch (e.Result.Text)
            {
                case "walk forward":
                case "drive":
                    HoldKey(DIK_W);
                    Console.WriteLine("--> Action: Holding W");
                    break;
                case "walk back":
                    HoldKey(DIK_S);
                    Console.WriteLine("--> Action: Holding S");
                    break;
                case "turn left":
                    TurnCamera(-200); // Modify this number if he turns too far or too little
                    Console.WriteLine("--> Action: Turned Camera Left");
                    break;
                case "turn right":
                    TurnCamera(200);  // Modify this number if he turns too far or too little
                    Console.WriteLine("--> Action: Turned Camera Right");
                    break;
                case "stop":
                    Console.WriteLine("--> Action: Stopped");
                    break;
                case "jump":
                    TapKey(DIK_SPACE);
                    Console.WriteLine("--> Action: Tapped Spacebar");
                    break;
                case "fight":
                    TapKey(DIK_LCONTROL);
                    Console.WriteLine("--> Action: Tapped L-Ctrl");
                    break;
                case "get in":
                    TapKey(DIK_F);
                    Console.WriteLine("--> Action: Tapped F");
                    break;
                case "panzer":
                    TypeCheatCode([0x19, 0x1E, 0x31, 0x2C, 0x12, 0x13]); 
                    Console.WriteLine("--> Action: PANZER Activated");
                    break;
                case "leave me alone":
                    TypeCheatCode([0x26, 0x12, 0x1E, 0x2F, 0x12, 0x32, 0x12, 0x1E, 0x26, 0x18, 0x31, 0x12]); 
                    Console.WriteLine("--> Action: LEAVE ME ALONE Activated");
                    break;
                case "aspirin":
                    TypeCheatCode([0x1E, 0x1F, 0x19, 0x17, 0x13, 0x17, 0x31]); 
                    Console.WriteLine("--> Action: ASPIRIN Activated");
                    break;
            }
        }

        // --- 3. HARDWARE INPUT METHODS ---
        static void HoldKey(ushort scanCode)
        {
            INPUT input = new() { type = INPUT_KEYBOARD };
            input.u.ki.wScan = scanCode;
            input.u.ki.dwFlags = KEYEVENTF_SCANCODE; 
            SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
            activeHeldKeys.Add(scanCode); 
        }

        static void ReleaseKey(ushort scanCode)
        {
            INPUT input = new() { type = INPUT_KEYBOARD };
            input.u.ki.wScan = scanCode;
            input.u.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP; 
            SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
            activeHeldKeys.Remove(scanCode);
        }

        static void ReleaseAllActiveKeys()
        {
            if (activeHeldKeys.Count == 0) return;
            foreach (ushort key in activeHeldKeys.ToList()) 
            {
                ReleaseKey(key);
            }
        }

        static void TapKey(ushort scanCode)
        {
            HoldKey(scanCode);
            Thread.Sleep(50); 
            ReleaseKey(scanCode);
        }

        static void TypeCheatCode(ReadOnlySpan<ushort> scanCodes)
        {
            foreach (ushort code in scanCodes)
            {
                TapKey(code);
                Thread.Sleep(20); 
            }
        }

        static void TurnCamera(int deltaX)
        {
            // Smoothly move the mouse over 20 mini-steps so the game engine registers it naturally
            int steps = 20;
            int dxPerStep = deltaX / steps;
            
            for (int i = 0; i < steps; i++)
            {
                INPUT input = new() { type = INPUT_MOUSE };
                input.u.mi.dx = dxPerStep;
                input.u.mi.dy = 0;
                input.u.mi.dwFlags = MOUSEEVENTF_MOVE;
                
                SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
                Thread.Sleep(5); // 5ms delay per step 
            }
        }
    }
}