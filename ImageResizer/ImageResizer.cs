// High-quality resizing in Resize() from http://stackoverflow.com/a/87786
using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace ImageResizer
{
    class ImageResizer : IImageResizer
    {
        byte[] imageBytes;
        Size maxSize;
        Size newSize;
        System.Drawing.Image image;
        Bitmap resizedImage;

        public byte[] ResizeIfLargerThan(byte[] image, Size maxSize)
        {
            try
            {
                imageBytes = image;
                this.maxSize = maxSize;
                if (imageBytes != null && imageBytes.Any())
                    ResizeIfLarger();
                return imageBytes;
            }
            catch (ArgumentException e)
            {
                throw new InvalidOperationException("Cannot load or process image", e);
            }
        }

        void ResizeIfLarger()
        {
            CheckMaxSize();
            using (image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes)))
            {
                if (NeedsResize())
                {
                    CalculateNewSize();
                    Resize();
                    SaveResizedImage();
                }
            }
        }

        void CheckMaxSize()
        {
            if (maxSize.Width < 10 || maxSize.Height < 10)
                throw new InvalidOperationException("MaxSize too small");
        }

        bool NeedsResize()
        {
            return image.Width > maxSize.Width || image.Height > maxSize.Height;
        }

        void CalculateNewSize()
        {
            newSize = image.Size;
            if (newSize.Width > maxSize.Width)
            {
                newSize.Width = maxSize.Width;
                newSize.Height = (int)Math.Round(
                    (double)maxSize.Width / (double)image.Width * (double)image.Height);
            }
            if (newSize.Height > maxSize.Height)
            {
                newSize.Height = maxSize.Height;
                newSize.Width = (int)Math.Round(
                    (double)maxSize.Height / (double)image.Height * (double)image.Width);
            }
        }

        void Resize()
        {
            resizedImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, newSize.Width, newSize.Height));
            }
        }

        void SaveResizedImage()
        {
            using (var stream = new MemoryStream())
            {
                resizedImage.Save(stream, image.RawFormat);
                stream.Position = 0;
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                imageBytes = bytes;
            }
        }
    }
}
