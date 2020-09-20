﻿namespace ShortDash.Core.Interfaces
{
    public interface IEncryptedChannelService
    {
        void CloseChannel(string channelId);

        string Encrypt(string channelId, string data);

        string Encrypt(string channelId, object data);

        string ExportEncryptedKey(string channelId);

        string ExportPublicKey();

        public string GenerateChallenge(string publicKey, out byte[] rawChallenge);

        public string GenerateChallengeResponse(string challenge, string publicKey);

        void ImportPrivateKey(string privateKeyXml);

        void OpenChannel(string channelId, string receiverPublicKeyXml);

        void OpenChannel(string channelId, string receiverPublicKeyXml, string encryptedKey);

        bool TryDecrypt(string channelId, string encryptedPacket, out string data);

        bool TryDecrypt<TDataType>(string channelId, string encryptedPacket, out TDataType data);

        public bool VerifyChallengeResponse(byte[] challenge, string challengeResponse);
    }

    public interface IEncryptedChannelService<T> : IEncryptedChannelService
    {
    }
}