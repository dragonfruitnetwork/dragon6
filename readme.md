# Dragon6 Client
[![DragonFruit Discord](https://img.shields.io/discord/482528405292843018?label=Discord&style=popout)](https://discord.gg/VA26u5Z)

The future face of Dragon6 apps.

## Status
This is in a very early stage of development. Complete compatibility with old copies and "legacy" Dragon6 apps is not guaranteed.
The app will first be developed to have feature parity with the desktop version before splitting out to mobile and other platforms.

## Development
Dragon6 is built using Blazor Hybrid (`DragonFruit.Six.Client`) and .NET MAUI (`DragonFruit.Six.Client.Maui`) to allow features to be developed for all platforms at the same time, bringing the end to the mobile being neglected and receiving quality updates inline with other versions.

To build/test/develop a copy of Sakura make sure you have the following prerequisites:

- A desktop with [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and **ASP.NET and web development** and **.NET Multi-platform App UI development** workloads installed
- An IDE you're comfortable with. We recommend using [JetBrains Rider](https://www.jetbrains.com/rider/) or [ReSharper](https://www.jetbrains.com/resharper/) and Visual Studio to access improved code formatting and styling suggestions.

### Downloading source code
Clone the repo using git:

```
git clone https://github.com/dragonfruitnetwork/dragon6 DragonFruit.Six.Client
cd DragonFruit.Six.Client
```
Double-clicking the `.sln` or a `.csproj` file inside the folder should open your IDE with all the files listed in a sidebar. VS Code users may need to open the project as a folder instead before being able to view the structure.

## Contributing

Contributions are welcome in multiple forms. The main ways you can contribute are testing the site and reporting bugs, or providing changes through pull requests.
Feedback is welcome and helps the project advance and enables more people to get involved.

## License
Dragon6 Client is licensed under AGPL version 3 or later. Refer to [the licence file](license.md) for more information. In short, if you want to use any resource from this project, you must open source your project under the same licence and provide attribution.
This doesn't cover the use of the DragonFruit Network name or logo, as these shouldn't be used.

Any questions? Drop us a line at inbox@dragonfruit.network, create a discussion here or join our [Discord](https://discord.gg/VA26u5Z) server and ask there.