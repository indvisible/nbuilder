using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;

namespace FizzWare.NBuilder
{
    public sealed class BuilderSetup
    {
        private static IPersistenceService persistenceService;
        public static bool AutoNameProperties;
        private static Dictionary<Type, IPropertyNamer> propertyNamers;
        private static IPropertyNamer defaultPropertyNamer;

        private static List<PropertyInfo> disabledAutoNameProperties;

        public static bool HasDisabledAutoNameProperties;

        public static DateTime? DateFromRestriction;
        public static DateTime? DateToRestriction;

        static BuilderSetup()
        {
            ResetToDefaults();
        }

        public static void ResetToDefaults()
        {
            SetDefaultPropertyNamer(new SequentialPropertyNamer(new ReflectionUtil()));
            persistenceService = new PersistenceService();
            AutoNameProperties = true;
            propertyNamers = new Dictionary<Type, IPropertyNamer>();
            HasDisabledAutoNameProperties = false;
            disabledAutoNameProperties = new List<PropertyInfo>();
            DateFromRestriction = null;
            DateToRestriction = null;
        }

        public static void SetDefaultPropertyNamer(IPropertyNamer propertyNamer)
        {
            defaultPropertyNamer = propertyNamer;
        }

        public static void SetPersistenceService(IPersistenceService service)
        {
            persistenceService = service;
        }

        public static IPersistenceService GetPersistenceService()
        {
            return persistenceService;
        }

        public static void SetCreatePersistenceMethod<T>(Action<T> saveMethod)
        {
            persistenceService.SetPersistenceCreateMethod(saveMethod);
        }

        public static void SetUpdatePersistenceMethod<T>(Action<T> saveMethod)
        {
            persistenceService.SetPersistenceUpdateMethod(saveMethod);
        }

        public static void SetPropertyNamerFor<T>(IPropertyNamer propertyNamer)
        {
            propertyNamers.Add(typeof(T), propertyNamer);
        }

        public static IPropertyNamer GetPropertyNamerFor<T>()
        {
            if (propertyNamers.ContainsKey(typeof(T)))
            {
                return propertyNamers[typeof(T)];
            }

            return defaultPropertyNamer;
        }

        public static void DisablePropertyNamingFor<T, TFunc>(Expression<Func<T, TFunc>> func)
        {
            HasDisabledAutoNameProperties = true;
            disabledAutoNameProperties.Add(GetProperty(func));
        }

        public static bool ShouldIgnoreProperty(PropertyInfo info)
        {
            if (disabledAutoNameProperties.Any(x => x.DeclaringType == info.DeclaringType && x.Name == info.Name))
                return true;

            return false;
        }

        private static PropertyInfo GetProperty<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return (PropertyInfo)memberExpression.Member;
        }

        private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            return memberExpression;
        }

        public static IPropertyNamer GetPropertyNamerFor(Type type)
        {
            if (propertyNamers.ContainsKey(type))
            {
                return propertyNamers[type];
            }

            return defaultPropertyNamer;
        }
    }
}