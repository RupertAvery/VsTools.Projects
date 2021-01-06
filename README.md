# Introduction

**VsTools.Projects** is a set of wrapper classes around XDocument for reading and modifying the contents of Visual Studio C# Project (.csproj) files.

It grew out of a need to programatically insert generated source files into a project.

VsTools.Projects only abstracts the most common elements, as the format is completely extensible and can contain custom elements specific to a third-party assembly.

The API and documentation are currently a work in progress, so expect some changes as the project progresses.

# NuGet

```
PS> Install-Package VsTools.Projects
```

# Usage

There are two types of .csproj format. The pre-2017 format was the format used by older .NET Framework projects and has the following root element:

```xml
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
```

The 2017 format is the format introduced by Visual Studio 2017 for newer .NET Framework projects and .NET Core and has the following root element:

```xml
<Project Sdk="Microsoft.NET.Sdk">
```

The newer format takes advantage of a lot of default values, and by assuming that all files in the path and sub-paths are part of the project reduces the size of the project file signigicantly.

VsTools.Projects can read and modify both, but will not validate the contents based on the expected format. It is up to you to make sure you are using the correct elements and attributes.

Make sure you understand the schema or element hierarchy.

## Loading an existing Project

To load an existing project, use `Project.Load`. The property `Is2017Project` will be `true` if the root element has the attribute `Sdk`.

```csharp
var project = new Project.Load("path\\to\\project.csproj");

var isNewVersion = project.Is2017Project;
```

## Adding a file for compilation to a Project

To add a `.cs` file to a `.csproj`, you need to add a `Compile` element to a new or existing `ItemGroup` element.

```csharp
var project = Project.Load("path\\to\\project.csproj");

var itemGroup = new ItemGroup();

var newFile = new Compile("path\\to\\file.cs");

itemGroup.AddContent(newfile);

project.Add(itemGroup);

project.Save();
```

This will insert the following xml as the last child of the `Project` element.

```xml
  <ItemGroup>
    <Compile Include="path\to\file.cs"/>
  </ItemGroup>
```

## PropertyGroup

The `PropertyGroup` element stores project properties such as `PlatformTarget`, `OutputType`, `AssemblyName`, `TargetFrameworkVersion` to name a few, as well as other custom properties.

### Accessing PropertyGroup Properties
 
PropertyGroup element propeties can be accessed through the `PropertyGroup.Properties` collection. Properties are of type `StringPropertyType` which allow you to set a `Condition`.

```csharp
var debug = project.PropertyGroups.First(x => x.Condition == " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");

Console.WriteLine(debug.Properties["DebugSymbols"].Value);
```

### Getting and Setting PropertyGroup Properties

The `PropertyGroup` class provides convenience methods `GetProperty` and `SetProperty` which avoids having to go through a `StringPropertyType`.

`GetProperty` returns the string value or `null` if the property does not exist.

```csharp
propertyGroup.GetProperty("Configuration");\
```

`SetProperty` is overloaded with a parameter to set a `Condition` attribute on the property. Setting a property value to `null` will remove the property element.

```csharp
// Set the property 'Configuration' to 'Debug' with a condition
propertyGroup.SetProperty("Configuration", "Debug", " '$(Configuration)' == '' ");
// Set the property 'OutputType' to 'Exe'
propertyGroup.SetProperty("OutputType", "Exe");
// Remove property 'SccLocalPath'
propertyGroup.SetProperty("SccLocalPath", null);
```

Setting a property can also be done through the `PropertyGroup.Properties` collection.

```csharp
debug.Properties["DebugSymbols"] = new Property("Configuration", "Debug") { Condition = " '$(Configuration)' == '' " };
```

## ItemGroup and Item

An `ItemGroup` contains a collection of `Item` objects, which define inputs into the build system.

Common items are `Reference`, `Compile`, `Resource`, and `None`, which tell the build system how to handle the specified item.

Items should inherit from the `Item` class. Items have a required `Include` attribute, and 0 or more Item Metadata that are usually encoded as child elements, for example, a `Reference` can have a `HintPath`

```csharp 
var references = new ItemGroup();

var reference = new Reference("Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL");

reference.HintPath = "..\\packages\\Newtonsoft.Json.11.0.1\\lib\\net45\\Newtonsoft.Json.dll";

reference.Private = "True";

references.Add(reference);

var files = new ItemGroup();

var file = new Compile("path\\to\\class.cs");

file.DependentUpon = "path\\to\\page.cshtml";

files.Add(file);

project.Add(references);
project.Add(files);

```

This will generate the following XML.

```xml
  <ItemGroup>
    <Reference Include="Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL">
      <HintPath>..\packages\Newtonsoft.Json.11.0.1\lib\net45\Newtonsoft.Json.dll</HintPath>
      <Private>True</Private>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    <Compile Include="path\to\class.cs"/>
      <DependentUpon>page.cshtml</DependentUpon>
    </Compile/>
    <Content Include="path\to\page.cshtml" />
  </ItemGroup>
```

## Item Metadata

For convenience, known Metadata for an element are exposed as string properties on the Item  implementation, e.g. `Reference` has a `HintPath` property as shown above.

It is possible to add custom metadata using `AddOrUpdateMetadataValue` on a class inheriting from `Item`, or `SetMetadata` or `AddMetadata` on any class inheriting from `ProjectElement`.

## Conditions

In MSBuild, Conditions allow for conditional compilation based on defined project properties and other user defined conditions. Conditions are implemented as a `Condition` attribute on most elememts. Setting a condition on an `ItemGroup` for example will cause items under that item group to be added only if the condition is met.

All classes that inherit from `ProjectElement` have a `Condition` property that sets the underlying element's `Condition` attribute.

The following will cause the itemGroup's items to be included during compilation if the Debug configuration is specified.

```csharp
itemGroup.Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
```

In order to set a `Condition` attribute on an item Metadata, ensure that the Metadata has been created by setting it through the property, then access the Metadata as a `Property` through the items `Metadata` collection property

```csharp
reference.Metadata["Project"].Condition = " '${CustomProperty}' == 'CustomValue' ";
```
