using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HolySerializer.Metadata
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false)]
    public class BinaryMemberAttribute : Attribute
    {
        public int Length { get; set; }
        public bool IsReverse { get; set; }
        public Type Converter { get; set; }
        public int Index { get; set; }
        public string Options { get; set; }
    }
}
