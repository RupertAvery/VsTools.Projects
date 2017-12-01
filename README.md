# Introduction

**VsTools.Projects** is a library for reading and modifying the contents of (currently) Visual Studio C# Project (.csproj) files.

It grew out of a need to automate the insertion of newly created files using data extracted from existing files.

**NOTE:** This is not supposed to be used inside Visual Studio as an alternative to writing an Visual Studio extension. Rather, it is a way to automate modifying the csproj XML easily, without having to think about elements and attributes and indentation.

# NuGet

You can install the library from NuGet

```
PS> Install-Package VsTools.Projects
```

# Features

* Read and write .csproj files (Only tested on 2017)
* Access to first-level elements easily as IEnumerable properties
    * Import
    * PropertyGroup
    * ItemGroup
    * Target
* Add and Remove Elements
* Default attributes and text elements exposed as properties, just get and set
* Automatic indentation

# Usage

## Add an ItemGroup with a Compile element

```csharp

// Open a project, create a new ItemGroup and add some files that will be nested under existing files
// in the project, assuming that the file resides in the same folder as the dependency
// and add the new ItemGroup after the first ItemGroup

var proj = Project.Load("path\\to\\project.csproj");

var firstItemGroup = proj.ItemGroups.First();

var itemGroup = new ItemGroup();

var newFile = new Compile("path\\to\\file.cs");

newFile.DependentUpon = "parent.cs";

itemGroup.AddContent(newfile);

firstItemGroup.AddAfterSelf(itemGroup);

project.Save("path\\to\\project.csproj");

```

This will insert the following xml after the first item group.

```xml
  <ItemGroup>
    <Compile Include="path\to\file.cs">
      <DependentUpon>parent.cs</DependentUpon>
    </Compile>
  </ItemGroup>
```

## Access PropertyGroup Elements

PropertyGroup child elements can be accessed through the object's indexer as elements vary between PropertyGroups. You can get or set an element through the accessor. Setting an element that does not exist will create the element. Setting the value on an existing element to null will remove the element.

```csharp

// Open a project, create a new ItemGroup and add some files that will be nested under existing files
// in the project, assuming that the file resides in the same folder as the dependency
// and add the new ItemGroup after the first ItemGroup

var proj = Project.Load("path\\to\\project.csproj");

var debug = proj.PropertyGroups.First(x => x.Condition == " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");

Console.WriteLine(debug["DebugSymbols"]);
Console.WriteLine(debug["DebugType"]);
Console.WriteLine(debug["Optimize"]);
Console.WriteLine(debug["OutputPath"]);
Console.WriteLine(debug["DefineConstants"]);
Console.WriteLine(debug["ErrorReport"]);
Console.WriteLine(debug["WarningLevel"]);
Console.WriteLine(debug["Prefer32Bit"]);
Console.WriteLine(debug["PrecompileBeforePublish"]);
Console.WriteLine(debug["RunCodeAnalysis"]);
Console.WriteLine(debug["TreatWarningsAsErrors"]);
Console.WriteLine(debug["PublishDatabases"]);
Console.WriteLine(debug["CodeAnalysisAdditionalOptions"]);

```

This will output:

```
true
full
false
bin\
DEBUG;TRACE
prompt
4
false
false
false
false
false
/assemblyCompareMode:StrongNameIgnoringVersion
```

## ItemGroup Contents

Since ItemGroup Contents are fairly consistent, there are specialized classes for accessing them. 

Example ItemGroup Contents:

```xml
  <ItemGroup>
    <Reference Include="Binbin.Linq.PredicateBuilder, Version=1.0.3.26645, Culture=neutral, processorArchitecture=MSIL">
      <HintPath>..\packages\Binbin.Linq.PredicateBuilder.1.0.3.26645\lib\net45\Binbin.Linq.PredicateBuilder.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Compile Include="path\to\class.cs"/>
      <DependentUpon>page.cshtml</DependentUpon>
    </Compile/>
    <Content Include="path\to\page.cshtml" />
  <ItemGroup>
```

ItemGroup contents inherit from ItemGroupContent. Accessing the Contents property will return a list of typed objects depending on the class name.  

```csharp 
var contents = itemGroup.Contents.ToList();

// of type Reference
Console.WriteLine(contents[0].HintPath);

// of type Compile
Console.WriteLine(contents[1].DependentUpon);

// of type Content
Console.WriteLine(contents[0].Include);

```