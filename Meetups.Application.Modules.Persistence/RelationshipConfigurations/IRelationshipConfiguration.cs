namespace Meetups.Application.Modules.Persistence.RelationshipConfigurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal interface IRelationshipConfiguration<TLeftEntity, TRightEntity>
    where TLeftEntity : class
    where TRightEntity : class
{
    void Configure(EntityTypeBuilder<TLeftEntity> leftEntity, EntityTypeBuilder<TRightEntity> rightEntity);
}

internal interface IRelationshipConfiguration<TEntity>
    where TEntity : class
{
    void Configure(EntityTypeBuilder<TEntity> leftEntity);
}