using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace Rcp.Utilities
{
    public class StringChunks
    {
        public IEnumerable<string> ChunkString(string input,
                                               int    chunkLength)
        {
            var retVal = new List<string>((int)Math.Ceiling((decimal)input.Length / (decimal)chunkLength));

            for (int i = 0; i < Math.Ceiling((decimal)input.Length / (decimal)chunkLength); i++)
            {
                var multiplier = i;
                var start = multiplier * chunkLength;
                var length = input.Length - start > chunkLength ? chunkLength : input.Length - start;

                retVal.Add(input.Substring(start, length));
            }

            return retVal;
        }
    }
}
