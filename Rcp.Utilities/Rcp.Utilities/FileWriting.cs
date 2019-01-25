// Copyright (c) 2019 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rcp.Utilities
{
    public static class FileWriting
    {
        public static async Task WriteReaderToFile(string       tempFileName,
                                                   DbDataReader reader,
                                                   string       delimiter = "|")
        {
            using (var file = new FileStream(tempFileName,
                                             FileMode.Create))
            using (var streamWriter = new StreamWriter(file))
            {
                var columns = Enumerable.Range(0,
                                               reader.FieldCount)
                                        .Select(reader.GetName)
                                        .ToList();

                await streamWriter.WriteLineAsync(string.Join(delimiter,
                                                              columns));

                while (await reader.ReadAsync())
                {
                    var woot = new object[reader.FieldCount];

                    reader.GetValues(woot);

                    await streamWriter.WriteLineAsync(string.Join(delimiter,
                                                                  woot.Select(x => x.ToString())));
                }

                await streamWriter.FlushAsync();
            }
        }
    }
}
