// <copyright file="BaseWebsiteAction.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.WebsiteActions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public abstract class BaseWebsiteAction : BaseData
    {
        protected BaseWebsiteAction()
        {
            this.ActionStatus = WebsiteActionStatus.Unread;
        }

        [Required]
        [EnumDataType(typeof(WebsiteActionType))]
        public WebsiteActionType ActionType { get; set; }

        [Required]
        [EnumDataType(typeof(WebsiteActionStatus))]
        public WebsiteActionStatus ActionStatus { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}