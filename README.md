# HTTP Request IP
__HTTP Request IP Spy for .NET__

[![rolosoft_public_packages MyGet Build Status](https://www.myget.org/BuildSource/Badge/rolosoft_public_packages?identifier=68df1389-82d6-4670-8ca0-10190394e5fa)](https://www.myget.org/)

## Overview
For web applications that need to know the IP address of the remote client.

To work out the IP address of a client is not easy. Proxy servers / software, VPNs and interim HTTP layers can make it difficult to work out the actual IP address.

This project is a .NET solution that inspects several HTTP request headers and makes a "best guess" on the client IP address.

This software can be used with .NET4.0 or higher.

__Note__: Will not work in IIS7.x pipeline mode in global.asax Application_Start() method.


## Installation
From [Nuget].
```
Install-Package Rsft.HttpRequestIp
```

## Quickstart

### Encode

```c#

/*Get best guess IP*/ 
string remoteIp = Rsft.HttpRequestIp.Getter.Get().BestGuessIp;

/*Is proxied?*/ 
bool isProxied = Rsft.HttpRequestIp.Getter.Get().IsProxied;

/*Relevant server variables.*/ 
ServerVariables serverVariables = Rsft.HttpRequestIp.Getter.Get().ServerVariables;

```

