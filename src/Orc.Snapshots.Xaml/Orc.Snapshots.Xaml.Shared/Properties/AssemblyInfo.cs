// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SharedAssembly.cs

[assembly: AssemblyTitle("Orc.Snapshots.Xaml")]
[assembly: AssemblyProduct("Orc.Snapshots.Xaml")]
[assembly: AssemblyDescription("Orc.Snapshots.Xaml library")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

[assembly: XmlnsPrefix("http://www.wildgums.com/orc.snapshots", "orcsnapshots")]
[assembly: XmlnsDefinition("http://www.wildgums.com/orc.snapshots", "Orc.Snapshots")]
[assembly: XmlnsDefinition("http://www.wildgums.com/orc.snapshots", "Orc.Snapshots.Converters")]
[assembly: XmlnsDefinition("http://www.wildgums.com/orc.snapshots", "Orc.Snapshots.Views")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
    )]