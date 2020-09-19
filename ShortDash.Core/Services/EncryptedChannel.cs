﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace ShortDash.Core.Services
{
    public class EncryptedChannel
    {
        private readonly Aes aes;
        private readonly RSA rsa;

        public EncryptedChannel(string receiverPublicKeyXml)
        {
            aes = Aes.Create();
            rsa = RSA.Create();
            rsa.FromXmlString(receiverPublicKeyXml);
        }

        ~EncryptedChannel()
        {
            aes.Dispose();
            rsa.Dispose();
        }

        public string Decrypt(byte[] data)
        {
            using var memoryStream = new MemoryStream(data);
            byte[] iv = new byte[16];
            memoryStream.Read(iv, 0, aes.IV.Length);
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public byte[] Encrypt(string data)
        {
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream();
            memoryStream.Write(aes.IV);
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(data);
            }
            return memoryStream.ToArray();
        }

        public string ExportEncryptedKey()
        {
            return Convert.ToBase64String(rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1));
        }

        public void ImportKey(byte[] key)
        {
            aes.Key = key;
        }

        public bool Verify(byte[] data, byte[] signature)
        {
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}