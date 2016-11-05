namespace SnsLite.Web.ViewModels
{
    public class UserModel
    {
        public UserModel(SnsUser user)
        {
            User = user;
        }

        public SnsUser User { get; private set; }
    }
}