// Copyright (c) 2019 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rcp.Utilities
{
    public static class SHA1
    {
        public static string Sha1Base64Hash(string input)
        {
            var hash = Sha1Bytes(input);
            return Convert.ToBase64String(hash);
        }

        public static byte[] Sha1Bytes(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        public static string Sha1Hash(string input)
        {
            var hash = Sha1Bytes(input);
            return string.Join("",
                               hash.Select(b => b.ToString("x2"))
                                   .ToArray());
        }
    }
}
