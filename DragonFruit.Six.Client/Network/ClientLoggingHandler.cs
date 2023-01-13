﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DragonFruit.Six.Client.Network
{
    public class ClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public ClientLoggingHandler(ILogger logger, HttpMessageHandler handler)
        {
            _logger = logger;
            InnerHandler = handler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting HTTP request to {url}", request.RequestUri.Host);
            var result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if ((int)result.StatusCode is >= 400 and <= 599)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogWarning("Request to {url} returned {status}: {message}", result.RequestMessage.RequestUri, result.StatusCode, content);
            }
            else
            {
                _logger.LogInformation("Request to {url} returned {status}", result.RequestMessage.RequestUri.Host, result.StatusCode);
            }

            return result;
        }
    }
}
