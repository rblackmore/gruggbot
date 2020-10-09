// <copyright file="AudioService.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Services
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using Discord;
    using Discord.Audio;

    internal class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> connectedChannels =
            new ConcurrentDictionary<ulong, IAudioClient>();

        internal async Task JoinAudio(IGuild guild, IVoiceChannel channel)
        {
            // Return if already in audio channel on this guild.
            if (this.connectedChannels.TryGetValue(guild.Id, out IAudioClient client))
                return;

            // Return if channel is not a member of the provided guild
            if (channel.Guild.Id != guild.Id)
                return;

            var audioClient = await channel.ConnectAsync()
                .ConfigureAwait(false);

            if (this.connectedChannels.TryAdd(guild.Id, audioClient))
            {
                // if you add a method to log happenings from this service,
                // you can uncomment these commente dlines to make use of that.
                // await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        internal async Task LeaveChannel(IGuild guild)
        {
            // Log Event Afterthis
            if (this.connectedChannels.TryRemove(guild.Id, out IAudioClient client))
                await client.StopAsync().ConfigureAwait(false);
        }

        internal async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            // your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path))
            {
                // Comment this out later
                await channel.SendMessageAsync("File does not exist").ConfigureAwait(false);
                return;
            }

            if (this.connectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                // await Log();
                using (var ffmpeg = CreateProcess(path))
                {
                    using (var stream = client.CreatePCMStream(AudioApplication.Music))
                    {
                        try
                        {
                            await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream)
                                .ConfigureAwait(false);
                        }
                        finally
                        {
                            await stream.FlushAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        private static Process CreateProcess(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }
    }
}
