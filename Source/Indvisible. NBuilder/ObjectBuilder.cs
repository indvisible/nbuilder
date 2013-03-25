namespace Indvisible.NBuilder
{
    using System;

    using FizzWare.NBuilder;
    using FizzWare.NBuilder.PropertyNaming;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly Type _type;

        private readonly IReflectionUtil _reflectionUtil;

        private IPropertyNamer _propertyNamer;

        private object[] constructorArgs;

        public ObjectBuilder(Type type, IReflectionUtil reflectionUtil)
        {
            _type = type;
            _reflectionUtil = reflectionUtil;
        }

        public IObjectBuilder WithPropertyNamer(IPropertyNamer thePropertyNamer)
        {
            _propertyNamer = thePropertyNamer;
            return this;
        }

        public object Build()
        {
            var obj = Construct(_type);
            Name(obj);
            return obj;
        }

        public dynamic Construct(Type type)
        {
            if (type.IsInterface)
            {
                throw new TypeCreationException("Cannot build an interface");
            }

            if (type.IsAbstract)
            {
                throw new TypeCreationException("Cannot build an abstract class");
            }

            var obj = _reflectionUtil.CreateInstanceOf(type);
            return obj;
        }

        public void Name(dynamic obj)
        {
            if (BuilderSetup.AutoNameProperties)
            {
                _propertyNamer.SetValuesOf(obj);
            }
        }
    }
}