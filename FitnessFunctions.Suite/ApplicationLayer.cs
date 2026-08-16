using MediatR;
using Mono.Cecil;
using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace FitnessFunctions.Suite
{
    public class ApplicationLayer
    {
        private const string ApplicationNamespace = "Application";

        [Fact]
        public void CommandsAndQueries_ShouldBeClearlyDefined()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var commandResult = Types.InCurrentDomain().That().HaveNameEndingWith("Command")
                .Should().ResideInNamespaceStartingWith(ApplicationNamespace)
                .And().ImplementInterface(typeof(IRequest)).Or().ImplementInterface(typeof(IRequest<>))
                .GetResult();

            var queryResult = Types.InCurrentDomain().That().HaveNameEndingWith("Query")
                .Should().ResideInNamespaceStartingWith(ApplicationNamespace)
                .And().ImplementInterface(typeof(IRequest)).Or().ImplementInterface(typeof(IRequest<>))
                .GetResult();

            Assert.True(commandResult.IsSuccessful, "Commands should be clearly defined and implement IRequest.");
            Assert.True(queryResult.IsSuccessful, "Queries should be clearly defined and implement IRequest.");
        }


        [Fact]
        public void CommandAndQueryHandlers_ShouldBeClearlyDefined()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var commandHandlerResult = Types.InCurrentDomain().That().HaveNameEndingWith("CommandHandler")
                .Should().ResideInNamespaceStartingWith(ApplicationNamespace)
                .And().ImplementInterface(typeof(IRequestHandler<>)).Or().ImplementInterface(typeof(IRequestHandler<,>))
                .GetResult();

            var queryHandlerResult = Types.InCurrentDomain().That().HaveNameEndingWith("QueryHandler")
                .Should().ResideInNamespaceStartingWith(ApplicationNamespace)
                .And().ImplementInterface(typeof(IRequestHandler<>)).Or().ImplementInterface(typeof(IRequestHandler<,>))
                .GetResult();

            Assert.True(commandHandlerResult.IsSuccessful, "Command handlers should be clearly defined and implement IRequestHandler.");
            Assert.True(queryHandlerResult.IsSuccessful, "Query handlers should be clearly defined and implement IRequestHandler.");
        }

        [Fact]
        public void Handlers_ShouldHaveExpectedEffects()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var commandHandlerResult = Types.InCurrentDomain().That().HaveNameEndingWith("CommandHandler")
                .Should().MeetCustomRule(new ValidCommandHandlerRule())
                .GetResult();

            Assert.True(commandHandlerResult.IsSuccessful, "Command handlers should have database context injected and take a Command as input.");

            var queryHandlerResult = Types.InCurrentDomain().That().HaveNameEndingWith("QueryHandler")
                .Should().MeetCustomRule(new ValidQueryHandlerRule())
                .GetResult();

            Assert.True(queryHandlerResult.IsSuccessful, "Query handlers should have database context injected, take a Query as input and not return an Entity directly.");
        }

        [Fact]
        public void ApplicationLayer_ShouldNotImplementDeclaredInterfaces()
        {
            Assembly.Load("Domain");
            Assembly.Load("Application");
            Assembly.Load("Infrastructure");
            Assembly.Load("Web");
            var interfaces = Types.InNamespace(ApplicationNamespace)
                .That().AreInterfaces().GetTypes();

            var classes = Types.InNamespace(ApplicationNamespace)
                .That().AreClasses().GetTypes();

            var result = true;
            foreach (var i in interfaces)
            {
                foreach (var c in classes)
                {
                    if (c.GetInterfaces().Contains(i))
                    {
                        result = false;
                        break;
                    }
                }
                if (!result) break;
            }

            Assert.True(result, "Application Layer should not include interface implementations.");
        }
    }

    internal sealed class ValidCommandHandlerRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            foreach (var iface in type.Interfaces)
            {
                if (iface.InterfaceType.IsGenericInstance)
                {
                    var name = ((GenericInstanceType)iface.InterfaceType).ElementType.FullName;
                    if (name.Contains("MediatR.IRequestHandler"))
                    {
                        var genericArgs = ((GenericInstanceType)iface.InterfaceType).GenericArguments;
                        if (genericArgs.Count == 2)
                        {
                            var commandType = genericArgs[0];
                            var returnType = genericArgs[1];
                            return HasDatabaseContextConstructorParameter(type)
                                && commandType.Name.EndsWith("Command")
                                && (returnType.FullName == "System.Int32" || returnType.FullName == "System.String" || returnType.FullName == "MediatR.Unit");
                        }
                        else
                        {
                            return genericArgs[0].Name.EndsWith("Command");
                        }
                    }
                }
            }
            return false;
        }

        private bool HasDatabaseContextConstructorParameter(TypeDefinition type)
        {
            var constructorParams = type.Methods.Where(m => m.IsConstructor).First().Parameters;
            return constructorParams.Any(p => p.ParameterType.FullName.EndsWith("IApplicationDbContext"));
        }
    }

    internal sealed class ValidQueryHandlerRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            foreach (var iface in type.Interfaces)
            {
                if (iface.InterfaceType.IsGenericInstance)
                {
                    var genericInstance = (GenericInstanceType)iface.InterfaceType;
                    var name = genericInstance.ElementType.FullName;
                    if (name.Contains("MediatR.IRequestHandler"))
                    {
                        var genericArgs = genericInstance.GenericArguments;
                        if (genericArgs.Count == 2)
                        {
                            var queryType = genericArgs[0];
                            var returnType = genericArgs[1];
                            return queryType.Name.EndsWith("Query") && IsDto(returnType);
                        }
                    }
                }
            }
            return false;
        }

        private bool IsDto(TypeReference type)
        {
            return type.IsGenericInstance
                ? IsDto(((GenericInstanceType)type).GenericArguments[0])
                : type.FullName.StartsWith("Application") && !IsDomainEntity(type);
        }

        private bool IsDomainEntity(TypeReference type)
        {
            var resolvedType = type.Resolve();
            return (resolvedType.BaseType != null && resolvedType.BaseType.FullName.Contains("BaseEntity")) || resolvedType.Interfaces.Any(i => i.InterfaceType.FullName.Contains("BaseEntity"));

        }
    }
}
