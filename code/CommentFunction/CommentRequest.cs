using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommentFunction
{
    internal class CommentRequest
    {
        public string PagePath { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? WebSite { get; set; }

        public string Content { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(PagePath))
            {
                throw new ArgumentNullException(nameof(PagePath));
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