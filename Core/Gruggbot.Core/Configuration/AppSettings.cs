using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Configuration
{
    /// <summary>
    /// Used to Generate appsettings json file with all required configuration settings.
    /// Settings will be empty.
    /// </summary>
    public class AppSettings
    {
        public string Token { get; set; }
        public string ImgurClientId { get; set; }
    }
}
