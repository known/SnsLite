using Known.Core;
using SnsLite.Services;

namespace SnsLite
{
    public class SnsUser : User
    {
        public string Avatar { get; set; }
        public string Introduction { get; set; }
        public bool FriendValidate { get; set; }
        public bool AllowComment { get; set; }
        public int ShareCount { get; set; }
        public int FriendCount { get; set; }
        public int AttentionCount { get; set; }
        public int FansCount { get; set; }
        public bool IsLogin { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsCurrentFriend { get; set; }
        public bool IsCurrentValidate { get; set; }
        public bool IsCurrentAttention { get; set; }
        public bool IsMutualAttention { get; set; }
        public string ValidateMessage { get; set; }

        public void UpdateCount(ISnsUserService service)
        {
            service.UpdateSnsCount(this);
        }
    }
}
