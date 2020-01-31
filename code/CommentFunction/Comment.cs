using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CommentFunction
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
                commentRequest.Year,
                commentRequest.Quarter,
                commentRequest.PostName,
                commentRequest.UserName,
                commentRequest.Content,
                Date
            }.GetHashCode().ToString("x8");
        }

        public string Id { get; }

        public Author Author { get; }

        public DateTime Date { get; }

        public string Content { get; }

        public string AsYaml()
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(this);
        }
    }
}