using System;

namespace CommentFunction
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
                    Url = new Uri(urlText).ToString();
                }
                catch
                {   //  Ignore, keep url null
                }
            }
        }

        public string Name { get; }

        public string? Url { get; }
    }
}