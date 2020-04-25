using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Value
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo> properties = null!;
        private List<FieldInfo> fields = null!;

        public static bool operator ==(ValueObject firstObject, ValueObject secondObject) =>
            Equals(firstObject, null) ? Equals(secondObject, null) : firstObject.Equals(secondObject);

        public static bool operator !=(ValueObject firstObject, ValueObject secondObject) =>
            !(firstObject == secondObject);

        public bool Equals(ValueObject valueObject) => Equals(valueObject as object);

        public override bool Equals(object obj) =>
            obj != null && GetType() == obj.GetType()
                && GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));

        private bool PropertiesAreEqual(object obj, PropertyInfo p) =>
            Equals(p.GetValue(this, null), p.GetValue(obj, null));

        private bool FieldsAreEqual(object obj, FieldInfo f) =>
            Equals(f.GetValue(this), f.GetValue(obj));

        //using GetCustomAttribute() instead of Attribute.IsDefined
        //source: https://github.com/ardalis/CleanArchitecture
        private IEnumerable<PropertyInfo> GetProperties() =>
            properties ??= GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                    .ToList();
        private IEnumerable<FieldInfo> GetFields() =>
            fields ??= (GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList());

        public override int GetHashCode()
        {
            unchecked   //allow overflow
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {
                    var value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach (var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        private int HashValue(int seed, object value) =>
            (seed * 23) + (value == null ? 0 : value.GetHashCode());
    }
}