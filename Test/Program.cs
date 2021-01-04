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
            var project = Project.Load("testproj.csproj");

            var itemGroup = new ItemGroup();
            // Add a file that will be included for compilation
            var newFile = new Compile("testfile.cs");
            // You can add many files to one ItemGroup...
            // Add it to the ItemGroup
            itemGroup.AddContent(newFile);

            // Add the new itemgroup adter the first itemgroup
            var firstItemGroup = project.ItemGroups.First();

            firstItemGroup.AddAfterSelf(itemGroup);

            // Add a project referecne.
            var referenceItemGroup = new ItemGroup();

            var guid = Guid.NewGuid();

            var reference = new ProjectReference("../classlibrary/classlibrary.csproj", $"{{{guid}}}", "Some.Namespace");

            referenceItemGroup.AddContent(reference);

            // add it after our itemgroup
            itemGroup.AddAfterSelf(referenceItemGroup);

            project.Save("testproj.csproj");
        }
    }
}
