namespace SnsLite.Web
{
    public enum AjaxRequestType
    {
        //Relation
        Relation_AddFriend,
        Relation_ValidateFriend,
        Relation_RemoveFriend,
        Relation_AddAttention,
        Relation_RemoveAttention,
        //Activity
        Activity_AddActivity,
        Activity_RemoveActivity,
        Activity_AddLike,
        Activity_RemoveLike,
        //Profile
        Profile_SaveBasic,
        Profile_ChangePassword,
    }
}