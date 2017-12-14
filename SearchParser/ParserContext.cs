using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.SearchParser
{
    public   class SearchParserContext
    {
    
        public Dictionary<string ,PropertyMap> Properties { get; set; }

        public SearchParserContext()
        {
            Properties = new Dictionary<string, PropertyMap>();
        }

    }
}
