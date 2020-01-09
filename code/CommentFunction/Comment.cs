using CommentFunction;
using System;
using YamlDotNet.Serialization;

namespace SearchFunction
{
    internal class Comment
    {
        public Comment(CommentRequest commentRequest)
        {
            commentRequest.Validate();

            Author = new Author(commentRequest.UserName, commentRequest.WebSite);
            Date = DateTime.Now;
            Content = commentRequest.Content;
            Id = new
            {
                commentRequest.PagePath,
                commentRequest.UserName,
                commentRequest.Content,
                Date
            }.GetHashCode();
        }

        public int Id { get; }

        public Author Author { get; }

        public DateTime Date { get; }

        public string Content { get; }

        public string AsYaml()
        {
            var serializer = new Serializer();

            return serializer.Serialize(this);
        }
    }
}