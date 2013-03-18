namespace Indvisible.NBuilder
{
    using System;

    public interface IReflectionUtil
    {
        object CreateInstanceOf(Type type);

        object CreateInstanceOf(Type type, params object[] args);

        bool RequiresConstructorArgs(Type type);

        bool IsDefaultValue(object value);
    }
}