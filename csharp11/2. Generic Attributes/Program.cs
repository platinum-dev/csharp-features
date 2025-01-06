using System.Reflection;

namespace _2._Generic_Attributes
{
    // before
    public class TypeAttribute : Attribute
    {
        public Type ParamType { get; }
        public TypeAttribute(Type t) => ParamType = t;
    }

    public class GenericType<T>
    {
        //[TypeAttribute(typeof(string))]
        [GenericAttribute<object>()]
        string Method() => "Test";
    }

    // c# 11
    public class GenericAttribute<T> : Attribute { }
}