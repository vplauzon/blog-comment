using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommentFunction
{
    internal class CommentRequest
    {
        public int Year { get; set; }

        public byte Quarter { get; set; }

        public string PostName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public Uri? WebSite { get; set; }

        public string Content { get; set; } = string.Empty;

        public void Validate()
        {
            if (Year < 2010 || Year > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(Year));
            }
            if (Quarter < 1 || Quarter > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(Quarter));
            }
            if (string.IsNullOrWhiteSpace(PostName) || PostName.Contains('/'))
            {
                throw new ArgumentNullException(nameof(PostName));
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new ArgumentNullException(nameof(UserName));
            }
            if (string.IsNullOrWhiteSpace(Content))
            {
                throw new ArgumentNullException(nameof(Content));
            }
        }
    }
}