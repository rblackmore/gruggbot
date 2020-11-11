namespace Gruggbot.DomainModel
{
    public class CommandMessageImage : CommandMessage
    {
        public int ImageId { get; set; }

        public ImageDetails Image { get; set; }
    }
}
