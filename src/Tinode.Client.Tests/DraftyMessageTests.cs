using System;
using System.Collections.Generic;
using Xunit;

namespace Tinode.Client.Tests
{
    public class DraftyMessageTests
    {
        [Fact]
        public void SetBold_ThrowException_IfPositionOrLenOutOfRange()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Bold(5, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Bold(4, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Bold(3, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Bold(3, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Bold(-1, 1));
        }

        [Fact]
        public void SetEmphasize_ThrowException_IfPositionOrLenOutOfRange()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Emphasize(5, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Emphasize(4, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Emphasize(3, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Emphasize(3, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Emphasize(-1, 1));
        }

        [Fact]
        public void SetCode_ThrowException_IfPositionOrLenOutOfRange()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Code(5, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Code(4, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Code(3, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Code(3, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Code(-1, 1));
        }

        [Fact]
        public void SetDeleted_ThrowException_IfPositionOrLenOutOfRange()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Deleted(5, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Deleted(4, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Deleted(3, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Deleted(3, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Deleted(-1, 1));
        }

        [Fact]
        public void SetNextLine_ThrowException_IfPositionOutOfRange()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.NextLine(5));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.NextLine(-1));
        }

        [Fact]
        public void SetLink_ThrowException_IfUrlOrTextIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.Link("", "text"));
            Assert.Throws<ArgumentException>(() => msg.Link("url", ""));
        }

        [Fact]
        public void SetImageLink_ThrowException_IfUrlOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.ImageRef("", "mime", 300, 300));
            Assert.Throws<ArgumentException>(() => msg.ImageRef("url", "", 300, 300));
        }

        [Fact]
        public void SetImageLink_ThrowException_IfWidthOrHeightIsLessThatOne()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageRef("url", "mime", 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageRef("url", "mime", 0, 1));
        }

