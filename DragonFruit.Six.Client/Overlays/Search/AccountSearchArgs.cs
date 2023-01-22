// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Text.RegularExpressions;
using DragonFruit.Six.Api.Accounts.Enums;

namespace DragonFruit.Six.Client.Overlays.Search
{
    public class AccountSearchArgs
    {
        private IdentifierType? _identifierType;

        public AccountSearchArgs(string identifier, Platform platform, IdentifierType? identifierType)
        {
            Identifier = identifier;
            Platform = platform;

            _identifierType = identifierType;
        }

        /// <summary>
        /// The username or ubisoft id of the user
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// The <see cref="Platform"/> the user is playing on
        /// </summary>
        public Platform Platform { get; private set; }

        /// <summary>
        /// The <see cref="IdentifierType"/> to search against.
        /// If a value was not provided, this will return a type based on the <see cref="Identifier"/> provided
        /// </summary>
        public IdentifierType IdentifierType => _identifierType ??= Guid.TryParse(Identifier, out _) ? IdentifierType.UserId : IdentifierType.Name;

        /// <summary>
        /// Attempts to create a <see cref="AccountSearchArgs"/> set from a dragon6 url
        /// </summary>
        /// <param name="url">The url to parse</param>
        /// <param name="args">The outputted <see cref="AccountSearchArgs"/></param>
        /// <remarks>
        /// A valid dragon6 url looks like https://dragon6.dragonfruit.network/stats/PC/14c01250-ef26-4a32-92ba-e04aa557d619?id=True
        /// </remarks>
        /// <returns><c>true</c> if the url was valid and the <see cref="args"/> were populated, otherwise <c>false</c></returns>
        public static bool TryParseFromUrl(string url, out AccountSearchArgs args)
        {
            args = null;

            // validate url is valid and host is dragon6
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.Host.Equals("dragon6.dragonfruit.network", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // there are 3 parts to a base stats url and always starts with /stats (see remarks)
            var pathMatcher = Regex.Match(uri.AbsolutePath, "/stats/(?<platform>[A-Z]{2,3})/(?<identifier>.*)", RegexOptions.IgnoreCase);

            if (!pathMatcher.Success)
            {
                return false;
            }

            if (!Enum.TryParse<Platform>(pathMatcher.Groups["platform"].Value, true, out var platform))
            {
                return false;
            }

            args = new AccountSearchArgs(pathMatcher.Groups["identifier"].Value, platform, uri.Query.Contains("id=true", StringComparison.OrdinalIgnoreCase) ? IdentifierType.UserId : null);
            return true;
        }

        public override string ToString() => $"{Platform}/{Identifier}";
    }
}
