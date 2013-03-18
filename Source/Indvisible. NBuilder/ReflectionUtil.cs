namespace Indvisible.NBuilder
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FizzWare.NBuilder;

    /// <summary>
    /// The reflection util.
    /// </summary>
    public class ReflectionUtil : IReflectionUtil
    {
        /// <summary>
        /// The create instance of type.
        /// </summary>
        /// <param name="type">
        /// The type to creation.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="TypeCreationException">
        /// </exception>
        public object CreateInstanceOf(Type type)
        {
            try
            {
                return Activator.CreateInstance(type, true);
            }
            catch (MissingMethodException e)
            {
                throw new TypeCreationException(type.Name + " does not have a default parameterless constructor", e);
            }
        }

        /// <summary>
        /// The create instance of type.
        /// </summary>
        /// <param name="type">
        /// The type to creation.
        /// </param>
        /// <param name="args">
        /// The arguments.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="TypeCreationException">
        /// </exception>
        public object CreateInstanceOf(Type type, params object[] args)
        {
            try
            {
                var instance = Activator.CreateInstance(type, args);
                return instance;
            }
            catch (MissingMethodException e)
            {
                var argList = string.Join(", ", args.Select(o => o.GetType().Name).ToArray());
                throw new TypeCreationException("Constructor with args " + argList, e);
            }
        }

        public bool RequiresConstructorArgs(Type type)
        {
            if (type.IsValueType)
            {
                return false;
            }

            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var constructorInfo in constructors)
            {
                if (constructorInfo.GetParameters().Length == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsDefaultValue(object value)
        {
            if (value is ValueType)
            {
                var type = value.GetType();

                var defaultValue = Activator.CreateInstance(type);

                return value.Equals(defaultValue);
            }

            return value == null;
        }
    }
}