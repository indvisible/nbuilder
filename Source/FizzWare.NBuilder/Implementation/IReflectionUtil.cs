namespace FizzWare.NBuilder.Implementation
{
    using System;

    public interface IReflectionUtil
    {
        T CreateInstanceOf<T>();

        T CreateInstanceOf<T>(params object[] args);

        bool RequiresConstructorArgs(Type type);

        bool IsDefaultValue(object value);
    }
}