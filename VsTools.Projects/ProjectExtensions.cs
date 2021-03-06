using System;

namespace VsTools.Projects
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
            propertyGroup.SetMetadata("Configuration", "Debug", " '$(Configuration)' == '' ");
            propertyGroup.SetMetadata("Platform", "AnyCPU", " '$(Platform)' == '' ");
            propertyGroup.SetMetadata("ProjectGuid", $"{{{projectGuid.ToString().ToUpper()}}}");
            propertyGroup.SetMetadata("OutputType", "Exe");
            propertyGroup.SetMetadata("RootNamespace", rootNameSpace);
            propertyGroup.SetMetadata("AssemblyName", assemblyName);
            propertyGroup.SetMetadata("TargetFrameworkVersion", targetFrameworkVersion);
            propertyGroup.SetMetadata("FileAlignment", "512");
            propertyGroup.SetMetadata("AutoGenerateBindingRedirects", "true");
            propertyGroup.SetMetadata("Deterministic", "true");
            project.Add(propertyGroup);

            var debugPropertyGroup = new PropertyGroup { Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' " };
            debugPropertyGroup.SetMetadata("PlatformTarget", "AnyCPU");
            debugPropertyGroup.SetMetadata("DebugSymbols", "true");
            debugPropertyGroup.SetMetadata("DebugType", "true");
            debugPropertyGroup.SetMetadata("DebugSymbols", "full");
            debugPropertyGroup.SetMetadata("Optimize", "false");
            debugPropertyGroup.SetMetadata("OutputPath", @"bin\Debug\");
            debugPropertyGroup.SetMetadata("DefineConstants", "DEBUG;TRACE");
            debugPropertyGroup.SetMetadata("ErrorReport", "prompt");
            debugPropertyGroup.SetMetadata("WarningLevel", "4");
            project.Add(debugPropertyGroup);

            var releasePropertyGroup = new PropertyGroup { Condition = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' " };
            releasePropertyGroup.SetMetadata("PlatformTarget", "AnyCPU");
            releasePropertyGroup.SetMetadata("DebugType", "pdbonly");
            releasePropertyGroup.SetMetadata("Optimize", "true");
            releasePropertyGroup.SetMetadata("OutputPath", @"bin\Release\");
            releasePropertyGroup.SetMetadata("DefineConstants", "TRACE");
            releasePropertyGroup.SetMetadata("ErrorReport", "prompt");
            releasePropertyGroup.SetMetadata("WarningLevel", "4");
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
            propertyGroup.SetMetadata("OutputType", "Exe");
            propertyGroup.SetMetadata("TargetFramework", targetFramework);
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