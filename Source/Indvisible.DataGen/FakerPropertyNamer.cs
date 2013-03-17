using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;

namespace Indvisible.DataGen
{
    public class FakerPropertyNamer : IPropertyNamer
    {
        protected readonly IReflectionUtil ReflectionUtil;
        protected const BindingFlags FLAGS = (BindingFlags.Public | BindingFlags.Instance);

        public FakerPropertyNamer()
        {
            ReflectionUtil = Activator.CreateInstance<ReflectionUtil>();
        }
        public FakerPropertyNamer(IReflectionUtil reflectionUtil)
        {
            ReflectionUtil = reflectionUtil;
        }

        private int _sequenceNumber;

        public void SetValuesOfAllIn<T>(IList<T> objects)
        {
            _sequenceNumber = 1;

            var type = typeof(T);

            foreach (T obj in objects)
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
                SetMemberValue(propertyInfo, obj);

            foreach (var propertyInfo in type.GetFields().Where(f => !f.IsLiteral))
                SetMemberValue(propertyInfo, obj);
        }

        protected static object GetCurrentValue<T>(MemberInfo memberInfo, T obj)
        {
            object currentValue = null;

            if (memberInfo is FieldInfo)
            {
                currentValue = ((FieldInfo)memberInfo).GetValue(obj);
            }

            if (memberInfo is PropertyInfo)
            {
                try
                {
                    if (((PropertyInfo)memberInfo).GetGetMethod() != null)
                    {
                        currentValue = ((PropertyInfo)memberInfo).GetValue(obj, null);
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

            if (memberInfo is FieldInfo)
            {
                type = ((FieldInfo)memberInfo).FieldType;
            }

            if (memberInfo is PropertyInfo)
            {
                type = ((PropertyInfo)memberInfo).PropertyType;
            }

            if (type != null && IsNullableType(type))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        protected virtual void SetValue<T>(MemberInfo memberInfo, T obj, object value)
        {
            if (value != null)
            {
                if (memberInfo is FieldInfo)
                    ((FieldInfo)memberInfo).SetValue(obj, value);

                if (memberInfo is PropertyInfo)
                {
                    if (((PropertyInfo)memberInfo).CanWrite)
                        ((PropertyInfo)memberInfo).SetValue(obj, value, null);
                }
            }
        }

        protected virtual bool ShouldIgnore(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                if (BuilderSetup.ShouldIgnoreProperty(((PropertyInfo)memberInfo)))
                    return true;

            return false;
        }

        protected virtual void SetMemberValue<T>(MemberInfo memberInfo, T obj)
        {
            Type type = GetMemberType(memberInfo);

            if (BuilderSetup.HasDisabledAutoNameProperties && ShouldIgnore(memberInfo))
                return;

            object currentValue = GetCurrentValue(memberInfo, obj);

            if (!ReflectionUtil.IsDefaultValue(currentValue))
                return;

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
                HandleUnknownType(type, memberInfo, obj);
            }

            SetValue(memberInfo, obj, value);
        }

        private void HandleUnknownType(Type type, MemberInfo memberInfo, object o)
        {
            throw new NotImplementedException();
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
            int newSequenceNumber = sequenceNumber % maxValue;
            if (newSequenceNumber == 0)
            {
                newSequenceNumber = maxValue;
            }

            return newSequenceNumber;
        }

        protected short GetInt16(MemberInfo memberInfo)
        {
            int newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, short.MaxValue);
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
            return DateTime.Now.Date.AddDays(_sequenceNumber - 1);
        }

        protected string GetString(MemberInfo memberInfo)
        {
            return memberInfo.Name + _sequenceNumber;
        }

        protected bool GetBoolean(MemberInfo memberInfo)
        {
            return (_sequenceNumber % 2) == 0 ? true : false;
        }

        protected Enum GetEnum(MemberInfo memberInfo)
        {
            Type enumType = GetMemberType(memberInfo);
            var enumValues = GetEnumValues(enumType);
            int newSequenceNumber = GetNewSequenceNumber(_sequenceNumber, enumValues.Length);
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

        protected static Array GetEnumValues(Type enumType)
        {
            var enumArray = EnumHelper.GetValues(enumType);
            return enumArray;
        }
    }
}