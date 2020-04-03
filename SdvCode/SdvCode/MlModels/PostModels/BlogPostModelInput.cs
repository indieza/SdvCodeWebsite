// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.MlModels.PostModels
{
    using Microsoft.ML.Data;

    public class BlogPostModelInput
    {
        [ColumnName("Content")]
        [LoadColumn(0)]
        public string Content { get; set; }

        [ColumnName("Prediction")]
        [LoadColumn(1)]
        public string Prediction { get; set; }
    }
}