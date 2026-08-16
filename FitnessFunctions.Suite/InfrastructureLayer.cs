using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace FitnessFunctions.Suite
{
    public class InfrastructureLayer
    {
        private const string ApplicationNamespace = "Application";
        private const string InfrastructureNamespace = "Infrastructure";

        [Fact]
        public void InfrastructureLayer_ImplementsInterfacesFromApplicationLayer()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var applicationInterfacesTypes = Types.InCurrentDomain().That().ResideInNamespace(ApplicationNamespace)
                                             .And().AreInterfaces().GetTypes();

            var infrastructureTypes = Types.InNamespace(InfrastructureNamespace).GetTypes();

            var areApplicationInterfacesImplemented = applicationInterfacesTypes.Any(appInterface =>
                                      infrastructureTypes.Any(infraType => infraType.GetInterfaces().Contains(appInterface)));

            Assert.True(areApplicationInterfacesImplemented,"Some application interfaces should be implemented in the Infrastructure Layer.");
        }

        [Fact]
        public void InfrastructureLayer_ShouldDependOnExternalFrameworksAndImplementRepositoryOrConfigurationPattern()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var baseEntityAssembly = Types.InCurrentDomain().That().HaveNameEndingWith("BaseEntity").GetTypes().First();
            var externalDependencies = Types.InNamespace(InfrastructureNamespace).GetTypes()
                                          .SelectMany(t => t.GetTypeInfo().Assembly.GetReferencedAssemblies())
                                          .Any(reference => reference.FullName.StartsWith("Microsoft.EntityFrameworkCore")
                                                         || reference.FullName.StartsWith("Microsoft.AspNetCore"));


            Assert.True(externalDependencies, "The Infrastructure Layer must depend on external dependencies.");

            var configurationImplemented = Types.InNamespace(InfrastructureNamespace).GetTypes()
                              .Any(type => type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType
                                                                 && interfaceType.Name.StartsWith("IEntityTypeConfiguration")
                                                                 && baseEntityAssembly.IsAssignableFrom(interfaceType.GenericTypeArguments[0])));

            var repositoryImplemented = Types.InNamespace(InfrastructureNamespace).GetTypes()
                              .Any(type => type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType
                                                                 && interfaceType.Name.StartsWith("IRepository")
                                                                 && baseEntityAssembly.IsAssignableFrom(interfaceType.GenericTypeArguments[0])));

            Assert.True(configurationImplemented || repositoryImplemented, "It is advised to implement either the Repository or the Configuration patterns.");
        }

        [Fact]
        public void InfrastructureLayer_ShouldUseAbstractionsOrExternalDependencies()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var infrastructureTypes = Types.InNamespace(InfrastructureNamespace).GetTypes().Where(t => !t.Name.EndsWith("Initialiser"));
            // The above exception should be added according to needs. Here, it is used to initialize the application DB, so we chose to
            // exclude it from the test since it does not impact coupling or cohesion.

            var invalidDependencies = new List<string>();
            foreach (var type in infrastructureTypes)
            {
                foreach (var ctor in type.GetConstructors())
                {
                    foreach (var param in ctor.GetParameters())
                    {
                        if (!param.ParameterType.IsInterface && !param.ParameterType.IsAbstract)
                        {
                            if (param.ParameterType.Namespace!.StartsWith("Application") || param.ParameterType.Namespace!.StartsWith("Infrastructure") || param.ParameterType.Namespace!.StartsWith("Domain") || param.ParameterType.Namespace!.StartsWith("Web"))
                            {
                                invalidDependencies.Add($"Class {type.Name} is dependent on an implementation called {param.ParameterType.Name}");
                            }
                        }
                    }
                }
            }

            Assert.True(invalidDependencies.Count.Equals(0),
                $"Found invalid dependencies in: {string.Join(", ", invalidDependencies)}");
        }
    }
}
