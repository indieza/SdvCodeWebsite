namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteActivities
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteActivitiesByNameInputModel
    {
        [Required]
        public string ActivityName { get; set; }
    }
}