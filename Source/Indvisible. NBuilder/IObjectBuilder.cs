namespace Indvisible.NBuilder
{
    using System;

    using FizzWare.NBuilder.PropertyNaming;

    /// <summary>
    /// The ObjectBuilder interface.
    /// </summary>
    public interface IObjectBuilder : IBuildable
    {
        IObjectBuilder WithPropertyNamer(IPropertyNamer propertyNamer);

        dynamic Construct(Type type);

        void Name(dynamic obj);
    }
}