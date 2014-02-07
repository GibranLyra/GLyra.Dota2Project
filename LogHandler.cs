using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota.CentralDota
{
    public static class LogHandler
    {
        /// <summary>
        /// Create a log in case of a item that's supposed to have only one value has more than one
        /// </summary>
        /// <param name="nodeName"></param>
        public static void createWarningLog(string nodeName, int expectedSize, int actualSize)
        {
            Debug.WriteLine("Warning, node " + nodeName + " supposed to have: " + expectedSize  + " item(s), but have " + actualSize);
        }
    }
}
