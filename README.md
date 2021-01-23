# Bulk .e2e file parser, anonymizer, and DICOM exporter
Automatically parses, anonymizes, and exports a DICOM file from one or more .e2e files.

Installation
============
1. Clone [GitHub repository](https://github.com/ccaneke/e2e_to_dicom_windows_service "GitHub repo")

2. Copy the following E2eFileInterpreter binaries directory to a directory on the Windows system to avoid Windows and Linux path conflicts.

`E2EFileInterpreter/E2EFileInterpreter/bin/Debug/netcoreapp3.1/win-x64` 

3. Copy the following Windows Service binaries directory to a directory on the Windows system to avoid Windows and Linux path conflicts.

`E2eToDicomWorkerService/E2eToDicomWorkerService/bin/Debug/netcoreapp3.1/win-x64`

4. Configure the E2eFileInterpreter program using the config file in the E2eFileInterpreter binaries directory.

`E2EFileInterpreter/E2EFileInterpreter/bin/Debug/netcoreapp3.1/win-x64/config.json`

5. Configure the E2eToDicomWorkerService Windows Service using the config file in the E2eFileInterpreter Windows Service binaries directory.

`E2eToDicomWorkerService/E2eToDicomWorkerService/bin/Debug/netcoreapp3.1/win-x64/config.json`

6. Install the E2eToDicomWorkerService Windows Service by running the standard Windows Service Installer using the command:

```
sc create <ServiceName> BinPath=C:\path\to\win-x64\E2eToDicomWorkerService.exe
```

Usage
=====

* Start the Service:

```
sc start <ServiceName>
```

* Stop the Service:

```
sc stop <ServiceName>
```

* Delete Service:

```
sc delete <ServiceName>
```

# Data
The data folder specified in the configuration file for the E2eFileInterpreter Windows Service must contain only .e2e files.
