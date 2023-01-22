// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Accounts.Enums;

namespace DragonFruit.Six.Client.Overlays.Search
{
    public struct SearchArgs
    {
        private IdentifierType? _identifierType;

        public SearchArgs(string identifier, Platform platform, IdentifierType? identifierType)
        {
            Identifier = identifier;
            Platform = platform;

            _identifierType = identifierType;
        }

        /// <summary>
        /// The username or ubisoft id of the user
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// The <see cref="Platform"/> the user is playing on
        /// </summary>
        public Platform Platform { get; }

        /// <summary>
        /// The <see cref="IdentifierType"/> to search against.
        /// If a value was not provided, this will return a type based on the <see cref="Identifier"/> provided
        /// </summary>
        public IdentifierType IdentifierType => _identifierType ??= Guid.TryParse(Identifier, out _) ? IdentifierType.UserId : IdentifierType.Name;

        public override string ToString() => $"{Platform}/{Identifier}";
    }
}
