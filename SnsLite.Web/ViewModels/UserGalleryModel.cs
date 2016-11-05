using Known.Core;
using SnsLite.Services;
using System.Collections.Generic;

namespace SnsLite.Web.ViewModels
{
    public enum UserGalleryType
    {
        ActiveUser,
        NewestUser,
        FriendUser,
        RecentVisitor
    }

    public class UserGalleryModel
    {
        public UserGalleryModel(UserGalleryType type, string userId = "")
        {
            EmptyText = "暂无相关数据！";
            var userService = ServiceFactory.GetService<ISnsUserService>();
            switch (type)
            {
                case UserGalleryType.ActiveUser:
                    Users = userService.GetActiveUsers();
                    break;
                case UserGalleryType.NewestUser:
                    Users = userService.GetNewestUsers();
                    break;
                case UserGalleryType.FriendUser:
                    Users = userService.GetFriendUsers(userId);
                    break;
                case UserGalleryType.RecentVisitor:
                    break;
                default:
                    break;
            }
        }

        public string EmptyText { get; private set; }
        public List<SnsUser> Users { get; private set; }
    }
}