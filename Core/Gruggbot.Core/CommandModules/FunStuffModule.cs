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

namespace Gruggbot.Core.CommandModules
{
    public class FunStuffModule : ModuleBase
    {

        private readonly ImgurClient _imgurClient;

        public FunStuffModule(ImgurClient imgurClient)
        {
            _imgurClient = imgurClient;
        }

        [Command("pug", RunMode = RunMode.Async)]
        public async Task PugAsync(int count = 1)
        {
            await Pugs(count);
        }

        private async Task Pugs(int count = 1)
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
