using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace newHttp
{
    public class RandomWord
    {
        public string word { get; set; }
        public string nameFromHeader { get; set; }

        public RandomWord(string word, string nameFromHeader)
        {
            this.word = word;
            this.nameFromHeader = nameFromHeader;
        }
    }
}
