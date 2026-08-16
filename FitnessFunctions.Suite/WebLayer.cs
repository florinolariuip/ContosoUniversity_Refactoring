using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace FitnessFunctions.Suite
{
    public class WebLayer
    {
        private const string ApplicationNamespace = "Application";
        private const string WebNamespace = "Web";

        [Fact]
        public void WebLayer_ShouldContainControllersOrEndpointGroups()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var types = Types.InCurrentDomain().That().AreClasses().GetTypes()
                .Where(t => t.Name.EndsWith("Controller")
                || (t.BaseType != null && t.BaseType.Name.Equals("EndpointGroupBase")));

            Assert.True(types.Any(),
                "Endpoints should be contained in either Controllers or Endpoint Groups.");
            Assert.True(types.All(t => t.Namespace!.StartsWith(WebNamespace)),
                "All Controllers or Endpoint Groups should reside in the Web Layer.");
        }

        [Fact]
        public void WebLayerEndpoints_ShouldHaveProperReturnTypes()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var methods = Types.InNamespace(WebNamespace).That().AreClasses().GetTypes()
                .Where(t => (t.Name.EndsWith("Controller") && !t.Name.Contains("BaseController")) || (t.BaseType != null && t.BaseType.Name.Equals("EndpointGroupBase")))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var validReturnTypes = methods.All(m => IsValidReturnType(m.ReturnType));

            Assert.True(validReturnTypes, "Methods should return either IActionResult, Void, a DTO, an external dependency types, or a primitive types, possibly wrapped in Task<>.");
        }

        private bool IsValidReturnType(Type returnType)
        {
            var primitiveTypes = new HashSet<string>{"System.Boolean","System.Byte","System.SByte","System.Char","System.Decimal",
                                 "System.Double","System.Single","System.Int32","System.UInt32","System.Int64","System.UInt64",
                                 "System.Int16", "System.UInt16", "System.String"};
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var genericArgument = returnType.GetGenericArguments()[0];
                if (genericArgument.IsGenericType &&
                    (genericArgument.GetGenericTypeDefinition() == typeof(IEnumerable<>) || genericArgument.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    genericArgument = genericArgument.GetGenericArguments()[0];
                }
                return genericArgument.Namespace!.StartsWith(ApplicationNamespace) || genericArgument.Namespace.StartsWith("Microsoft.AspNetCore")
                    || primitiveTypes.Contains(genericArgument.FullName!) || genericArgument.Name.Equals("IActionResult") || genericArgument.Name!.Equals("Void");
            }
            return returnType.Namespace!.StartsWith(ApplicationNamespace) || returnType.Namespace.StartsWith("Microsoft.AspNetCore")
                    || primitiveTypes.Contains(returnType.FullName!) || returnType.Name.Equals("IActionResult") || returnType.Name!.Equals("Void");
        }


        [Fact]
        public void CreateAndUpdateEndpoints_ShouldWorkWithCommands()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var createAndUpdateMethods = Types.InNamespace(WebNamespace).That().AreClasses().GetTypes()
                .Where(t => t.Name.EndsWith("Controller")
                      || (t.BaseType != null && t.BaseType.Name.Equals("EndpointGroupBase")))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Update"));

            var validParameters = createAndUpdateMethods.All(m => m.GetParameters().Any(p =>
                                  p.ParameterType.Name.EndsWith("Command")
                                  && p.ParameterType.Namespace!.StartsWith(ApplicationNamespace)) || m.GetParameters().Count() == 0);

            Assert.True(validParameters, "Create and Update Endpoints should work with Commands.");
        }
    }
}
