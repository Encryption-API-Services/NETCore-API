﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Blog
{
    public class UpdateBlogPost
    {
        public string BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogBody { get; set; }
    }
}
