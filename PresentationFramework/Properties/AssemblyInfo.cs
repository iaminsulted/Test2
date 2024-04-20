﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyVersion("5.0.0.0")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Shapes")]
[assembly: InternalsVisibleTo("PresentationFramework.Royale, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PresentationFramework.Luna, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PresentationFramework.Aero, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PresentationFramework.Aero2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PresentationFramework.AeroLite, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PresentationFramework.Classic, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("System.Windows.Presentation, PublicKey=00000000000000000400000000000000")]
[assembly: InternalsVisibleTo("PresentationFramework-SystemCore, PublicKey=00000000000000000400000000000000")]
[assembly: InternalsVisibleTo("PresentationFramework-SystemXml, PublicKey=00000000000000000400000000000000")]
[assembly: InternalsVisibleTo("System.Windows.Controls.Ribbon, PublicKey=00000000000000000400000000000000")]
[assembly: Dependency("mscorlib,", LoadHint.Always)]
[assembly: Dependency("System,", LoadHint.Always)]
[assembly: Dependency("WindowsBase,", LoadHint.Always)]
[assembly: Dependency("PresentationCore,", LoadHint.Always)]
[assembly: Dependency("System.Xaml,", LoadHint.Sometimes)]
[assembly: Dependency("System.Core,", LoadHint.Sometimes)]
[assembly: ThemeInfo(ResourceDictionaryLocation.ExternalAssembly, ResourceDictionaryLocation.None)]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Shell")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "av")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Shell")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "wpf")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Shell")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/netfx/2009/xaml/presentation", "wpf")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "System.Windows.Markup")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml", "x")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Input")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/xps/2005/06", "metro")]
[assembly: XmlnsCompatibleWith("http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key", "http://schemas.microsoft.com/winfx/2006/xaml")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06/documentstructure", "System.Windows.Documents.DocumentStructures")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyFileVersion("5.0.20.52003")]
[assembly: AssemblyInformationalVersion("5.0.0-rtm.20520.3+9e81b0885121e9958e48895ae48be9639a396528")]
[assembly: AssemblyProduct("PresentationFramework")]
[assembly: AssemblyTitle("PresentationFramework")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/wpf")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
[assembly: AssemblyDefaultAlias("PresentationFramework")]
[assembly: AssemblyMetadata("FileVersion", "5.0.20.52003")]
[assembly: AssemblyMetadata("BuiltBy", "a0019LG")]
[assembly: AssemblyMetadata("Repository", "https://github.com/dotnet/wpf")]
[assembly: AssemblyMetadata("Commit", "9e81b0885121e9958e48895ae48be9639a396528")]
[assembly: AssemblyMetadata("Language", "C#")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
// PresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
[assembly: TypeForwardedTo(typeof(IProvidePropertyFallback))]
// System.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
[assembly: TypeForwardedTo(typeof(ArrayExtension))]
[assembly: TypeForwardedTo(typeof(IProvideValueTarget))]
[assembly: TypeForwardedTo(typeof(NullExtension))]
[assembly: TypeForwardedTo(typeof(StaticExtension))]
[assembly: TypeForwardedTo(typeof(TypeExtension))]
// WindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
[assembly: TypeForwardedTo(typeof(NameScope))]