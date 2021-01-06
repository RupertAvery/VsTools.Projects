using System;
using VsTools.Projects;
using Project = VsTools.Projects.Project;

namespace Test
{

    public static class ProjectExtensions
    {
        public static Project CreateDefaultConsole(Guid projectGuid, string rootNameSpace, string assemblyName, string targetFrameworkVersion)
        {
            var project = Project.CreatePreVS2017Project();

            var import = new Import(@"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props")
            {
                Condition = @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"
            };
            project.Add(import);

            var propertyGroup = new PropertyGroup();
            propertyGroup.SetProperty("Configuration", "Debug", " '$(Configuration)' == '' ");
            propertyGroup.SetProperty("Platform", "AnyCPU", " '$(Platform)' == '' ");
            propertyGroup.SetProperty("ProjectGuid", $"{{{projectGuid.ToString().ToUpper()}}}");
            propertyGroup.SetProperty("OutputType", "Exe");
            propertyGroup.SetProperty("RootNamespace", rootNameSpace);
            propertyGroup.SetProperty("AssemblyName", assemblyName);
            propertyGroup.SetProperty("TargetFrameworkVersion", targetFrameworkVersion);
            propertyGroup.SetProperty("FileAlignment", "512");
            propertyGroup.SetProperty("AutoGenerateBindingRedirects", "true");
            propertyGroup.SetProperty("Deterministic", "true");
            project.Add(propertyGroup);

            var debugPropertyGroup = new PropertyGroup { Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' " };
            debugPropertyGroup.SetProperty("PlatformTarget", "AnyCPU");
            debugPropertyGroup.SetProperty("DebugSymbols", "true");
            debugPropertyGroup.SetProperty("DebugType", "true");
            debugPropertyGroup.SetProperty("DebugSymbols", "full");
            debugPropertyGroup.SetProperty("Optimize", "false");
            debugPropertyGroup.SetProperty("OutputPath", @"bin\Debug\");
            debugPropertyGroup.SetProperty("DefineConstants", "DEBUG;TRACE");
            debugPropertyGroup.SetProperty("ErrorReport", "prompt");
            debugPropertyGroup.SetProperty("WarningLevel", "4");
            project.Add(debugPropertyGroup);

            var releasePropertyGroup = new PropertyGroup { Condition = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' " };
            releasePropertyGroup.SetProperty("PlatformTarget", "AnyCPU");
            releasePropertyGroup.SetProperty("DebugType", "pdbonly");
            releasePropertyGroup.SetProperty("Optimize", "true");
            releasePropertyGroup.SetProperty("OutputPath", @"bin\Release\");
            releasePropertyGroup.SetProperty("DefineConstants", "TRACE");
            releasePropertyGroup.SetProperty("ErrorReport", "prompt");
            releasePropertyGroup.SetProperty("WarningLevel", "4");
            project.Add(releasePropertyGroup);

            var referenceItemGroup = new ItemGroup();
            referenceItemGroup.Add(new Reference("System"));
            referenceItemGroup.Add(new Reference("System.Core"));
            referenceItemGroup.Add(new Reference("System.Xml.Linq"));
            referenceItemGroup.Add(new Reference("System.Data.DataSetExtensions"));
            referenceItemGroup.Add(new Reference("Microsoft.CSharp"));
            referenceItemGroup.Add(new Reference("System.Data"));
            referenceItemGroup.Add(new Reference("System.Net.Http"));
            referenceItemGroup.Add(new Reference("System.Xml"));
            project.Add(referenceItemGroup);

            var targetsImport = new Import(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");
            project.Add(targetsImport);

            return project;
        }

        public static Project CreateDefaultConsole(string targetFramework)
        {
            var project = Project.CreateVS2017Project();

            var propertyGroup = new PropertyGroup();
            propertyGroup.SetProperty("OutputType", "Exe");
            propertyGroup.SetProperty("TargetFramework", targetFramework);
            project.Add(propertyGroup);

            var itemGroup = new ItemGroup();
            var appSettings = new None() { Update = "appsettings.json" };
            appSettings.SetMetadata("CopyToOutputDirectory", "PreserveNewest");
            itemGroup.Add(appSettings);
            project.Add(itemGroup);

            return project;
        }

    }
}