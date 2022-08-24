using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBASharp {
    internal static class Numpy {
        internal static byte[] Invert(byte[] bytes) {
            var result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++) {
                result[i] = (byte)~bytes[i];
            }
            return result;
        }

        internal static short[] Invert(short[] array) {
            var result = new short[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = (short)~array[i];
            }
            return result;
        }

        internal static int[] Invert(int[] array) {
            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = ~array[i];
            }
            return result;
        }

        internal static long[] Invert(long[] array) {
            var result = new long[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = ~array[i];
            }
            return result;
        }

        internal static byte[] BitwiseAnd(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] & bytesB[i]);
            }
            return result;
        }

        internal static short[] BitwiseAnd(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] & arrayB[i]);
            }
            return result;
        }

        internal static int[] BitwiseAnd(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] & arrayB[i];
            }
            return result;
        }

        internal static long[] BitwiseAnd(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] & arrayB[i];
            }
            return result;
        }

        internal static byte[] BitwiseOr(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] | bytesB[i]);
            }
            return result;
        }

        internal static short[] BitwiseOr(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] | arrayB[i]);
            }
            return result;
        }

        internal static int[] BitwiseOr(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] | arrayB[i];
            }
            return result;
        }

        internal static long[] BitwiseOr(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] | arrayB[i];
            }
            return result;
        }

        internal static byte[] BitwiseXor(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] ^ bytesB[i]);
            }
            return result;
        }

        internal static short[] BitwiseXor(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] ^ arrayB[i]);
            }
            return result;
        }

        internal static int[] BitwiseXor(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] ^ arrayB[i];
            }
            return result;
        }

        internal static long[] BitwiseXor(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] ^ arrayB[i];
            }
            return result;
        }

        internal static byte[] Modulo(byte[] bytes, byte mod) {
            var result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++) {
                result[i] = (byte)(bytes[i] % mod);
            }
            return result;
        }

        internal static short[] Modulo(short[] array, short mod) {
            var result = new short[array.Length];
            for (int i = 0; i < array.Length; i++) {
                if (mod > 0)
                    result[i] = Math.Abs((short)(array[i] % mod));
                else if (mod < 0)
                    result[i] = (short)(array[i] % mod);
            }
            return result;
        }

        internal static int[] Modulo(int[] array, int mod) {
            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++) {
                if (mod > 0)
                    result[i] = Math.Abs(array[i] % mod);
                else if (mod < 0)
                    result[i] = array[i] % mod;
            }
            return result;
        }

        internal static long[] Modulo(long[] array, long mod) {
            var result = new long[array.Length];
            for (int i = 0; i < array.Length; i++) {
                if (mod > 0)
                    result[i] = Math.Abs(array[i] % mod);
                else if (mod < 0)
                    result[i] = array[i] % mod;
            }
            return result;
        }

        internal static byte[] ZerosByte(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            return new byte[size];
        }

        internal static short[] ZerosShort(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            return new short[size];
        }

        internal static int[] ZerosInt(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            return new int[size];
        }

        internal static long[] ZerosLong(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            return new long[size];
        }

        internal static byte[] OnesByte(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new byte[size];
            for (int i = 0; i < size; i++)
                array[i] = 1;
            return array;
        }

        internal static short[] OnesShort(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new short[size];
            for (int i = 0; i < size; i++)
                array[i] = 1;
            return array;
        }

        internal static int[] OnesInt(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = 1;
            return array;
        }

        internal static long[] OnesLong(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new long[size];
            for (int i = 0; i < size; i++)
                array[i] = 1;
            return array;
        }

        internal static short[] MinusOnesShort(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new short[size];
            for (int i = 0; i < size; i++)
                array[i] = -1;
            return array;
        }

        internal static int[] MinusOnesInt(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = -1;
            return array;
        }

        internal static long[] MinusOnesLong(int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new long[size];
            for (int i = 0; i < size; i++)
                array[i] = -1;
            return array;
        }

        internal static byte[] Add(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] + bytesB[i]);
            }
            return result;
        }

        internal static short[] Add(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] + arrayB[i]);
            }
            return result;
        }

        internal static int[] Add(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] + arrayB[i];
            }
            return result;
        }

        internal static int[] Add(int[] arrayA, byte[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] + arrayB[i];
            }
            return result;
        }

        internal static long[] Add(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] + arrayB[i];
            }
            return result;
        }

        internal static byte[] Subtract(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] - bytesB[i]);
            }
            return result;
        }

        internal static short[] Subtract(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] - arrayB[i]);
            }
            return result;
        }

        internal static int[] Subtract(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] - arrayB[i];
            }
            return result;
        }

        internal static int[] Subtract(int[] arrayA, byte[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] - arrayB[i];
            }
            return result;
        }

        internal static long[] Subtract(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] - arrayB[i];
            }
            return result;
        }

        internal static byte[] Multiply(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] * bytesB[i]);
            }
            return result;
        }

        internal static short[] Multiply(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] * arrayB[i]);
            }
            return result;
        }

        internal static int[] Multiply(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] * arrayB[i];
            }
            return result;
        }

        internal static long[] Multiply(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] * arrayB[i];
            }
            return result;
        }

        internal static byte[] Multiply(byte[] bytesA, byte value) {
            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] * value);
            }
            return result;
        }

        internal static short[] Multiply(short[] arrayA, short value) {
            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] * value);
            }
            return result;
        }

        internal static int[] Multiply(int[] arrayA, int value) {

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] * value;
            }
            return result;
        }

        internal static long[] Multiply(long[] arrayA, long value) {

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] * value;
            }
            return result;
        }

        internal static byte[] Divide(byte[] bytesA, byte[] bytesB) {
            if (bytesA.Length != bytesB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(bytesA)} && {nameof(bytesB)} are not the same length!");

            var result = new byte[bytesA.Length];
            for (int i = 0; i < bytesA.Length; i++) {
                result[i] = (byte)(bytesA[i] / bytesB[i]);
            }
            return result;
        }

        internal static byte[] Divide(byte[] arrayA, byte value) {

            var result = new byte[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (byte)(arrayA[i] / value);
            }
            return result;
        }

        internal static short[] Divide(short[] arrayA, short[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] / arrayB[i]);
            }
            return result;
        }

        internal static short[] Divide(short[] arrayA, short value) {

            var result = new short[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = (short)(arrayA[i] / value);
            }
            return result;
        }

        internal static int[] Divide(int[] arrayA, int[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] / arrayB[i];
            }
            return result;
        }

        internal static int[] Divide(int[] arrayA, int value) {

            var result = new int[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] / value;
            }
            return result;
        }

        internal static long[] Divide(long[] arrayA, long[] arrayB) {
            if (arrayA.Length != arrayB.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayA)} && {nameof(arrayB)} are not the same length!");

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] / arrayB[i];
            }
            return result;
        }

        internal static long[] Divide(long[] arrayA, long value) {

            var result = new long[arrayA.Length];
            for (int i = 0; i < arrayA.Length; i++) {
                result[i] = arrayA[i] / value;
            }
            return result;
        }

        internal static byte[] Fill(byte value, int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new byte[size];
            for (int i = 0; i < size; i++)
                array[i] = value;
            return array;
        }

        internal static byte[] Fill(byte value, int valueCount, int arraySize) {
            if (arraySize == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(arraySize));

            var array = new byte[arraySize];
            for (int i = 0; i < valueCount; i++)
                array[i] = value;
            return array;
        }

        internal static short[] Fill(short value, int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new short[size];
            for (int i = 0; i < size; i++)
                array[i] = value;
            return array;
        }

        internal static short[] Fill(short value, int valueCount, int arraySize) {
            if (arraySize == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(arraySize));

            var array = new short[arraySize];
            for (int i = 0; i < valueCount; i++)
                array[i] = value;
            return array;
        }

        internal static int[] Fill(int value, int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = value;
            return array;
        }

        internal static int[] Fill(int value, int valueCount, int arraySize) {
            if (arraySize == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(arraySize));

            var array = new int[arraySize];
            for (int i = 0; i < valueCount; i++)
                array[i] = value;
            return array;
        }

        internal static long[] Fill(long value, int size) {
            if (size == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size));

            var array = new long[size];
            for (int i = 0; i < size; i++)
                array[i] = value;
            return array;
        }

        internal static long[] Fill(long value, int valueCount, int arraySize) {
            if (arraySize == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(arraySize));

            var array = new long[arraySize];
            for (int i = 0; i < valueCount; i++)
                array[i] = value;
            return array;
        }
    }
}
