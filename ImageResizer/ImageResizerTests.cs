using System;
using System.Linq;
using Ploeh.AutoFixture;
using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace ImageResizer
{
    public class ImageResizerTests
    {
        static readonly byte[] gif25x37px =
        {
            0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x19, 0x0, 0x25, 0x0, 0x80, 0x0, 0x0, 0xFF, 0xFF, 0xFF, 
            0x0, 0x0, 0x0, 0x21, 0xF9, 0x4, 0x0, 0x0, 0x0, 0x0, 0x0, 0x2C, 0x0, 0x0, 0x0, 0x0, 0x19, 0x0,
            0x25, 0x0, 0x0, 0x2, 0x1D, 0x84, 0x8F, 0xA9, 0xCB, 0xED, 0xF, 0xA3, 0x9C, 0xB4, 0xDA, 0x8B,
            0xB3, 0xDE, 0xBC, 0xFB, 0xF, 0x86, 0xE2, 0x48, 0x96, 0xE6, 0x89, 0xA6, 0xEA, 0xCA, 0xB6, 0xAE,
            0x56, 0x0, 0x0, 0x3B
        };
        private IImageResizer target;
        private byte[] result;

        private void VerifyExpectedSizeOf(Size expected)
        {
            using (var actual = System.Drawing.Image.FromStream(new MemoryStream(result)))
            {
                Assert.AreEqual(expected.Width, actual.Width);
                Assert.AreEqual(expected.Height, actual.Height);
            }
        }

        [SetUp]
        public void Init()
        {
            target = new ImageResizer();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ResizeIfLargerThan_WithInvalidBytes()
        {
            target.ResizeIfLargerThan(new Fixture().Create<byte[]>(), new Size(10, 10));
        }

        [Test]
        public void ResizeIfLargerThan_WithNullImage()
        {
            result = target.ResizeIfLargerThan(null, new Size(25, 37));
            Assert.IsNull(result);
        }

        [Test]
        public void ResizeIfLargerThan_WithEmptyBytes()
        {
            result = target.ResizeIfLargerThan(new byte[0], new Size(25, 37));
            Assert.IsFalse(result.Any());
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ResizeIfLargerThan_WithTooSmallMaxSize()
        {
            target.ResizeIfLargerThan(gif25x37px, new Size(1, 1));
        }

        [Test]
        public void ResizeIfLargerThan_WithNotLargerImage()
        {
            result = target.ResizeIfLargerThan(gif25x37px, new Size(25, 37));
            Assert.AreEqual(gif25x37px, result);
        }

        [Test]
        public void ResizeIfLargerThan_WithLargerWidth()
        {
            result = target.ResizeIfLargerThan(gif25x37px, new Size(10, 40));
            VerifyExpectedSizeOf(new Size(10, 15));
        }

        [Test]
        public void ResizeIfLargerThan_WithLargerHeight()
        {
            result = target.ResizeIfLargerThan(gif25x37px, new Size(30, 10));
            VerifyExpectedSizeOf(new Size(7, 10));
        }
    }
}
