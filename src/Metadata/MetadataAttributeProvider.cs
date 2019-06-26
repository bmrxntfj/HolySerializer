using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HolySerializer.Metadata
{
    public enum MetadataOrderType
    {
        Index,
        Name,
        Declare
    }

    public class MetadataAttributeProvider : AbstractMetadataProvider
    {
        /// <summary>
        /// true that asc order,false that desc order.
        /// </summary>
        public bool Asc { get; set; }
        /// <summary>
        /// Index,Name,Declare
        /// </summary>
        public MetadataOrderType OrderType { get; set; }
        /// <summary>
        /// true that must be need attribute,false that non-attribute.
        /// </summary>
        public bool Strict { get; set; }

        private static ConcurrentDictionary<Type, SerializeMetadata> MetadataCache = new ConcurrentDictionary<Type, SerializeMetadata>();
        public override SerializeMetadata GetMetadataFrom(object value)
        {
            var cnx = new SerializeMetadata();
            var isDeserialize = value is Type;
            if (isDeserialize)
            {
                cnx.ObjectType = (Type)value;
            }
            else
            {
                cnx.ObjectType = (value != null ? value.GetType() : null);
            }

            if (cnx.ObjectType != null && base.IsRecursive(cnx.ObjectType))
            {
                if (!MetadataCache.ContainsKey(cnx.ObjectType))
                {
                    cnx = GetSerializeMetadata(cnx.ObjectType);
                    MetadataCache.TryAdd(cnx.ObjectType, cnx);
                }
                else
                {
                    cnx = MetadataCache[cnx.ObjectType];
                }
            }
            return cnx;
        }

        SerializeMetadata ConvertFrom(Type type, BinaryMemberAttribute contract)
        {
            var parent = new SerializeMetadata();
            parent.ObjectType = type;
            parent.Converter = contract.Converter;
            parent.Options = contract.Options;
            parent.Index = contract.Index;
            parent.IsReverse = contract.IsReverse;
            if (contract.Length > 0) { parent.Length = contract.Length; } else { parent.Length = base.SizeOf(parent.ObjectType); }
            return parent;
        }

        void Cover(SerializeMetadata one, SerializeMetadata newOne)
        {
            one.Index = newOne.Index;
            one.IsReverse = newOne.IsReverse;
            one.Converter = newOne.Converter;
            one.Options = newOne.Options;
            one.Childs = newOne.Childs;
            if (one.Length == 0) { one.Length = newOne.Length; }
        }

        SerializeMetadata GetOneSerializeMetadata(object p)
        {
            BinaryMemberAttribute attr = null;
            Type type = null;
            if (p is Type)
            {
                type = (Type)p;
                attr = type.GetCustomAttribute<BinaryMemberAttribute>(true);
            }
            else if (p is PropertyInfo)
            {
                attr = ((PropertyInfo)p).GetCustomAttribute<BinaryMemberAttribute>();
                type = ((PropertyInfo)p).PropertyType;
            }
            SerializeMetadata metadata = null;
            if (attr == null)
            {
                if (this.Strict) { return metadata; } else { metadata = new SerializeMetadata { ObjectType = type }; }
            }
            else
            {
                metadata = ConvertFrom(type, attr);
            }
            return metadata;
        }

        SerializeMetadata GetSerializeMetadata(Type type)
        {
            SerializeMetadata parent = GetOneSerializeMetadata(type);
            if (parent == null) { return null; }

            var pros = base.GetAvailableProperty(type);

            pros.ForEach(p =>
            {
                SerializeMetadata child = GetOneSerializeMetadata(p);
                if (child == null) { return; }

                if (IsRecursive(child.ObjectType))
                {
                    var newchild = GetSerializeMetadata(p.PropertyType);
                    if (child != null && newchild != null)
                    {
                        Cover(child, newchild);
                    }
                }

                child.Name = p.Name;
                if (parent.Childs == null) { parent.Childs = new List<SerializeMetadata>(); }
                parent.Childs.Add(child);
            });
            parent.Childs = this.Sort(parent.Childs);
            //if (parent.Length == 0 && parent.Childs?.Count > 0 && !parent.Childs.Any(c => c.Length == 0)) { parent.Length = parent.Childs.Sum(c => c.Length); }
            return parent;
        }

        List<SerializeMetadata> Sort(List<SerializeMetadata> metadata)
        {
            if (metadata == null) { return null; }
            switch (this.OrderType)
            {
                case MetadataOrderType.Index:
                    {
                        return this.Asc ? metadata.OrderBy(c => c.Index).ToList() : metadata.OrderByDescending(c => c.Index).ToList();
                    }
                case MetadataOrderType.Name:
                    {
                        return this.Asc ? metadata.OrderBy(c => c.Name).ToList() : metadata.OrderByDescending(c => c.Name).ToList();
                    }
                case MetadataOrderType.Declare:
                    {
                        if (!this.Asc) { metadata.Reverse(); }
                        return metadata;
                    }
            }
            return null;
        }
    }
}
