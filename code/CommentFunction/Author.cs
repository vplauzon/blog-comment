using System;

namespace SearchFunction
{
    internal class Author
    {
        public Author(string name, string? urlText)
        {
            Name = name;

            if (!string.IsNullOrWhiteSpace(urlText))
            {
                try
                {
                    Url = new Uri(urlText);
                }
                catch
                {   //  Ignore, keep url null
                }
            }
        }

        public string Name { get; }

        public Uri? Url { get; }
    }
}