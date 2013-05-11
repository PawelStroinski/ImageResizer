ImageResizer
============
Just this

    interface IImageResizer
    {
        byte[] ResizeIfLargerThan(byte[] image, Size maxSize);
    }
