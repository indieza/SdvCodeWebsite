// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;

    public class FavouritePostProfile : Profile
    {
        public FavouritePostProfile()
        {
            this.CreateMap<FavouritePost, FavouritePostViewModel>()
                .ForMember(
                    dm => dm.Id,
                    mo => mo.MapFrom(x => x.Post.Id))
                .ForMember(
                    dm => dm.Title,
                    mo => mo.MapFrom(x => x.Post.Title))
                .ForMember(
                    dm => dm.ShortContent,
                    mo => mo.MapFrom(x => x.Post.ShortContent))
                .ForMember(
                    dm => dm.CreatedOn,
                    mo => mo.MapFrom(x => x.Post.CreatedOn))
                .ForMember(
                    dm => dm.Category,
                    mo => mo.MapFrom(x => x.Post.Category));
        }
    }
}