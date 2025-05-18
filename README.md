# Gruggbot

## Introduction
Gruggbot is a experimental bot for [Discord](https://discord.com/).

Just using this to play around with the Discord APIs, using [Discord.NET](https://docs.discordnet.dev/index.html) library.

## Installation (WIP)

### Token Setting

Bot token, obtained from [Discord Developer Portal](https://discord.com/developers/applications) can be added via .NET configuration. In development, this should be done in the secrets.json settings file.

```json
{
  "Bot.Token": "your-token-here"
}
```

Alternatively, the token can be added as an Environment Variable named `Bot__Token`.
