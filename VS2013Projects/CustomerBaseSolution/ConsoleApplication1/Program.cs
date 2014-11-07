using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            (new TestContext()).AEntitys.ToList();
        }
    }

    public class AEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TestContext : DbContext
    {
        public DbSet<AEntity> AEntitys { get; set; }

        public TestContext()
            : base("CBDevDb")
        {

        }
    }


}
