namespace Gruggbot.DomainModel
{
    public class CommandMessage
    {
        public int ID { get; set; }

        public int CommandId { get; set; }

        public int Sequence { get; set; }

        public string Text { get; set; }
    }
}
