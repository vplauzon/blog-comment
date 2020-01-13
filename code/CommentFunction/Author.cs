using System;

namespace SearchFunction
{
    internal class Author
    {
        public Author(string name, Uri? url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; }
        
        public Uri? Url { get; }
    }
}