// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public enum EmojiType
    {
        [Display(Name = "Smiles and People")]
        SmilesAndPeople = 1,
        [Display(Name = "Animals and Nature")]
        AnimalsAndNature = 2,
        [Display(Name = "Eat and Drink")]
        EatAndDrink = 3,
        [Display(Name = "Activities")]
        Activities = 4,
        [Display(Name = "Travel and Places")]
        TravelAndPlaces = 5,
        [Display(Name = "Objects")]
        Objects = 6,
        [Display(Name = "Symbols")]
        Symbols = 7,
    }
}