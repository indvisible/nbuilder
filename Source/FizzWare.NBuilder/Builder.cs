using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;

namespace FizzWare.NBuilder
{
    using System;
    using System.Collections.Generic;

    public class Builder<T>
    {
        public static ISingleObjectBuilder<T> CreateNew()
        {
            InitChain();
            var reflectionUtil = new ReflectionUtil();
            var propertyNamer = BuilderSetup.GetPropertyNamerFor<T>();
            return new ObjectBuilder<T>(reflectionUtil).WithPropertyNamer(propertyNamer);
        }

        public static ISingleObjectBuilder<T> CreateNew(IPropertyNamer propertyNamer)
        {
            InitChain();
            return new ObjectBuilder<T>(new ReflectionUtil()).WithPropertyNamer(propertyNamer);
        }

        public static IListBuilder<T> CreateListOfSize(int size)
        {
            Guard.Against(size < 1, "Size of list must be 1 or greater");
            InitChain();
            var propertyNamer = BuilderSetup.GetPropertyNamerFor<T>();
            return CreateListOfSize(size, propertyNamer);
        }

        public static IListBuilder<T> CreateListOfSize(int size, IPropertyNamer propertyNamer)
        {
            Guard.Against(size < 1, "Size of list must be 1 or greater");
            InitChain();
            return new ListBuilder<T>(size, propertyNamer, new ReflectionUtil());
        }

        private static void InitChain()
        {
            BuilderSetup.ClassesChain = new Dictionary<Type, string> { { typeof(T), "main node" } };
        }
    }
}