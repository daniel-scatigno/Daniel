using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCPC.Blazor.Library
{
    public class CustomErrorEventArgs<TType>
    {
        public Dictionary<string, string> CustomErrors { get; set; }
        public TType Model { get; set; }


    }
}
