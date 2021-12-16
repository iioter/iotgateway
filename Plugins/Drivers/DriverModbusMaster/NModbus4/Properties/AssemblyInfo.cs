using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("NModbus4")]
[assembly: AssemblyProduct("NModbus4")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("Copyright © 2006 Scott Alexander, 2015 Dmitry Turin")]
[assembly: AssemblyDescription("NModbus4 is a C# implementation of the Modbus protocol. " +
           "Provides connectivity to Modbus slave compatible devices and applications. " +
           "Supports ASCII, RTU, TCP, and UDP protocols. " +
           "NModbus4 it's a fork of NModbus(https://code.google.com/p/nmodbus)")]

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]
[assembly: AssemblyInformationalVersion("3.0.0-dev")]

#if !SIGNED
[assembly: InternalsVisibleTo("NModbus4.UnitTests")]
[assembly: InternalsVisibleTo("NModbus4.IntegrationTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif
