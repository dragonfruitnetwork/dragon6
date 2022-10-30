// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using System.Threading.Tasks;
using DragonFruit.Data.Serializers.Newtonsoft;
using DragonFruit.Six.Api.Authentication.Entities;
using DragonFruit.Six.Api.Services.Developer;
using DragonFruit.Six.Client.Database;

namespace DragonFruit.Six.Client.Network
{
    public class Dragon6DebugClient : Dragon6DeveloperClient
    {
        private const string TokenFileName = "ubi.token";

        private readonly IDragon6Platform _fileSystem;

        public Dragon6DebugClient(IDragon6Platform fileSystem)
            : base("00000000-0000-0000-0000-000000000001.045a00e5c0", "EzsMSGkhL9Z5EDUYzuMnBsf1GyL/ygAR0DjdHl60coQ", "dragon6.token.read")
        {
            _fileSystem = fileSystem;
        }

        protected override async Task<IUbisoftToken> GetToken(string sessionId)
        {
            var tokenFile = Path.Combine(_fileSystem.Cache, TokenFileName);
            IUbisoftToken token = FileServices.ReadFileOrDefault<Dragon6Token>(tokenFile);

            if (token != null && token.SessionId != sessionId)
            {
                return token;
            }

            token = await base.GetToken(sessionId).ConfigureAwait(false);
            FileServices.WriteFile(tokenFile, token);

            return token;
        }
    }
}
