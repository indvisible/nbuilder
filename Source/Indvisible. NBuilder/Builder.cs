namespace Indvisible.NBuilder
{
    using System;

    using FizzWare.NBuilder;
    using FizzWare.NBuilder.Implementation;
    using FizzWare.NBuilder.PropertyNaming;

    public class BuilderNested
    {
        public static IObjectBuilder CreateNew(Type type)
        {
            var reflectionUtil = new ReflectionUtil();
            var propertyNamer = BuilderSetup.GetPropertyNamerFor(type);
            return new ObjectBuilder(type, reflectionUtil).WithPropertyNamer(propertyNamer);
        }

        public static IObjectBuilder CreateNew(Type type, IPropertyNamer propertyNamer)
        {
            return new ObjectBuilder(type, new ReflectionUtil()).WithPropertyNamer(propertyNamer);
        }
    }
}