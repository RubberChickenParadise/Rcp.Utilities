// Copyright (c) 2019 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;

namespace Rcp.Utilities
{
    /// <summary>
    ///     The string writer with a specific character encoding.
    /// </summary>
    /// <remarks>
    ///     If you need anything other than utf-16 you need this extension.  This just has all the various constructor
    ///     overloads for StringWriter
    /// </remarks>
    public class EncodedStringWriter : StringWriter
    {
        private readonly Encoding _encoding;

        public EncodedStringWriter(Encoding encoding)
        {
            _encoding = encoding;
        }

        public EncodedStringWriter(IFormatProvider formatProvider,
                                   Encoding        encoding)
            : base(formatProvider)
        {
            _encoding = encoding;
        }

        public EncodedStringWriter(StringBuilder sb,
                                   Encoding      encoding)
            : base(sb)
        {
            _encoding = encoding;
        }

        public EncodedStringWriter(StringBuilder   sb,
                                   IFormatProvider formatProvider,
                                   Encoding        encoding)
            : base(sb,
                   formatProvider)
        {
            _encoding = encoding;
        }

        /// <summary>
        ///     Gets the character encoding.
        /// </summary>
        public override Encoding Encoding => _encoding ?? base.Encoding;
    }
}
