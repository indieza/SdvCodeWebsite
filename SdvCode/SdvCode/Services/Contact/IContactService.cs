// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Contact
{
    using SdvCode.ViewModels.Contacts;

    public interface IContactService
    {
        void SendEmail(ContactInputModel model);
    }
}