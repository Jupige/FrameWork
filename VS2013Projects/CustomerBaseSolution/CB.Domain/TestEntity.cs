using System;
using BaseFramework.Domain;

namespace CustomerBaseSolution
{
    public class TestEntity : AuditableEntity
    {
        public string Name { get; set; }

        public DateTime TestDate { get; set; }


        public TestEntity()
        {

        }

        public void ThrowNewExcetion()
        {
            throw new DomainException("抛出的第一个错误【{0}】", "哈哈");
        }
    }
}