        [Fact]
        public void SetImageBlob_ThrowException_IfUrlOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.Image("", "mime", 300, 300));
            Assert.Throws<ArgumentException>(() => msg.Image("url", "", 300, 300));
        }

        [Fact]
        public void SetImageBlob_ThrowException_IfWidthOrHeightIsLessThatOne()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Image("url", "mime", 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.Image("url", "mime", 0, 1));
        }

        [Fact]
        public void SetAttachRef_ThrowException_IfUrlOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.BlobRef("url", ""));
            Assert.Throws<ArgumentException>(() => msg.BlobRef("", "mime"));
        }

        [Fact]
        public void AttachBlob_ThrowException_IfBlobOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.Blob("blob", ""));
            Assert.Throws<ArgumentException>(() => msg.Blob("", "mime"));
        }

        [Fact]
        public void Hashtag_ThrowException_IfTextIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.Hashtag(""));
        }

        [Fact]
        public void Mention_ThrowException_IfTextIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.Mention(""));
        }


        [Theory]
        [MemberData(nameof(JsonSerializationSource))]
        public void ToJson_ShouldComplete(DraftyMessage message, string expectedJson)
        {
            var json = message.ToJsonString();

            Assert.Equal(expectedJson, json);
        }

        public static IEnumerable<object[]> JsonSerializationSource()
        {
            yield return new object[]
            {
                DraftyMessage.Create("text").Mention("me"),
                "{\"txt\":\"textme\",\"fmt\":[{\"at\":4,\"len\":2,\"key\":0}],\"ent\":[{\"tp\":\"MN\",\"data\":{\"val\":\"me\"}}]}"
            };

            yield return new object[]
            {
                DraftyMessage
                    .Create()
                    .Code("piece of code"),
                "{\"txt\":\"piece of code\",\"fmt\":[{\"at\":0,\"len\":13,\"tp\":\"CO\"}],\"ent\":[]}"
            };

            yield return new object[]
            {
                DraftyMessage
                    .Create()
                    .Emphasize("look at")
                    .Text(" here ")
                    .Link("google", "https://google.com")
                    .Bold(8, 11),
                "{\"txt\":\"look at here google\"," +
                "\"fmt\":[{\"at\":0,\"len\":7,\"tp\":\"EM\"},{\"at\":13,\"len\":6,\"key\":0},{\"at\":8,\"len\":11,\"tp\":\"ST\"}]," +
                "\"ent\":[{\"tp\":\"LN\",\"data\":{\"url\":\"https://google.com\"}}]}"
            };

            yield return new object[]
            {
                DraftyMessage.Create()
                    .BlobRef("https://google.com", "text/html"),
                "{\"txt\":\"\",\"fmt\":[{\"at\":-1,\"len\":0,\"key\":0}],\"ent\":[{\"tp\":\"EX\",\"data\":{\"mime\":\"text/html\",\"ref\":\"https://google.com\"}}]}"
            };

            yield return new object[]
            {
                DraftyMessage.Create()
                    .BlobRef("https://google.com", "text/html", size: 300)
                    .BlobRef("https://google.com", "text/html", name: "google page")
                    .BlobRef("https://google.com", "text/html", name: "google page", size: 500),
                "{\"txt\":\"\"," +
                "\"fmt\":[" +
                "{\"at\":-1,\"len\":0,\"key\":0}," +
                "{\"at\":-1,\"len\":0,\"key\":1}," +
                "{\"at\":-1,\"len\":0,\"key\":2}]," +
                "\"ent\":[" +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/html\",\"ref\":\"https://google.com\",\"size\":300}}," +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/html\",\"ref\":\"https://google.com\",\"name\":\"google page\"}}," +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/html\",\"ref\":\"https://google.com\",\"name\":\"google page\",\"size\":500}}]}"
            };

            yield return new object[]
            {
                DraftyMessage.Create()
                    .Blob("Q3l0aG9uPT0wPT00LjAuMAo=", "text/plain", size: 300)
                    .Blob("Q3l0aG9uPT0wPT00LjAuMAo=", "text/plain", name: "simple text")
                    .Blob("Q3l0aG9uPT0wPT00LjAuMAo=", "text/plain", name: "simple text", size: 500),
                "{\"txt\":\"\"," +
                "\"fmt\":[" +
                "{\"at\":-1,\"len\":0,\"key\":0}," +
                "{\"at\":-1,\"len\":0,\"key\":1}," +
                "{\"at\":-1,\"len\":0,\"key\":2}]," +
                "\"ent\":[" +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/plain\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"size\":300}}," +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/plain\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"name\":\"simple text\"}}," +
                "{\"tp\":\"EX\",\"data\":{\"mime\":\"text/plain\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"name\":\"simple text\",\"size\":500}}]}"
            };

            yield return new object[]
            {
                DraftyMessage.Create()
                    .Image("Q3l0aG9uPT0wPT00LjAuMAo=", "image/jpeg", 100, 200)
                    .Image("Q3l0aG9uPT0wPT00LjAuMAo=", "image/jpeg", 140, 250, size: 300)
                    .Image("Q3l0aG9uPT0wPT00LjAuMAo=", "image/jpeg", 200, 210, name: "simple image")
                    .Image("Q3l0aG9uPT0wPT00LjAuMAo=", "image/jpeg", 200, 210, name: "simple image", size: 1000),
                "{\"txt\":\"    \"," +
                "\"fmt\":[{\"at\":0,\"len\":1,\"key\":0},{\"at\":1,\"len\":1,\"key\":1},{\"at\":2,\"len\":1,\"key\":2},{\"at\":3,\"len\":1,\"key\":3}]," +
                "\"ent\":[" +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/jpeg\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"width\":100,\"height\":200}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/jpeg\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"width\":140,\"height\":250,\"size\":300}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/jpeg\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"width\":200,\"height\":210,\"name\":\"simple image\"}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/jpeg\",\"val\":\"Q3l0aG9uPT0wPT00LjAuMAo=\",\"width\":200,\"height\":210,\"name\":\"simple image\",\"size\":1000}}]}"
            };
            
            yield return new object[]
            {
                DraftyMessage.Create()
                    .ImageRef("https://i.imgur.com/RHq0ix3.gif", "image/gif", 100, 200)
                    .ImageRef("https://i.imgur.com/RHq0ix3.gif", "image/gif", 140, 250, size: 300)
                    .ImageRef("https://i.imgur.com/RHq0ix3.gif", "image/gif", 200, 210, name: "simple image")
                    .ImageRef("https://i.imgur.com/RHq0ix3.gif", "image/gif", 200, 210, name: "simple image", size: 1000),
                "{\"txt\":\"    \"," +
                "\"fmt\":[{\"at\":0,\"len\":1,\"key\":0},{\"at\":1,\"len\":1,\"key\":1},{\"at\":2,\"len\":1,\"key\":2},{\"at\":3,\"len\":1,\"key\":3}]," +
                "\"ent\":[" +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/gif\",\"ref\":\"https://i.imgur.com/RHq0ix3.gif\",\"width\":100,\"height\":200}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/gif\",\"ref\":\"https://i.imgur.com/RHq0ix3.gif\",\"width\":140,\"height\":250,\"size\":300}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/gif\",\"ref\":\"https://i.imgur.com/RHq0ix3.gif\",\"width\":200,\"height\":210,\"name\":\"simple image\"}}," +
                "{\"tp\":\"IM\",\"data\":{\"mime\":\"image/gif\",\"ref\":\"https://i.imgur.com/RHq0ix3.gif\",\"width\":200,\"height\":210,\"name\":\"simple image\",\"size\":1000}}]}"
            };
        }
    }
}