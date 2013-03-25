namespace Indvisible.NBuilder
{
    using System;

    using FizzWare.NBuilder.PropertyNaming;

    public class BuilderNested
    {
        public static IObjectBuilder CreateNew(Type type, IPropertyNamer propertyNamer)
        {
            return new ObjectBuilder(type, new ReflectionUtil()).WithPropertyNamer(propertyNamer);
        }
    }
}