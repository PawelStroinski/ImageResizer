ImageResizer
============
[![Build Status](https://travis-ci.org/PawelStroinski/ImageResizer.svg?branch=master)](https://travis-ci.org/PawelStroinski/ImageResizer)

Just this

    interface IImageResizer
    {
        byte[] ResizeIfLargerThan(byte[] image, Size maxSize);
    }
