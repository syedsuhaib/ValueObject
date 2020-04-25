using System;

namespace Value
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberAttribute : Attribute { }
}
