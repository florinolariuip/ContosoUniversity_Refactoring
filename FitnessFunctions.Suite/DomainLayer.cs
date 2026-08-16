using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace FitnessFunctions.Suite {
    public class DomainLayer
    {
        private const string DomainNamespace = "Domain";

        [Fact]
        public void BaseEntity_ShouldExistInDomainLayerAndBeAbstract()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(DomainNamespace)
                              .That().AreClasses()
                              .And().HaveName("BaseEntity")
                              .GetTypes();

            Assert.True(result.Count().Equals(1), "There should be exactly one BaseEntity class in the Domain layer.");
            Assert.True(result.First().IsAbstract, "BaseEntity should be an abstract class.");
        }

        [Fact]
        public void Entities_ShouldHaveAnIdPropertyAndBeInsideDomain()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var entitiesInDomainResult = Types.InCurrentDomain()
                    .That().AreClasses()
                    .And().DoNotResideInNamespace(DomainNamespace)
                    .GetTypes();

            Assert.True(entitiesInDomainResult.Where(e => e.BaseType != null && e.BaseType.Name.Equals("BaseEntity")).Count().Equals(0),
                "All entities should reside in the Domain layer.");

            var classes = Types.InCurrentDomain()
                                .That().AreClasses()
                                .GetTypes();

            var entitiesTypes = classes.Where(e => e.BaseType != null && e.BaseType.Name.Equals("BaseEntity"));

            foreach (var type in entitiesTypes)
            {
                var hasIdPropertyResult = type.GetProperties().Any(prop => prop.Name == "Id");
                Assert.True(hasIdPropertyResult, $"Entity {type.Name} should have an Id property.");
            }
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [Fact]
        public void Enums_ShouldBeDeclaredInTheDomainLayer()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InCurrentDomain()
                .That().ResideInNamespace(DomainNamespace)
                .Or().ResideInNamespace("Application")
                .Or().ResideInNamespace("Infrastructure")
                .Or().ResideInNamespace("Web")
                              .GetTypes();

            var enums = result.Where(t => t.IsEnum && !t.Namespace!.StartsWith(DomainNamespace)).ToList();

            Assert.True(enums.Count().Equals(0), "All enums should reside within the Domain Layer.");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [Fact]
        public void DomainLayer_ShouldNotDependOnExternalLibrariesAndFrameworks()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var result = Types.InNamespace(DomainNamespace)
                              .That().AreClasses()
                              .Should().NotHaveDependencyOn("Microsoft.EntityFrameworkCore")
                              // The above list should be adapted to the application's needs
                              .GetResult();

            Assert.True(result.IsSuccessful, "Classes in the Domain Layer should not depend on unwanted external libraries.");
        }

        [Fact]
        public void DomainLayer_ClassesShouldHaveMethodsAndPropertiesPocoOrAggregated()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var pocoTypes = new HashSet<String>{"void","bool","boolean","byte","char","decimal","double","idictionary",
                                              "float","int","int32","long","sbyte","short","string","methodbase",
                                              "uint","ulong","ushort","datetime","timespan","type","exception",
                                              "ienumerable","ilist","icollection","typecode","nullable`1","datetimeoffset",
                                              "ireadonlycollection`1","ilist`1","icollection`1","byte[]"};

            var types = Types.InNamespace(DomainNamespace)
                             .That().AreClasses()
                             .GetTypes();

            foreach (var type in types)
            {
                foreach (var property in type.GetProperties())
                {
                    Assert.True(pocoTypes.Any(p => property.PropertyType.Name.ToLower().EndsWith(p))
                        || property.PropertyType.Namespace!.StartsWith(DomainNamespace),
                        $"Property {property.Name} in class {type.Name} is of type {property.PropertyType}, which is not an allowed type.");
                }

                foreach (var method in type.GetMethods())
                {
                    Assert.True(pocoTypes.Any(p => method.ReturnType.Name.ToLower().EndsWith(p))
                        || method.ReturnType.Namespace!.StartsWith(DomainNamespace),
                        $"Method {method.Name} in class {type.Name} returns {method.ReturnType}, which is not an allowed type.");
                }
            }
        }
    }
}
