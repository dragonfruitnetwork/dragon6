// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DragonFruit.Data.Serializers.Newtonsoft;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Authentication.Entities;
using DragonFruit.Six.Api.Services.Developer;
using DragonFruit.Six.Client.Database;
using Microsoft.Extensions.Logging;

namespace DragonFruit.Six.Client.Network
{
    public class Dragon6DebugClient : Dragon6DeveloperClient
    {
        private const string TokenFileName = "ubi.token";

        private readonly IDragon6Platform _fileSystem;
        private readonly ILogger<Dragon6Client> _logger;

        public Dragon6DebugClient(IDragon6Platform fileSystem, ILogger<Dragon6Client> logger)
            : base("00000000-0000-0000-0000-000000000001.045a00e5c0", "EzsMSGkhL9Z5EDUYzuMnBsf1GyL/ygAR0DjdHl60coQ", "dragon6.token.read")
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        protected override async Task<IUbisoftToken> GetToken(string sessionId)
        {
            var tokenFile = Path.Combine(_fileSystem.Cache, TokenFileName);
            IUbisoftToken token = FileServices.ReadFileOrDefault<Dragon6Token>(tokenFile);

            if (token != null && token.SessionId != sessionId)
            {
                _logger.LogDebug("Token loaded: {session} (Expires {exp}", token.SessionId, token.Expiry);
                return token;
            }

            token = await base.GetToken(sessionId).ConfigureAwait(false);
            FileServices.WriteFile(tokenFile, token);

            _logger.LogDebug("New token fetched: {session} (Expires {exp})", token.SessionId, token.Expiry);

            return token;
        }

        protected sealed override HttpMessageHandler CreateHandler()
        {
            var handler = base.CreateHandler() ?? new SocketsHttpHandler { AutomaticDecompression = DecompressionMethods.All };
            return new ClientLoggingHandler(_logger, handler);
        }
    }
}
