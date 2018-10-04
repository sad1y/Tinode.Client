using System;

namespace Tinode.Client
{
    [Flags]
    public enum AccessPermission
    {
        /// <summary>
        /// is not a permission per se but an indicator that permissions are explicitly cleared/not set. It usually indicates that the default permissions should not be applied
        /// </summary>
        No = 0,

        /// <summary>
        /// permission to subscribe to a topic
        /// </summary>
        Join = 1,

        /// <summary>
        /// permission to receive {data} packets 
        /// </summary>
        Read = 2,

        /// <summary>
        /// permission to {pub} to topic 
        /// </summary>
        Write = 4,

        /// <summary>
        /// permission to receive presence updates {pres}
        /// </summary>
        Presence = 8,

        /// <summary>
        ///  permission to approve requests to join a topic; a user with such permission is topic's manager
        /// </summary>
        Approve = 16,

        /// <summary>
        /// permission to invite other people to join the topic
        /// </summary>
        Sharing = 32,

        /// <summary>
        ///  permission to hard-delete messages; only owners can completely delete topics
        /// </summary>
        Delete = 64,

        /// <summary>
        /// user is the topic owner; topic may have a single owner only; some topics have no owner
        /// </summary>
        Owner = 128
    }
}