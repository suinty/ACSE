﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace System
{
    public static class DataExtensions
    {
        /// <summary>
        /// Returns a tuple containing the two nibbles of the byte.
        /// </summary>
        /// <param name="b">The input byte</param>
        /// <returns>The byte's nibbles</returns>
        public static (byte, byte) GetNibbles(this byte b)
        {
            return ((byte)(b & 0x0F), (byte)((b >> 4) & 0x0F));
        }

        /// <summary>
        /// Returns the bit from a specified index of a byte.
        /// </summary>
        /// <param name="b">The input byte</param>
        /// <param name="index">The bit to retrieve</param>
        /// <param name="reverse">Sets if the index should be reversed</param>
        /// <returns>The requested bit</returns>
        public static byte GetBit(this byte b, int index, bool reverse = false)
        {
            if (index < 0 || index > 7) throw new ArgumentException("Index is expected to be in the range of 0 - 7!");
            return (byte)((reverse ? b >> (7 - index) : b >> index) & 1);
        }

        /// <summary>
        /// Returns a list of bits for any ValueType object supplied that isn't a struct.
        /// </summary>
        /// <param name="input">The input object</param>
        /// <returns><see cref="Collections.ICollection"/> of bits</returns>
        public static ICollection<byte> GetBits(this ValueType input)
        {
            if (!input.GetType().IsPrimitive) throw new ArgumentException("Object cannot be a struct!");

            var bits = Marshal.SizeOf(input) * 8;
            var output = new byte[bits];
            dynamic obj = input;
            for (var i = 0; i < bits; i++)
            {
                output[i] = (byte)((obj >> i) & 1);
            }

            return output;
        }

        /// <summary>
        /// Sets a specified bit of a byte.
        /// </summary>
        /// <param name="b">The byte to modify</param>
        /// <param name="index">The index of the bit to set</param>
        /// <param name="set">Should the bit be set or not</param>
        /// <param name="reverse">Is the index reversed (e.g. 0 = topmost bit)</param>
        public static void SetBit(ref this byte b, int index, bool set, bool reverse = false)
        {
            if (index < 0 || index > 7) throw new ArgumentException("Index is expected to be in the range of 0 - 7!");
            var bitmask = 1 << (reverse ? 7 - index : index);
            if (set)
            {
                b |= (byte) bitmask;
            }
            else
            {
                b &= (byte) ~bitmask;
            }
        }

        /// <summary>
        /// Creates a byte from an <see cref="IEnumerable{T}"/>. If less than 8 entries are supplied, 0 will be used for the remaining bits.
        /// </summary>
        /// <param name="byteArray">The <see cref="IEnumerable{T}"/> to create the byte from</param>
        /// <returns>The created byte</returns>
        public static byte GetByte(this IEnumerable<byte> byteArray)
        {
            var bitArray = byteArray as byte[] ?? byteArray.ToArray();
            byte val = 0;
            for (var i = 0; i < (bitArray.Length >= 8 ? 8 : bitArray.Length); i++)
            {
                val |= (byte)((bitArray[i] & 1) << i);
            }

            return val;
        }
    }
}
