using Discord;
using Discord.Commands;
using Gruggbot.Core.Logging;
using Gruggbot.Core.Service;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Random fun commands to play with")]
    public class FunStuffModule : ModuleBase
    {
        private readonly ILogger<FunStuffModule> _logger;
        private readonly ImgurClient _imgurClient;
        private readonly AudioService _audioService;

        public FunStuffModule(ILogger<FunStuffModule> logger, ImgurClient imgurClient)
        {
            _logger = logger;
            _imgurClient = imgurClient;
            //_audioService = audioService;
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
        public async Task PugAsync([Summary("Number of pugs to send (Max 10)")]int count = 1)
        {
            Random rando = new Random();
            int page = rando.Next(0, 100);

            count = (count > 10) ? 10 : count;
            count = (count < 1) ? 1 : count;

            var imageEndpoint = new GalleryEndpoint(_imgurClient);
            var images = await imageEndpoint.SearchGalleryAdvancedAsync("pug", page: page, sort: GallerySortOrder.Viral);

            var imageList = images.ToList();

            if (imageList.Count < 1)
                throw new ApplicationException($"ImageList Count Less than 1: {imageList.Count}");

            int next = rando.Next(0, imageList.Count - 1);

            var pugs = new List<string>();

            //Loop through for 'count' to get some pugs
            for (int idx = 0; idx < count; idx++)
            {

                bool isSuccess = false;
                int retries = 0;

                //Loop until we successfully get a pug, or we fail 20 times
                do
                {
                    retries++;
                    //If the selected index is just an image, get appropriate link to it.
                    if (imageList[next] is IGalleryImage image)
                    {
                        pugs.Add((!String.IsNullOrEmpty(image.Gifv)) ? image.Gifv : image.Link);
                        isSuccess = true;
                    }
                    //If not IGalleryImage, but is IGalleryAlbum
                    else if (imageList[next] is IGalleryAlbum album)
                    {

                        if (album.ImagesCount > 0)
                        {
                            int albumNext = (album.ImagesCount > 0) ? rando.Next(0, album.ImagesCount - 1) : 0;
                            IImage pugImage = album.Images.ElementAt(albumNext);
                            pugs.Add((!String.IsNullOrEmpty(pugImage.Gifv)) ? pugImage.Gifv : pugImage.Link);
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

            foreach (var pug in pugs)
            {
                await ReplyAsync(pug);
            }

            _logger.LogCommandCall(Context.Message.Author.Username, "Pug", count.ToString());

        }
    }
}
