// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.Snapshots.Xaml")]
[assembly: AssemblyProduct("Orc.Snapshots.Xaml")]
[assembly: AssemblyDescription("Orc.Snapshots.Xaml library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/snapshots", "orcsnapshots")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Behaviors")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Controls")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Converters")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Fonts")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Markup")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Views")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Windows")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]
