using System;
using System.ComponentModel;

namespace SnsLite
{
    public enum ViewRange
    {
        [Description("所有人可见")]
        Public = 0,
        [Description("好友可见")]
        Friend = 1,
        [Description("粉丝可见")]
        Fans = 2,
        [Description("好友和粉丝可见")]
        FriendAndFans = 3,
        [Description("仅自己可见")]
        Private = 4
    }

    public class Activity
    {
        public string Id { set; get; }
        public string Content { get; set; }
        public ViewRange ViewRange { get; set; }
        public DateTime CreateTime { get; set; }
        public SnsUser User { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
    }
}
