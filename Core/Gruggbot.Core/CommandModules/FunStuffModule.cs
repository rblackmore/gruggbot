using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Gruggbot.Core.Logging;

using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;

using Microsoft.Extensions.Logging;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Random fun commands to play with")]
    public class FunStuffModule : ModuleBase
    {
        private readonly ILogger<FunStuffModule> _logger;
        //private readonly ApiClient _imgurClient;
        //private readonly AudioService _audioService;

        public FunStuffModule(ILogger<FunStuffModule> logger)
        {
            _logger = logger;
            //_imgurClient = imgurClient;
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
            await Task.CompletedTask;
        }

        [Command("mario"), Summary("It's-a-me Mario!")]
        public async Task Mario()
        {

            StringBuilder top = new StringBuilder();
            StringBuilder bot = new StringBuilder();


            top.AppendLine(":black_circle: :black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle:");
            top.AppendLine(":black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle:");
            top.AppendLine(":black_circle: :black_circle: :chestnut: :chestnut: :chestnut: :chestnut: :cookie: :chestnut: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :cookie: :chestnut: :cookie: :cookie: :cookie: :chestnut: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :cookie: :chestnut: :chestnut: :cookie: :cookie: :cookie: :chestnut: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :chestnut: :cookie: :cookie: :cookie: :cookie: :chestnut: :chestnut: :chestnut: :chestnut:");
            top.AppendLine(":black_circle: :black_circle: :black_circle: :cookie: :cookie: :cookie: :cookie: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :black_circle: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :chestnut:");

            bot.AppendLine(":black_circle: :chestnut: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":chestnut: :chestnut: :chestnut: :chestnut: :red_circle: :red_circle: :red_circle: :red_circle: :chestnut: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":cookie: :cookie: :chestnut: :red_circle: :cookie: :red_circle: :red_circle: :cookie: :red_circle: :chestnut: :cookie: :cookie:");
            bot.AppendLine(":cookie: :cookie: :cookie: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :cookie: :cookie: :cookie:");
            bot.AppendLine(":cookie: :cookie: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :cookie: :cookie:");
            bot.AppendLine(":black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :black_circle: :black_circle: :red_circle: :red_circle: :red_circle:");
            bot.AppendLine(":black_circle: :chestnut: :chestnut: :chestnut: :black_circle: :black_circle: :black_circle: :black_circle: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":chestnut: :chestnut: :chestnut: :chestnut: :black_circle: :black_circle: :black_circle: :black_circle: :chestnut: :chestnut: :chestnut: :chestnut:");


            await ReplyAsync(top.ToString());
            await ReplyAsync(bot.ToString());
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

        //[Command("pug", RunMode = RunMode.Async)]
        //public async Task PugAsync([Summary("Number of pugs to send (Max 10)")]int count = 1)
        //{
        //    Random rando = new Random();
        //    int page = rando.Next(0, 100);

        //    count = (count > 10) ? 10 : count;
        //    count = (count < 1) ? 1 : count;

        //    var imageEndpoint = new Imgur.API.Endpoints.ImageEndpoint(_imgurClient, this._httpClient);
        //    imageEndpoint.
        //    var images = await imageEndpoint.SearchGalleryAdvancedAsync("pug", page: page, sort: GallerySortOrder.Viral);

        //    var imageList = images.ToList();

        //    if (imageList.Count < 1)
        //        throw new ApplicationException($"ImageList Count Less than 1: {imageList.Count}");

        //    int next = rando.Next(0, imageList.Count - 1);

        //    var pugs = new List<string>();

        //    //Loop through for 'count' to get some pugs
        //    for (int idx = 0; idx < count; idx++)
        //    {

        //        bool isSuccess = false;
        //        int retries = 0;

        //        //Loop until we successfully get a pug, or we fail 20 times
        //        do
        //        {
        //            retries++;
        //            //If the selected index is just an image, get appropriate link to it.
        //            if (imageList[next] is IGalleryImage image)
        //            {
        //                pugs.Add((!String.IsNullOrEmpty(image.Gifv)) ? image.Gifv : image.Link);
        //                isSuccess = true;
        //            }
        //            //If not IGalleryImage, but is IGalleryAlbum
        //            else if (imageList[next] is IGalleryAlbum album)
        //            {

        //                if (album.ImagesCount > 0)
        //                {
        //                    int albumNext = (album.ImagesCount > 0) ? rando.Next(0, album.ImagesCount - 1) : 0;
        //                    IImage pugImage = album.Images.ElementAt(albumNext);
        //                    pugs.Add((!String.IsNullOrEmpty(pugImage.Gifv)) ? pugImage.Gifv : pugImage.Link);
        //                    isSuccess = true;
        //                }
        //                else
        //                {
        //                    next = rando.Next(0, imageList.Count - 1);
        //                }
        //            }

        //        } while (!isSuccess || retries > 20);


        //        imageList.RemoveAt(next);
        //        next = rando.Next(0, imageList.Count - 1);

        //    }

        //    foreach (var pug in pugs)
        //    {
        //        await ReplyAsync(pug);
        //    }

        //    _logger.LogCommandCall(Context.Message.Author.Username, "Pug", count.ToString());

        //}
    }
}
