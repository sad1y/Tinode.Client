using System;
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

            Assert.Throws<ArgumentException>(() => msg.ImageLink("", "mime", 300, 300));
            Assert.Throws<ArgumentException>(() => msg.ImageLink("url", "", 300, 300));
        }

        [Fact]
        public void SetImageLink_ThrowException_IfWidthOrHeightIsLessThatOne()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageLink("url", "mime", 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageLink("url", "mime", 0, 1));
        }

        [Fact]
        public void SetImageBlob_ThrowException_IfUrlOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.ImageBlob("", "mime", 300, 300));
            Assert.Throws<ArgumentException>(() => msg.ImageBlob("url", "", 300, 300));
        }

        [Fact]
        public void SetImageBlob_ThrowException_IfWidthOrHeightIsLessThatOne()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageBlob("url", "mime", 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => msg.ImageBlob("url", "mime", 0, 1));
        }

        [Fact]
        public void SetAttachRef_ThrowException_IfUrlOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.AttachRef("url", ""));
            Assert.Throws<ArgumentException>(() => msg.AttachRef("", "mime"));
        }

        [Fact]
        public void AttachBlob_ThrowException_IfBlobOrMimeIsEmpty()
        {
            var msg = new DraftyMessage("text");

            Assert.Throws<ArgumentException>(() => msg.AttachBlob("blob", ""));
            Assert.Throws<ArgumentException>(() => msg.AttachBlob("", "mime"));
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
    }
}