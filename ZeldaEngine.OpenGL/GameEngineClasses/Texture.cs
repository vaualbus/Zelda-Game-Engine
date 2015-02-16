using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using ZeldaEngine.Base.Game.RenderingEngine.ResourceManager;

namespace ZeldaEngine.OpenGL.GameEngineClasses
{
    public class Texture
    {
        private static readonly Dictionary<string, TextureResources> LoadedTextures =
            new Dictionary<string, TextureResources>();

        private readonly TextureResources _buffer;

        public Bitmap Bitmap { get; private set; }

        public int[] Pixels { get; private set; }

        public Texture(string fileName)
        {
            if (LoadedTextures.ContainsKey(fileName))
                _buffer = LoadedTextures[fileName];
            else
            {
                _buffer = new TextureResources(LoadTexture(fileName));
                LoadedTextures.Add(fileName, _buffer);
            }
        }

        public void Bind(int samplerSlot = 0)
        {
            Debug.Assert(samplerSlot >= 0 && samplerSlot <= 31);
            GL.ActiveTexture(TextureUnit.Texture0 + samplerSlot);
            GL.BindTexture(TextureTarget.Texture2D, _buffer.Id);
        }

        private int LoadTexture(string fileName)
        {
            var bitmap = new Bitmap(fileName);
            var pixels = GetRGB(bitmap, 0, 0, bitmap.Width, bitmap.Height, 0, bitmap.Width);

            //Still required else TexImage2D will be applyed on the last bound texture
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                Convert.ToInt32(TextureWrapMode.Repeat));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                Convert.ToInt32(TextureWrapMode.Repeat));

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMinFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, bitmap.Width, bitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

            Pixels = GetRGB(bitmap, 0, 0, bitmap.Width, bitmap.Height, 0, bitmap.Width);

            Bitmap = bitmap;

            return texture;
        }

        private static int[] GetRGB(Bitmap image, int startX, int startY, int w, int h, int offset, int scansize)
        {
            const int PixelWidth = 3;
            const System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;

            // En garde!
            if (image == null) throw new ArgumentNullException("image");
            if (startX < 0 || startX + w > image.Width) throw new ArgumentOutOfRangeException("startX");
            if (startY < 0 || startY + h > image.Height) throw new ArgumentOutOfRangeException("startY");
            if (w < 0 || w > scansize || w > image.Width) throw new ArgumentOutOfRangeException("w");
            //if (h < 0 || (rgbArray.Length < offset + h * scansize) || h > image.Height) throw new ArgumentOutOfRangeException("h");

            BitmapData data = image.LockBits(new Rectangle(startX, startY, w, h),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, pixelFormat);
            var rgbArray = new int[w*h];

            try
            {
                byte[] pixelData = new Byte[data.Stride];
                for (int scanline = 0; scanline < data.Height; scanline++)
                {
                    Marshal.Copy(data.Scan0 + (scanline*data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                    {
                        // PixelFormat.Format32bppRgb means the data is stored
                        // in memory as BGR. We want RGB, so we must do some 
                        // bit-shuffling.
                        rgbArray[offset + (scanline*scansize) + pixeloffset] =
                            (pixelData[pixeloffset*PixelWidth + 2] << 16) + // R 
                            (pixelData[pixeloffset*PixelWidth + 1] << 8) + // G
                            pixelData[pixeloffset*PixelWidth]; // B
                    }
                }
            }
            finally
            {
                image.UnlockBits(data);
            }

            return rgbArray;
        }
    }
}