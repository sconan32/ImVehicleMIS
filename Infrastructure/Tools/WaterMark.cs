using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Brushes;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats;

namespace Socona.ImVehicle.Infrastructure.Tools
{
    public enum MarkType
    {
        Text, Image
    }
    /**//// <summary>
    /// 给图片添加水印得类得描述
    /// </summary>
    public static class WaterMark
    {



        #region ---------------------方法事件---------------------
        public static MemoryStream Mark(MemoryStream inStream, string text)
        {

            text = $"╞甘井子区重点车辆管理系统:{text}╡";
            //首先先判断该图片是否是 gif动画，如果为gif动画不对图片进行改动
            using (Image<Rgba32> img = Image.Load(inStream.ToArray(), out IImageFormat format))
            {

                FontFamily fontFamily = null;
                SystemFonts.TryFind("sans-serif", out fontFamily);
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("serif", out fontFamily);
                }
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("等线", out fontFamily);
                }
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("DengXian", out fontFamily);
                }
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("宋体", out fontFamily);
                }
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("SimSun", out fontFamily);
                }
                if (fontFamily == null)
                {
                    SystemFonts.TryFind("Arial", out fontFamily);
                }
                var font = fontFamily.CreateFont(14f);
                using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(font, text, Rgba32.FromHex("ffffffdd"), 20, false)))
                {

                    IImageEncoder encoder = null;
                    if (format == ImageFormats.Bmp)
                    {
                        encoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
                    }
                    else if (format == ImageFormats.Jpeg)
                    {
                        encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                    }
                    else if (format == ImageFormats.Png)
                    {
                        encoder = new SixLabors.ImageSharp.Formats.Png.PngEncoder();
                    }
                    else if (format == ImageFormats.Gif)
                    {
                        encoder = new SixLabors.ImageSharp.Formats.Gif.GifEncoder();
                    }
                    MemoryStream outStream = new MemoryStream();
                    img2.Save(outStream, encoder);
                    outStream.Close();
                    return outStream;
                }
            }
        }

        public static IImageProcessingContext<TPixel> ApplyScalingWaterMarkSimple<TPixel>(this IImageProcessingContext<TPixel> processingContext, Font font, string text, TPixel color, float padding)
          where TPixel : struct, IPixel<TPixel>
        {
            return processingContext.Apply(img =>
            {
                float targetWidth = img.Width - (padding * 2);
                float targetHeight = img.Height - (padding * 2);

                // measure the text size
                SizeF size = TextMeasurer.Measure(text, new RendererOptions(font));

                //find out how much we need to scale the text to fill the space (up or down)
                float scalingFactor = Math.Min(img.Width / size.Width, img.Height / size.Height);

                //create a new font 
                Font scaledFont = new Font(font, scalingFactor * font.Size);

                var center = new PointF(img.Width / 2, 20);//img.Height / 2);
                var c1 = new PointF(center.X - 1, center.Y - 1);
                var c2 = new PointF(center.X + 1, center.Y - 1);
                var c3 = new PointF(center.X - 1, center.Y + 1);
                var c4 = new PointF(center.X + 1, center.Y + 1);
                
                img.Mutate(i =>
                {
                    var tempColor = color;
                    tempColor.PackFromRgba32(Rgba32.FromHex("dddddd88"));
                    i.DrawText(text, scaledFont, tempColor, c1, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c2, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c3, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c4, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, color, center, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                });
            });
        }
        public static IImageProcessingContext<TPixel> ApplyScalingWaterMark<TPixel>(this IImageProcessingContext<TPixel> processingContext, Font font, string text, TPixel color, float padding, bool wordwrap)
         where TPixel : struct, IPixel<TPixel>
        {
            if (wordwrap)
            {
                return processingContext.ApplyScalingWaterMarkWordWrap(font, text, color, padding);
            }
            else
            {
                return processingContext.ApplyScalingWaterMarkSimple(font, text, color, padding);
            }
        }

        public static IImageProcessingContext<TPixel> ApplyScalingWaterMarkWordWrap<TPixel>(this IImageProcessingContext<TPixel> processingContext, Font font, string text, TPixel color, float padding)
            where TPixel : struct, IPixel<TPixel>
        {
            return processingContext.Apply(img =>
            {
                float targetWidth = img.Width - (padding * 2);
                float targetHeight = img.Height - (padding * 2);

                float targetMinHeight = img.Height - (padding * 3); // must be with in a margin width of the target height

                // now we are working i 2 dimensions at once and can't just scale because it will cause the text to 
                // reflow we need to just try multiple times

                var scaledFont = font;
                SizeF s = new SizeF(float.MaxValue, float.MaxValue);

                float scaleFactor = (scaledFont.Size / 2);// everytime we change direction we half this size
                int trapCount = (int)scaledFont.Size * 2;
                if (trapCount < 10)
                {
                    trapCount = 10;
                }

                bool isTooSmall = false;

                while ((s.Height > targetHeight || s.Height < targetMinHeight) && trapCount > 0)
                {
                    if (s.Height > targetHeight)
                    {
                        if (isTooSmall)
                        {
                            scaleFactor = scaleFactor / 2;
                        }

                        scaledFont = new Font(scaledFont, scaledFont.Size - scaleFactor);
                        isTooSmall = false;
                    }

                    if (s.Height < targetMinHeight)
                    {
                        if (!isTooSmall)
                        {
                            scaleFactor = scaleFactor / 2;
                        }
                        scaledFont = new Font(scaledFont, scaledFont.Size + scaleFactor);
                        isTooSmall = true;
                    }
                    trapCount--;

                    s = TextMeasurer.Measure(text, new RendererOptions(scaledFont)
                    {
                        WrappingWidth = targetWidth
                    });
                }

                var center = new PointF(padding, img.Height / 2);
                var c1 = new PointF(center.X - 1, center.Y - 1);
                var c2 = new PointF(center.X + 1, center.Y - 1);
                var c3 = new PointF(center.X - 1, center.Y + 1);
                var c4 = new PointF(center.X + 1, center.Y + 1);
                img.Mutate(i =>
                {
                    var tempColor = color;
                    tempColor.PackFromRgba32(Rgba32.FromHex("dddddd88"));
                    i.DrawText(text, scaledFont, tempColor, c1, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c2, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c3, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, tempColor, c4, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    i.DrawText(text, scaledFont, color, center, new TextGraphicsOptions(true)
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        WrapTextWidth = targetWidth
                    });

                }
                );
            });
        }
    }
    #endregion
}
