namespace SdvCode.Models
{
    public class FollowUnfollow
    {
        public string PersonId { get; set; }

        public string FollowerId { get; set; }

        public bool IsFollowed { get; set; }
    }
}