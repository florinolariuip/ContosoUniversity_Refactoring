using Microsoft.AspNetCore.Diagnostics;
using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace FitnessFunctions.Suite
{
    public class Overall
    {
        private const string DomainNamespace = "Domain";
        private const string ApplicationNamespace = "Application";
        private const string InfrastructureNamespace = "Infrastructure";
        private const string WebNamespace = "Web";
        private const string CurrentNamespace = "FitnessFunctions";


        [Fact]
        public void ShouldHaveDefinedCleanArchitectureLayers()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var layers = new[] { DomainNamespace, ApplicationNamespace, InfrastructureNamespace, WebNamespace, CurrentNamespace };
            var dependenciesNamespacesPrefixes = new[] { "Mono", "Newtonsoft", "NuGet", "AutoMapper", "MediatR" };

            var namespaces = Types.InCurrentDomain().GetTypes()
                .Select(t => t.Namespace)
                .Where(ns => !string.IsNullOrEmpty(ns) && dependenciesNamespacesPrefixes.All(prefix => !ns.StartsWith(prefix)))
                .Distinct()
                .OrderBy(ns => ns)
                .ToList();


            Assert.True(
                namespaces.All(ns => ns != null && layers.Any(layer => ns.StartsWith(layer, StringComparison.OrdinalIgnoreCase))),
                "The expected layers do not match the namespaces found in the assembly");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [Fact]
        public void DomainLayer_ShouldNotDependOnOtherLayers()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(DomainNamespace)
                .ShouldNot().HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, WebNamespace)
                .GetResult();

            Assert.True(result.IsSuccessful, "Domain layer should not depend on any other layers");
        }

        [Fact]
        public void ApplicationLayer_ShouldNotDependOnInfrastructureOrPresentation()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(ApplicationNamespace)
                .ShouldNot().HaveDependencyOnAny(InfrastructureNamespace, WebNamespace)
                .GetResult();

            Assert.True(result.IsSuccessful, "Application layer should not depend on Infrastructure layer or Web layer");
        }

        [Fact]
        public void InfrastructureLayer_ShouldNotDependOnPresentation()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(InfrastructureNamespace)
                .ShouldNot().HaveDependencyOnAny(WebNamespace)
                .GetResult();

            Assert.True(result.IsSuccessful, "Infrastructure layer should not depend on Web layer");
        }

        [Fact]
        public void PresentationLayer_ShouldDependOnApplicationAndInfrastructure()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(WebNamespace)
                .That()
                .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace)
                .GetTypes();

            Assert.True(result.Any(), "Web layer should have at least one class that depends on Application or Infrastructure layers.");
        }

        [Fact]
        public void ApplicationLayer_MethodsShouldDeclareLessThan10LocalVariables()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var methods = Types.InNamespace(ApplicationNamespace).That().AreClasses().GetTypes()
                .SelectMany(t => t.GetMethods());

            var result = methods.All(m => m.GetMethodBody() == null || m.GetMethodBody()!.LocalVariables.Count <= 10);

            Assert.True(result, "Business logic constraint is violated.");
        }
    }
}
