All future changes to the repository must be made by means of the develop branch. To introduce those changes to the STABLE branch, create a pull request and at least two people must approve the request. This is a QA and integration testing measure. It must compile and it must contain meaningful first party additions to the code base. All files on our repo must belong to us.

Commits made should ideally contain jira compliant format. The reference docs on this can be found here. https://confluence.atlassian.com/fisheye/using-smart-commits-960155400.html

Code must be standard C# style compliant. For reference see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions

Commenting must be sufficient but succinct. The format would be the XML documentation native to Visual Studio. Reference documentation on the style we use is found here. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments

Scripts written by us should be within our namespace. Appropriate subspaces can be made as needed.
