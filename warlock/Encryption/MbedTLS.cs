﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Shadowsocks.Encryption
{
    internal class MbedTLS
    {
        const string DLLNAME = "libsscrypto";

        public const int MBEDTLS_ENCRYPT = 1;
        public const int MBEDTLS_DECRYPT = 0;

        public static bool Available { get; }
        static MbedTLS()
        {
            var dllPath = Utils.TouchEncryptorDll();
            if (IntPtr.Size == 4)
            {
                Available = true;
                LoadLibrary(dllPath);
            }
        }

        public static byte[] MD5(byte[] input)
        {
            //byte[] output = new byte[16];
            //md5(input, (uint)input.Length, output);
            //return output;
            //i believe microsoft
            using (var md5 = System.Security.Cryptography.MD5.Create())
                return md5.ComputeHash(input);
        }

        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr cipher_info_from_string(string cipher_name);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cipher_init(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_setup(IntPtr ctx, IntPtr cipher_info);

        // XXX: Check operation before using it
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_setkey(IntPtr ctx, byte[] key, int key_bitlen, int operation);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_set_iv(IntPtr ctx, byte[] iv, int iv_len);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_reset(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_update(IntPtr ctx, byte[] input, int ilen, byte[] output, ref int olen);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cipher_free(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void md5(byte[] input, uint ilen, byte[] output);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_get_size_ex();
    }
}
