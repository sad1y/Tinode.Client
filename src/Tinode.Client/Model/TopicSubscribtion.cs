using Pbx;

namespace Tinode.Client
{
    public class TopicSubscribtion
    {
        private readonly TopicSub _topicSub;

        private TopicSubscribtion(TopicSub topicSub)
        {
            _topicSub = topicSub;
            Public = VCard.FromByteString(topicSub.Public);
            Private = PrivateData.FromByteString(topicSub.Private);
        }

        public VCard Public { get; }
        public PrivateData Private { get; }
        public long UpdatedAt => _topicSub.UpdatedAt;
        public long DeletedAt => _topicSub.DeletedAt;
        public long TouchedAt => _topicSub.TouchedAt;
        public long LastSeenTime => _topicSub.LastSeenTime;
        public string LastSeenUserAgent => _topicSub.LastSeenUserAgent;

        public int DelId => _topicSub.DelId;
        public int ReadId => _topicSub.ReadId;
        public int RecvId => _topicSub.RecvId;
        public int SeqId => _topicSub.SeqId;
        public string Topic => _topicSub.Topic;
        public string UserId => _topicSub.UserId;

        public AccessMode Acs => _topicSub.Acs;
        public bool Online => _topicSub.Online;

        public static TopicSubscribtion FromTopicSub(TopicSub topicSub) => new TopicSubscribtion(topicSub);
    }
}