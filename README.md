# Bulk .e2e file parser, anonymizer, and DICOM exporter
Automatically parses, anonymizes, and exports a DICOM file from one or more .e2e files.

Installation
============

1. Download E2eFileInterpreter binary bundle.
 
2. Download Windows Service binary bundle.

3. Configure the E2eFileInterpreter program using the config file in the E2eFileInterpreter binary bundle.

4. Configure the E2eFileInterpreter Windows Service using the config file in the E2eFileInterpreter Windows Service binary bundle.

5. Install the E2eFileInterpreter Windows Service by running the standard Windows Service Installer using the command:

```
sc create <ServiceName> BinPath=C:\full\path\to\E2eFileInterpreter\Windows\Service\binary\bundle\dir\WindowsServiceE2eFileInterpreter.exe
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
