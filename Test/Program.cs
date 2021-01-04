using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsTools.Projects;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var project = ProjectExtensions.CreateDefaultConsole(Guid.NewGuid(), "Test", "Test", "v4.5.2");

            //var firstItemGroup = project.ItemGroups.First();

            //var folder = (Folder)firstItemGroup.Contents.First();

            //Console.WriteLine(folder.Include);

            var itemGroup = new ItemGroup();

            var newFile = new Compile("testfile.cs");

            newFile.DependentUpon = "parent.cs";

            itemGroup.AddContent(newFile);

            project.Add(itemGroup);

            //firstItemGroup.AddAfterSelf(itemGroup);

            var referenceItemGroup = new ItemGroup();

            var guid = Guid.NewGuid();

            var reference = new ProjectReference("../classlibrary/classlibrary.csproj", $"{{{guid}}}", "Some.Namespace");

            referenceItemGroup.AddContent(reference);

            itemGroup.AddAfterSelf(referenceItemGroup);

            project.Save("testproj.csproj");
        }
    }
}
