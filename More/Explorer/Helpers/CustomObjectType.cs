using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    [TypeConverter(typeof(CustomObjectConverter))]
    public class CustomObjectType
    {
        private List<CustomProperty> propertyList = new List<CustomProperty>();

        [Browsable(false)]
        public List<CustomProperty> Properties
        {
            get
            {
                return propertyList;
            } 
            set
            {
                propertyList = value;
            }
        }

        private readonly Dictionary<string, string> valueDictionary = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                string val; 
                valueDictionary.TryGetValue(name, out val); 
                return val;
            }
            set
            {
                valueDictionary[name] = value;
            }
        }

        private class CustomObjectConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                var standardProperties = base.GetProperties(context, value, attributes);
                var obj = value as CustomObjectType;
                var customProps = obj == null ? null : obj.Properties;
                var props = new PropertyDescriptor[standardProperties.Count + (customProps == null ? 0 : customProps.Count)];
                standardProperties.CopyTo(props, 0);
                if (customProps != null)
                {
                    int index = standardProperties.Count;
                    foreach (var property in customProps)
                    {
                        props[index++] = new CustomPropertyDescriptor(property);
                    }
                }
                return new PropertyDescriptorCollection(props);
            }
        }
        private class CustomPropertyDescriptor : PropertyDescriptor
        {
            private readonly CustomProperty property;
            public CustomPropertyDescriptor(CustomProperty property)
                : base(property.Name, null)
            {
                this.property = property;
            }
            public override string Category { get { return "Parameters"; } }
            public override string Description { get { return property.Description; } }
            public override string Name { get { return property.Name; } }
            public override bool ShouldSerializeValue(object component) { return ((CustomObjectType)component)[property.Name] != null; }
            public override void ResetValue(object component) { ((CustomObjectType)component)[property.Name] = null; }
            public override bool IsReadOnly { get { return false; } }
            public override Type PropertyType { get { return property.Type; } }
            public override bool CanResetValue(object component) { return true; }
            public override Type ComponentType { get { return typeof(CustomObjectType); } }
            public override void SetValue(object component, object value) { ((CustomObjectType)component)[property.Name] = value as string; }
            public override object GetValue(object component) { return ((CustomObjectType)component)[property.Name] ?? property.DefaultValue; }
        }
    }


    public class CustomProperty
    {
        #region Private Fields
        private Type type;
        #endregion

        #region Public Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                if (type != typeof(string))
                {
                    DefaultValue = Activator.CreateInstance(value);
                }
            }
        }
        #endregion
    }
}
