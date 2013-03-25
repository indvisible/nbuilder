using System;

namespace Indvisible.DataGen
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using FizzWare.NBuilder;
    using FizzWare.NBuilder.Implementation;
    using FizzWare.NBuilder.PropertyNaming;

    using Indvisible.NBuilder;
    using Indvisible.RealisticData;

    using IReflectionUtil = FizzWare.NBuilder.Implementation.IReflectionUtil;
    using ReflectionUtil = FizzWare.NBuilder.Implementation.ReflectionUtil;

    public class RealisticPropertyNamer : IPropertyNamer
    {
        protected readonly IReflectionUtil _reflectionUtil;

        protected const BindingFlags FLAGS = (BindingFlags.Public | BindingFlags.Instance);

        public RealisticPropertyNamer()
        {
            _reflectionUtil = Activator.CreateInstance<ReflectionUtil>();
            _random = new Random();
        }

        private int _sequenceNumber;

        private Random _random;

        public void SetValuesOfAllIn<T>(IList<T> objects)
        {
            _sequenceNumber = 1;

            var type = typeof(T);

            foreach (var obj in objects)
            {
                foreach (var propertyInfo in type.GetProperties(FLAGS).Where(p => p.CanWrite))
                {
                    SetMemberValue(propertyInfo, obj);
                }

                foreach (var fieldInfo in type.GetFields().Where(f => !f.IsLiteral))
                {
                    SetMemberValue(fieldInfo, obj);
                }

                _sequenceNumber++;
            }
        }

        public void SetValuesOfAllIn(Type type, IEnumerable<dynamic> objects)
        {
            _sequenceNumber = 1;

            foreach (var obj in objects)
            {
                foreach (var propertyInfo in type.GetProperties(FLAGS).Where(p => p.CanWrite))
                {
                    SetMemberValue(propertyInfo, obj);
                }

                foreach (var fieldInfo in type.GetFields().Where(f => !f.IsLiteral))
                {
                    SetMemberValue(fieldInfo, obj);
                }

                _sequenceNumber++;
            }
        }

        public virtual void SetValuesOf<T>(T obj)
        {
            var type = typeof(T);

            foreach (var propertyInfo in type.GetProperties(FLAGS).Where(p => p.CanWrite))
            {
                SetMemberValue(propertyInfo, obj);
            }

            foreach (var propertyInfo in type.GetFields().Where(f => !f.IsLiteral))
            {
                SetMemberValue(propertyInfo, obj);
            }
        }


        public virtual void SetValuesOf(Type type, object obj)
        {
            foreach (var propertyInfo in type.GetProperties(FLAGS).Where(p => p.CanWrite))
            {
                SetMemberValue(propertyInfo, obj);
            }

            foreach (var propertyInfo in type.GetFields().Where(f => !f.IsLiteral))
            {
                SetMemberValue(propertyInfo, obj);
            }
        }

        protected static object GetCurrentValue<T>(MemberInfo memberInfo, T obj)
        {
            object currentValue = null;

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                currentValue = fieldInfo.GetValue(obj);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                try
                {
                    if (propertyInfo.GetGetMethod() != null)
                    {
                        currentValue = propertyInfo.GetValue(obj, null);
                    }
                }
                catch (Exception)
                {
                    Trace.WriteLine(string.Format("NBuilder warning: {0} threw an exception when attempting to read its current value", memberInfo.Name));
                }
            }

            return currentValue;
        }

        protected static object GetCurrentValue(Type type, MemberInfo memberInfo, object obj)
        {
            object currentValue = null;

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                currentValue = fieldInfo.GetValue(obj);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                try
                {
                    if (propertyInfo.GetGetMethod() != null)
                    {
                        currentValue = propertyInfo.GetValue(obj, null);
                    }
                }
                catch (Exception)
                {
                    Trace.WriteLine(string.Format("NBuilder warning: {0} threw an exception when attempting to read its current value", memberInfo.Name));
                }
            }

            return currentValue;
        }

        protected static Type GetMemberType(MemberInfo memberInfo)
        {
            Type type = null;

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                type = fieldInfo.FieldType;
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                type = propertyInfo.PropertyType;
            }

            if (type != null && isNullableType(type))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        protected void SetValue<T>(MemberInfo memberInfo, T obj, object value)
        {
            if (value == null)
            {
                return;
            }

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, value);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo == null)
            {
                return;
            }

            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, value, null);
            }
        }

        protected void SetValue(Type type, MemberInfo memberInfo, object obj, object value)
        {
            if (value == null)
            {
                return;
            }

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, value);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo == null)
            {
                return;
            }

            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, value, null);
            }
        }

        private static bool isNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        protected bool ShouldIgnore(MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                if (BuilderSetup.ShouldIgnoreProperty(propertyInfo))
                {
                    return true;
                }
            }

            return false;
        }

        protected void SetMemberValue<T>(MemberInfo memberInfo, T obj)
        {
            var type = GetMemberType(memberInfo);

            if (ShouldIgnore(memberInfo))
            {
                return;
            }

            var currentValue = GetCurrentValue(memberInfo, obj);

            if (!_reflectionUtil.IsDefaultValue(currentValue))
            {
                return;
            }

            object value = null;

            if (type == typeof(short))
            {
                value = GetInt16(memberInfo);
            }
            else if (type == typeof(int))
            {
                value = GetInt32(memberInfo);
            }
            else if (type == typeof(long))
            {
                value = GetInt64(memberInfo);
            }
            else if (type == typeof(decimal))
            {
                value = GetDecimal(memberInfo);
            }
            else if (type == typeof(float))
            {
                value = GetSingle(memberInfo);
            }
            else if (type == typeof(double))
            {
                value = GetDouble(memberInfo);
            }
            else if (type == typeof(ushort))
            {
                value = GetUInt16(memberInfo);
            }
            else if (type == typeof(uint))
            {
                value = GetUInt32(memberInfo);
            }
            else if (type == typeof(ulong))
            {
                value = GetUInt64(memberInfo);
            }
            else if (type == typeof(char))
            {
                value = GetChar(memberInfo);
            }
            else if (type == typeof(byte))
            {
                value = GetByte(memberInfo);
            }
            else if (type == typeof(sbyte))
            {
                value = GetSByte(memberInfo);
            }
            else if (type == typeof(DateTime))
            {
                value = GetDateTime(memberInfo);
            }
            else if (type == typeof(string))
            {
                value = GetString(memberInfo);
            }
            else if (type == typeof(bool))
            {
                value = GetBoolean(memberInfo);
            }
            else if (type.BaseType == typeof(Enum))
            {
                value = GetEnum(memberInfo);
            }
            else if (type == typeof(Guid))
            {
                value = GetGuid(memberInfo);
            }
            else
            {
                value = handleUnknownType(memberInfo);
            }

            SetValue(memberInfo, obj, value);
        }

        protected void SetMemberValue(Type type, MemberInfo memberInfo, object obj)
        {
            if (ShouldIgnore(memberInfo))
            {
                return;
            }

            var currentValue = GetCurrentValue(memberInfo, obj);

            if (!_reflectionUtil.IsDefaultValue(currentValue))
            {
                return;
            }

            object value = null;

            if (type == typeof(short))
            {
                value = GetInt16(memberInfo);
            }
            else if (type == typeof(int))
            {
                value = GetInt32(memberInfo);
            }
            else if (type == typeof(long))
            {
                value = GetInt64(memberInfo);
            }
            else if (type == typeof(decimal))
            {
                value = GetDecimal(memberInfo);
            }
            else if (type == typeof(float))
            {
                value = GetSingle(memberInfo);
            }
            else if (type == typeof(double))
            {
                value = GetDouble(memberInfo);
            }
            else if (type == typeof(ushort))
            {
                value = GetUInt16(memberInfo);
            }
            else if (type == typeof(uint))
            {
                value = GetUInt32(memberInfo);
            }
            else if (type == typeof(ulong))
            {
                value = GetUInt64(memberInfo);
            }
            else if (type == typeof(char))
            {
                value = GetChar(memberInfo);
            }
            else if (type == typeof(byte))
            {
                value = GetByte(memberInfo);
            }
            else if (type == typeof(sbyte))
            {
                value = GetSByte(memberInfo);
            }
            else if (type == typeof(DateTime))
            {
                value = GetDateTime(memberInfo);
            }
            else if (type == typeof(string))
            {
                value = GetString(memberInfo);
            }
            else if (type == typeof(bool))
            {
                value = GetBoolean(memberInfo);
            }
            else if (type.BaseType == typeof(Enum))
            {
                value = GetEnum(memberInfo);
            }
            else if (type == typeof(Guid))
            {
                value = GetGuid(memberInfo);
            }
            else
            {
                value = handleUnknownType(memberInfo);
            }

            SetValue(memberInfo, obj, value);
        }

        /// <summary>
        /// Gets the new sequence number taking into account a maximum value.
        /// 
        /// If the current sequence number is above the maximum value it will 
        /// reset it to one, and continue the sequence from there until the maximum 
        /// value is reached again.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="maxValue">The max value.</param>
        /// <returns></returns>
        private static int GetNewSequenceNumber(int sequenceNumber, int maxValue)
        {
            var newSequenceNumber = sequenceNumber % maxValue;
            if (newSequenceNumber == 0)
            {
                newSequenceNumber = maxValue;
            }

            return newSequenceNumber;
        }

        protected short GetInt16(MemberInfo memberInfo)
        {
            var newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, short.MaxValue);
            return Convert.ToInt16(newSequenceNumber);
        }

        protected int GetInt32(MemberInfo memberInfo)
        {
            return _sequenceNumber;
        }

        protected long GetInt64(MemberInfo memberInfo)
        {
            return Convert.ToInt64(_sequenceNumber);
        }

        protected decimal GetDecimal(MemberInfo memberInfo)
        {
            return Convert.ToDecimal(_sequenceNumber);
        }

        protected float GetSingle(MemberInfo memberInfo)
        {
            return Convert.ToSingle(_sequenceNumber);
        }

        protected double GetDouble(MemberInfo memberInfo)
        {
            return Convert.ToDouble(_sequenceNumber);
        }

        protected ushort GetUInt16(MemberInfo memberInfo)
        {
            return Convert.ToUInt16(_sequenceNumber);
        }

        protected uint GetUInt32(MemberInfo memberInfo)
        {
            return Convert.ToUInt32(_sequenceNumber);
        }

        protected ulong GetUInt64(MemberInfo memberInfo)
        {
            return Convert.ToUInt64(_sequenceNumber);
        }

        protected char GetChar(MemberInfo memberInfo)
        {
            int newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, 26);
            newSequenceNumber += 64;

            return Convert.ToChar(newSequenceNumber);
        }

        protected byte GetByte(MemberInfo memberInfo)
        {
            int newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, byte.MaxValue);
            return Convert.ToByte(newSequenceNumber);
        }

        protected sbyte GetSByte(MemberInfo memberInfo)
        {
            int newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, sbyte.MaxValue);

            return Convert.ToSByte(newSequenceNumber);
        }

        protected DateTime GetDateTime(MemberInfo memberInfo)
        {
            var start = new DateTime(1900, 1, 1);
            var end = DateTime.MaxValue;
            if (BuilderSetup.DateFromRestriction.HasValue)
            {
                start = BuilderSetup.DateFromRestriction.Value;
            }

            if (BuilderSetup.DateToRestriction.HasValue)
            {
                end = BuilderSetup.DateToRestriction.Value.AddDays(-1);
            }

            var range = (int)(end - start).TotalDays;
            if (range < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("Current range value: {0}", range));
            }

            var value = _random.Next(range);
            var dateTime = start.AddDays(value);
            if (dateTime >= BuilderSetup.DateToRestriction)
            {
                throw  new ArgumentOutOfRangeException(string.Format("Restriction: {0}, currentValue: {1}", BuilderSetup.DateToRestriction, dateTime));
            }

            return dateTime;

            return DateTime.Now.Date.AddDays(_sequenceNumber - 1);
        }

        protected string GetString(MemberInfo memberInfo)
        {
            var propertyName = memberInfo.Name.ToUpperInvariant();
            if (propertyName.Contains("NAME"))
            {
                if (propertyName.Contains("COMPANY"))
                {
                    return Company.GetName();
                }

                if (propertyName.Contains("FULL"))
                {
                    return Name.GetName();
                }

                if (propertyName.Contains("LAST") || propertyName.Contains("SURNAME"))
                {
                    return Name.GetLastName();
                }

                return Name.GetFirstName();
            }

            if (propertyName.Contains("ADDRESS"))
            {
                return Address.GetStreetAddress();
            }

            return memberInfo.Name + _sequenceNumber;
        }

        protected bool GetBoolean(MemberInfo memberInfo)
        {
            return (_sequenceNumber % 2) == 0;
        }

        protected Enum GetEnum(MemberInfo memberInfo)
        {
            var enumType = GetMemberType(memberInfo);
            var enumValues = getEnumValues(enumType);
            var newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, enumValues.Length);
            return Enum.Parse(enumType, enumValues.GetValue(newSequenceNumber - 1).ToString(), true) as Enum;
        }

        protected Guid GetGuid(MemberInfo memberInfo)
        {
            var bytes = new byte[16];
            var convertedBytes = BitConverter.GetBytes(_sequenceNumber);

            bytes[12] = convertedBytes[3];
            bytes[13] = convertedBytes[2];
            bytes[14] = convertedBytes[1];
            bytes[15] = convertedBytes[0];

            return new Guid(bytes);
        }

        private static Array getEnumValues(Type enumType)
        {
            var enumArray = EnumHelper.GetValues(enumType);
            return enumArray;
        }

        private dynamic handleUnknownType(MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                var propertyType = propertyInfo.PropertyType;
                var enumerableFlag = typeof(IEnumerable).IsAssignableFrom(propertyType);

                if (enumerableFlag)
                {
                    var size = _random.Next(10);
                    var typeInCollection = propertyType.GetGenericArguments()[0];
                    var constructor = typeof(List<>).MakeGenericType(typeInCollection).GetConstructor(new[] { typeof(int) });
                    if (constructor != null)
                    {
                        dynamic list = constructor.Invoke(new object[] { size });
                        var objectBuilder = BuilderNested.CreateNew(typeInCollection, this);
                        for (var i = 0; i < size; i++)
                        {
                            list.Add(objectBuilder.Build());
                        }

                        return list;
                    }

                    throw new BuilderException("Constructor is null");

                    // Type propertyType = propertyInfo.PropertyType;
                    // object obj123 = typeof(Builder<>).MakeGenericType(propertyType).GetMethod("CreateListOfSize", new[] { typeof(int) }).Invoke(null, new object[] { size });
                    // typeof(ListBuilder<>).MakeGenericType(propertyType).GetConstructor(BindingFlags.Public, null, )
                    // new ListBuilder<T>(size, propertyNamer, new ReflectionUtil());
                    // var reflectionUtil = new ReflectionUtil();
                    // var propertyNamer = new FakePropertyNamer(reflectionUtil);
                    // var constructor = typeof(ListBuilder<>).MakeGenericType(propertyType).GetConstructor(new[] { typeof(int), typeof(IPropertyNamer), typeof(IReflectionUtil) });
                    //  
                    //    dynamic listBuilder = constructor.Invoke(new object[] { size, propertyNamer, reflectionUtil });
                    //    var result = listBuilder.Build();
                    //    //object result = listBuilder.GetType().GetMethod("Build").Invoke(null, null);
                    //
                    //    //object fake = method.Invoke(null, new object[] { size });
                    //    //MethodInfo methodInfo = typeof(ListBuilder<>).MakeGenericType(propertyType).GetMethod("Build", BindingFlags.Public);
                    //    //object invoke = methodInfo.Invoke(null, null);
                    //
                    //    return result;
                }

                var build = BuilderNested.CreateNew(propertyType, this).Build();
                return build;
            }

            return null;
        }
    }
}