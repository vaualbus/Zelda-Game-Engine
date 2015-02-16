using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.OpenGL.GameEngineClasses.ResourceLoader
{
    public class Texture2D : IGameResourceLoader
    {
        private Texture _texture;

        public string AssetDirectorySuffix
        {
            get { return "Textures"; }
        }

        public string Name
        {
            get
            {
                return string.Format("Resource loader for loading texture with extension {0}", string.Join(", ", AssetFileExtensions));
            }
        }

        public IEnumerable<string> AssetFileExtensions
        {
            get
            {
                return new List<string>
                {
                    "jpg",
                    "png",
                };
            }
        }

        public Bitmap Bitmap { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public ColorPalette ColorPalette { get; private set; }

        public Texture2D()
        {
        }

        public object LoadObject(string pathName)
        {
            try
            {
                _texture = new Texture(pathName);
                return new Texture2D
                {
                    Bitmap = _texture.Bitmap,
                    Width = _texture.Bitmap.Width,
                    Height = _texture.Bitmap.Height,
                    ColorPalette = _texture.Bitmap.Palette

                };
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void Bind()
        {
            if (_texture != null)
                _texture.Bind();
        }

        public void SetTransperency()
        {
            Bitmap.MakeTransparent();
        }

        public Color GetPixelColor(int x, int y)
        {
            return Bitmap.GetPixel(x, y);
        }

        public void SetPixel(int x, int y, Color color)
        {
            Bitmap.SetPixel(x, y, color);
        }

    }
}