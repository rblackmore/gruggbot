using Discord.Commands;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using System.Diagnostics;
using Gruggbot.Core.Service;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Random fun commands to play with")]
    public class FunStuffModule : ModuleBase
    {
        private readonly ImgurClient _imgurClient;
        private readonly AudioService _audioService;

        public FunStuffModule(ImgurClient imgurClient, AudioService audioService)
        {
            _imgurClient = imgurClient;
            _audioService = audioService;
        }

        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("8ball", RunMode = RunMode.Async), Summary("Ask a question, get an answer")]
        [RequireOwner]
        [Hidden]
        public async Task EightBall([Remainder(), Summary("the Question to answer")]string question)
        {

        }

        [Command("noot")]
        [Hidden]
        public async Task Noot(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;

            if (channel == null)
                await ReplyAsync("User must be in a voice channel"); //Remove this and do nothing later

            //await _audioService.JoinAudio(Context.Guild, (Conte))
        }

        [Command("hjälp", RunMode = RunMode.Async), Summary("no hablar español")]
        [Hidden]
        public async Task HjalpCmd()
        {
            await ReplyAsync("No Hablar Español");
        }

        [Command("pug", RunMode = RunMode.Async)]
        public async Task PugAsync(int count = 1)
        {
            Random rando = new Random();
            int page = rando.Next(0, 100);

            count = (count > 10) ? 10 : count;
            count = (count < 1) ? 1 : count;

            var imageEndpoint = new GalleryEndpoint(_imgurClient);
            var images = await imageEndpoint.SearchGalleryAdvancedAsync("pug", page: page);

            var imageList = images.ToList();

            if (imageList.Count < 1)
                throw new ApplicationException($"ImageList Count Less than 1: {imageList.Count}");

            int next = rando.Next(0, imageList.Count - 1);

            for (int idx = 0; idx < count; idx++)
            {
                bool isSuccess = false;
                int retries = 0;
                do
                {
                    retries++;
                    if (imageList[next] is IGalleryImage image)
                    {
                        await ReplyAsync(image.Link);
                        isSuccess = true;
                    }
                    else if (imageList[next] is IGalleryAlbum album)
                    {
                        if (album.ImagesCount > 0)
                        {
                            int albumNext = (album.ImagesCount > 0) ? rando.Next(0, album.ImagesCount - 1) : 0;
                            await ReplyAsync(album.Images.ToList()[albumNext].Link);
                            isSuccess = true;
                        }
                        else
                        {
                            next = rando.Next(0, imageList.Count - 1);
                        }
                    }

                } while (!isSuccess || retries > 20);


                imageList.RemoveAt(next);
                next = rando.Next(0, imageList.Count - 1);

            }

        }
    }
}
