using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC.Code
{
    public class Supplier
    {
        public string Name { get; set; }

        public FileHeader Header { get; set; }

        public Supplier(string name)
        {
            this.Name = name;
            this.Header = FileHeader.GetHeader(name);
        }
    }
}
