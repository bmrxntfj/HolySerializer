using HolySerializer.Default;
using System;

namespace HolySerializerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello HolySerializer Example!");
            var cus = new Cus
            {
                Version = 1,
                Id = 1,
                Key = "main",
                Remark = "lastest new version 1.0",
                Cuso = new Cuso
                {
                    CusId = 2,
                    Name = "holy serializer",
                    CreatedTime = DateTime.Now,
                    Address = "china tokyo newyok",
                    UUId = Guid.NewGuid()
                }
            };
            var array = DefaultBinarySerializer.Instance.Serialize(cus);
            var cus_d = DefaultBinarySerializer.Instance.Deserialize<Cus>(array);
        }
    }
}
