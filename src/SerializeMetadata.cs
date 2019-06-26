using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HolySerializer
{
    public class SerializeMetadata
    {
        public Type ObjectType { get; set; }
        public string Name { get; set; }
        public bool IsReverse { get; set; }
        public Type Converter { get; set; }
        public int Index { get; set; }
        public string Options { get; set; }
        public int Length { get; set; }
        public List<SerializeMetadata> Childs { get; set; }
    }
}
