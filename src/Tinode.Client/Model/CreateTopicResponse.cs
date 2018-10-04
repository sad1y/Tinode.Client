namespace Tinode.Client
{
    public class CreateTopicResponse
    {
        public string Id { get; }

        public CreateTopicResponse(string id)
        {
            Id = id;
        }
    }
}