# Network-Enabled Ballistic Rangefinder Calculator

A professional-grade C# console application designed for long-range shooting and optical calculations. It calculates distance to targets using object size and reticle subtensions (MIL/MOA) and features real-time data transmission over a TCP network.

## Key Features

*   Dual Standard Support: Works with both MIL (MRAD) and MOA reticle types.
*   Multi-Unit Conversion: Input sizes in Inches, Centimeters, or Meters.
*   Precision Math: High-accuracy ballistic formulas for range estimation.
*   Network Integration: Automatically transmits calculation results to a remote listener/server using TCP Sockets.
*   Robust Input Handling: 
    *   Supports decimal inputs (e.g., 2.5).
    *   Input cancellation/step-back feature (type b to go back).
    *   Built-in error handling for invalid data.

## Technical Stack

*   Language: C# (.NET)
*   Protocol: TCP/IP (Layer 4 OSI)
*   Architecture: Client-Server

## Project Structure

1.  BallisticCalculator (Client): The main tool for distance calculation and data broadcasting.
2.  SimpleServer (Listener): A lightweight server component that catches and displays incoming calculation data from the network.

## How to Use

1.  Start the Listener: Run the SimpleServer application first. It will start listening on 127.0.0.1:8888.
2.  Run the Calculator: Launch BallisticCalculator.exe.
3.  Perform Calculation: Follow the on-screen prompts to enter object size and reticle value.
4.  Network Broadcast: Once the distance is calculated, the client will attempt to send a data packet to the server.
5.  Verify: Check the Server console or use Wireshark (Loopback adapter) to see the TCP traffic.

## Network Debugging (Wireshark)

To inspect the data transmission, use the following filter in Wireshark:
`tcp.port == 8888```

## Future Goals
*   Integration with ballistic drop tables (DOPE).
*   Encrypted data transmission (SSL/TLS).
*   GUI version for mobile/desktop.

---
*Created as part of a Cybersecurity and Networking deep-dive study.*