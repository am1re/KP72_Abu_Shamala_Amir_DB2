using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Database
{
    class SearchResult
    {
        public SearchResult(long id, string attr, string tsHeadline)
        {
            Id = id;
            Attr = attr;
            TsHeadline = tsHeadline;
        }

        public long Id { get; set; }

        public string Attr { get; set; }

        public string TsHeadline { get; set; }
    }
}
