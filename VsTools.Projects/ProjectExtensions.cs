using System;

namespace VsTools.Projects
{
    public static class ProjectExtensions
    {
        public static Project CreateDefaultConsole(Guid projectGuid, string rootNameSpace, string assemblyName, string targetFrameworkVersion)
        {
            var project = new Project();
            var import = new Import(@"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props");
            import.Condition = @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')";
            project.Add(import);
            var propertyGroup = new PropertyGroup();
            propertyGroup.AddChild("Configuration", "Debug", " '$(Configuration)' == '' ");
            propertyGroup.AddChild("Platform", "AnyCPU", " '$(Platform)' == '' ");
            propertyGroup["ProjectGuid"] = $"{{{projectGuid.ToString().ToUpper()}}}";
            propertyGroup["OutputType"] = "Exe";
            propertyGroup["RootNamespace"] = rootNameSpace;
            propertyGroup["AssemblyName"] = assemblyName;
            propertyGroup["TargetFrameworkVersion"] = targetFrameworkVersion;
            propertyGroup["FileAlignment"] = "512";
            propertyGroup["AutoGenerateBindingRedirects"] = "true";
            propertyGroup["Deterministic"] = "true";
            project.Add(propertyGroup);

            var propertyGroup2 = new PropertyGroup();
            propertyGroup2.Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
            propertyGroup2["PlatformTarget"] = "AnyCPU";
            propertyGroup2["DebugSymbols"] = "true";
            propertyGroup2["DebugType"] = "true";
            propertyGroup2["DebugSymbols"] = "full";
            propertyGroup2["Optimize"] = "false";
            propertyGroup2["OutputPath"] = @"bin\Debug\";
            propertyGroup2["DefineConstants"] = "DEBUG;TRACE";
            propertyGroup2["ErrorReport"] = "prompt";
            propertyGroup2["WarningLevel"] = "4";
            project.Add(propertyGroup2);

            var propertyGroup3 = new PropertyGroup();
            propertyGroup3.Condition = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ";
            propertyGroup3["PlatformTarget"] = "AnyCPU";
            propertyGroup3["DebugType"] = "pdbonly";
            propertyGroup3["Optimize"] = "true";
            propertyGroup3["OutputPath"] = @"bin\Release\";
            propertyGroup3["DefineConstants"] = "TRACE";
            propertyGroup3["ErrorReport"] = "prompt";
            propertyGroup3["WarningLevel"] = "4";
            project.Add(propertyGroup3);

            var itemGroup = new ItemGroup();
            itemGroup.AddContent(new Reference("System"));
            itemGroup.AddContent(new Reference("System.Core"));
            itemGroup.AddContent(new Reference("System.Xml.Linq"));
            itemGroup.AddContent(new Reference("System.Data.DataSetExtensions"));
            itemGroup.AddContent(new Reference("Microsoft.CSharp"));
            itemGroup.AddContent(new Reference("System.Data"));
            itemGroup.AddContent(new Reference("System.Net.Http"));
            itemGroup.AddContent(new Reference("System.Xml"));
            project.Add(itemGroup);

            var import2 = new Import(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");
            project.Add(import2);

            return project;
        }
    }
}