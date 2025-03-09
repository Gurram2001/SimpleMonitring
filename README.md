# Simple System Monitor

## Description
A lightweight console application to monitor system performance, including CPU usage and top memory-consuming processes. The application logs the data to a file and updates in real-time.

## Features
- Displays overall CPU usage
- Lists the top 10 processes by memory usage
- Logs monitoring data to a file
- Refreshes every 5 seconds

## Requirements
- .NET Framework or .NET Core
- Windows operating system (for process monitoring support)

## Installation
1. Clone the repository:
   ```sh
   git clone <repository_url>
   ```
2. Navigate to the project folder:
   ```sh
   cd SimpleSystemMonitor
   ```
3. Build the project:
   ```sh
   dotnet build
   ```

## Usage
Run the application using:
```sh
dotnet run
```
The log file is stored in the **Documents** folder with a timestamped filename.

## Exit
Press `Ctrl + C` to stop the application.

## License
This project is open-source and available for modification and use.

