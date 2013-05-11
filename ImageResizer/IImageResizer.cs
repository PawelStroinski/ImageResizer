using System.Drawing;

namespace ImageResizer
{
    interface IImageResizer
    {
        byte[] ResizeIfLargerThan(byte[] image, Size maxSize);
    }
}
