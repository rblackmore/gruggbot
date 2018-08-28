using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.Service
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel channel)
        {
            // Return if already in audio channel on this guild.
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
                return;

            // Return if channel is not a member of the provided guild
            if (channel.Guild.Id != guild.Id)
                return;

            var audioClient = await channel.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // if you add a method to log happenings from this service,
                // you can uncomment these commente dlines to make use of that.
                // await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveChannel(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
                await client.StopAsync(); //Log Event Afterthis

        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            // your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist"); //Comment this out later
                return;
            }

            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                //await Log();
                using (var ffmpeg = CreateProcess(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }

            }
        }

        private Process CreateProcess(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            return Process.Start(ffmpeg);
        }
    }
}
