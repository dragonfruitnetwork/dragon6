// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.AspNetCore.Mvc;

namespace DragonFruit.Six.Client.WebServer
{
    [Route("~/")]
    public class HostController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View("~/Host.cshtml");
    }
}
