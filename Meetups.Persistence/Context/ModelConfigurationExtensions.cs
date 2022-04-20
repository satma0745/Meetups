namespace Meetups.Persistence.Context;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Meetups.Persistence.RelationshipConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class ModelConfigurationExtensions
{
    public static ModelBuilder ApplyEntityTypeConfigurations(this ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    public static ModelBuilder ApplyRelationshipConfigurations(this ModelBuilder modelBuilder)
    {
        var relationshipConfigurationsInfo = Assembly
            .GetExecutingAssembly()
            .DefinedTypes
            .Where(HasParameterlessConstructor)
            .Where(type => !type.IsGenericType)
            .SelectMany(GetRelationshipConfigurationInfo);

        foreach (var relationshipConfigurationInfo in relationshipConfigurationsInfo)
        {
            modelBuilder.ApplyRelationshipConfiguration(relationshipConfigurationInfo);
        }
        
        return modelBuilder;
    }

    private static bool HasParameterlessConstructor(Type type) =>
        type.GetConstructor(Type.EmptyTypes) is not null;

    /// <remarks>Gets <b>at most one</b> relationship configuration from a type.</remarks>
    private static IEnumerable<RelationshipConfigurationInfo> GetRelationshipConfigurationInfo(Type type) =>
        type
            .GetInterfaces()
            .Where(@interface => @interface.IsGenericType)
            .Where(@interface =>
                @interface.GetGenericTypeDefinition() == typeof(IRelationshipConfiguration<>) ||
                @interface.GetGenericTypeDefinition() == typeof(IRelationshipConfiguration<,>))
            .Select(@interface => new RelationshipConfigurationInfo(type, @interface.GenericTypeArguments))
            .Take(1);

    private static void ApplyRelationshipConfiguration(
        this ModelBuilder modelBuilder,
        RelationshipConfigurationInfo relationshipConfigurationInfo)
    {
        var relationshipConfiguration = Activator.CreateInstance(relationshipConfigurationInfo.ConfigurationType);

        var configureMethod = relationshipConfigurationInfo.ConfigurationType
            .GetMethods()
            .Single(method => method.Name == nameof(IRelationshipConfiguration<object>.Configure));

        var entities = relationshipConfigurationInfo.EntityTypes
            .Select(modelBuilder.GetGenericEntityTypeBuilder)
            .Cast<object>()
            .ToArray();
        
        configureMethod.Invoke(relationshipConfiguration, entities);
    }

    private static EntityTypeBuilder GetGenericEntityTypeBuilder(this ModelBuilder modelBuilder, Type entityType)
    {
        var entityMethod = typeof(ModelBuilder)
            .GetMethods()
            .Where(method => method.Name == nameof(ModelBuilder.Entity))
            .Where(method => method.IsGenericMethod)
            .Where(method => method.GetParameters().Length == 0)
            .Select(method => method.GetGenericMethodDefinition())
            .Single(method => method.GetGenericArguments().Length == 1);

        var genericEntityMethod = entityMethod.MakeGenericMethod(entityType);
        var entityTypeBuilder = genericEntityMethod.Invoke(modelBuilder, Array.Empty<object>());

        return entityTypeBuilder as EntityTypeBuilder;
    }

    private record RelationshipConfigurationInfo(Type ConfigurationType, Type[] EntityTypes);
}