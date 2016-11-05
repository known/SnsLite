using Known;
using Known.Core;
using System.Collections.Generic;

namespace SnsLite.Services
{
    public interface ISnsUserService : IUserService
    {
        SnsUser GetSnsUser(string account);
        SnsUser GetSnsUserById(string id);
        void UpdateSnsCount(SnsUser user);
        Result UpdateAvatar(string id, string avatar);
        Result UpdateUser(SnsUser user);
        List<SnsUser> GetActiveUsers();
        List<SnsUser> GetNewestUsers();
        List<SnsUser> GetFriendUsers(string userId);
        bool CheckFriend(string userId, string friendId, bool? isValidate = null);
        Result CreateFriend(string userId, string friendId, bool isValidate, string requestMessage);
        Result AdoptFriend(string userId, string friendId, bool passed, string responseMessage);
        Result RemoveFriend(string userId, string friendId);
        bool CheckAttention(string userId, string attentionId);
        Result CreateAttention(string userId, string attentionId);
        Result RemoveAttention(string userId, string attentionId);
    }
}
