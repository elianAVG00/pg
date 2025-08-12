using System;
using System.Reflection;

namespace PGMainService.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);
        string GetDocumentationExample(MemberInfo member);
        string GetDocumentation(Type type);
    }
}