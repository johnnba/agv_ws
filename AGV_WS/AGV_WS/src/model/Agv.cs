using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class Agv
    {
        public UInt64 Id { get; set; }
        public Agv(UInt64 id)
        {
            Id = id;
        }
    }
}
