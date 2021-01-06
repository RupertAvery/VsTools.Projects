using System;
using System.Linq;
using VsTools.Projects;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Add();
        }

        public static void Create()
        {
            var testproject = ProjectExtensions.CreateDefaultConsole("netcoreapp3.1");

            var itemGroup1 = new ItemGroup();
            
            var newFile1 = new Compile("testfile.cs");

            itemGroup1.Add(newFile1);
            
            testproject.Add(itemGroup1);
            
            testproject.SaveAs("testconsole.csproj");
        }

        public static void Add()
        {
            var project = Project.Load("testproj.csproj");

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

            project.Save();
        }


        public static void AddBeforeLastImport()
        {
            var project = Project.Load("testproj.csproj");

            var lastImport = project.Imports.Last();

            var itemGroup = new ItemGroup();

            var newFile = new Compile("testfile.cs");

            itemGroup.Add(newFile);

            lastImport.AddBeforeSelf(itemGroup);

            project.Save();
        }

        public static void AddAfterFirstItemGroup()
        {
            var project = Project.Load("testproj.csproj");

            var itemGroup = new ItemGroup();

            var newFile = new Compile("testfile.cs");

            itemGroup.Add(newFile);

            var firstItemGroup = project.ItemGroups.First();

            firstItemGroup.AddAfterSelf(itemGroup);

            var referenceItemGroup = new ItemGroup();

            var guid = Guid.NewGuid();

            var reference = new ProjectReference("../classlibrary/classlibrary.csproj")
            {
                Project = $"{{{guid}}}",
                Name = "Some.Namespace", 
            };

            // Add a Condition attribute to an item Metadata
            reference.Metadata["Project"].Condition = " '${CustomProperty}' == 'CustomValue' ";

            referenceItemGroup.Add(reference);

            itemGroup.AddAfterSelf(referenceItemGroup);

            project.Save();
        }
    }
}
