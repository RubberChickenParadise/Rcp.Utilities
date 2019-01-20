// Copyright (c) 2019 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Rcp.Utilities
{
    public static class Utility
    {
        /// <summary>
        ///     Deserialize a byte array to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FromByteArray<T>(this byte[] byteArray)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException(nameof(byteArray));
            }

            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream(byteArray))
            {
                return (T) binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static T FromXml<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(xml))
            {
                var obj = (T) serializer.Deserialize(reader);

                return obj;
            }
        }

        /// <summary>
        ///     Serialize an object to a byte array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream,
                                          obj);
                return memoryStream.ToArray();
            }
        }

        public static string ToXml(this object obj)
        {
            var xsSubmit = new XmlSerializer(obj.GetType());

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer,
                                       obj);

                    return sww.ToString(); // Your XML
                }
            }
        }
    }
}
