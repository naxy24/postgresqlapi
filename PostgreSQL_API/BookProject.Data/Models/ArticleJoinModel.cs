﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookProject.Data.Models
{
    public class ArticleJoinModel
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int MagazineId { get; set; }
        public string MagazineName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
