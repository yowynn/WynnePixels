using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class WynnePixels
{
    Bitmap a;
    public void Test(string imgPath)
    {
        Bitmap b = (Bitmap)Bitmap.FromFile(imgPath);
        Graphics g = Graphics.FromImage(b);
        b.GetThumbnailImage(50, 50, null, IntPtr.Zero);
    }

    public static Image ReadImage(string imgPath)
    {
        Image img;
        using (FileStream s = new FileStream(imgPath, FileMode.Open))
        {
            img = Image.FromStream(s);
        }
        return img;
    }

    public static Image ScaleTo(Image img, int width, int height)
    {
        float scale = Math.Max((float)width / img.Width, (float)height / img.Height);
        width = (int)(img.Width * scale);
        height = (int)(img.Height * scale);
        return img.GetThumbnailImage(width, height, null, IntPtr.Zero);
    }

    public static Image MapColor(Image img, List<Color> colorList)
    {
        var b = (Bitmap)img; 
        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                var color = b.GetPixel(x, y);
                var index = FindSimilarColor(color, colorList);
                var newcolor = colorList[index];
                b.SetPixel(x, y, newcolor);
            }
        }
        return b;
    }

    public static Image MapColor(Image img, List<Color> colorList, out int[] usage)
    {
        usage = new int[colorList.Count];
        var b = (Bitmap)img;
        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                var color = b.GetPixel(x, y);
                var index = FindSimilarColor(color, colorList);
                usage[index]++;
                var newcolor = colorList[index];
                b.SetPixel(x, y, newcolor);
            }
        }
        return b;
    }

    public static int FindSimilarColor(Color color, List<Color> colorList)
    {
        int index = -1;
        var diff = float.MaxValue;
        for (int i = 0; i < colorList.Count;  i++)
        {
            var c = colorList[i];
            var dr = Math.Abs(c.R - color.R);
            var dg = Math.Abs(c.G - color.G);
            var db = Math.Abs(c.B - color.B);
            var da = Math.Abs(c.A - color.A);
            //var tardif = dr + dg + db + da;
            //var tardif = dr * dr + dg * dg + db * db + da * da;
            var tardif = CIELAB(c, color);
            if (tardif < diff)
            {
                diff = tardif;
                index = i;
            }
        }
        return index;
    }

    public static Color GetColor(string argb)
    {
        if (argb.Length < 8)
        {
            argb = "FFFFFFFF".Substring(0, 8 - argb.Length) + argb;
        }
        var a = int.Parse(argb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        var r = int.Parse(argb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        var g = int.Parse(argb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        var b = int.Parse(argb.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        return Color.FromArgb(a, r, g, b);
    }

    public static float[] Color2Lab(Color color)
    {
        float[] arr = new float[3];
        float B = Gamma(color.B / 255f);
        float G = Gamma(color.G / 255f);
        float R = Gamma(color.R / 255f);
        float X = 0.412453f * R + 0.357580f * G + 0.180423f * B;
        float Y = 0.212671f * R + 0.715160f * G + 0.072169f * B;
        float Z = 0.019334f * R + 0.119193f * G + 0.950227f * B;

        X /= 0.95047f;
        Y /= 1.0f;
        Z /= 1.08883f;

        float FX = X > 0.008856f ? MathF.Pow(X, 1.0f / 3.0f) : (7.787f * X + 0.137931f);
        float FY = Y > 0.008856f ? MathF.Pow(Y, 1.0f / 3.0f) : (7.787f * Y + 0.137931f);
        float FZ = Z > 0.008856f ? MathF.Pow(Z, 1.0f / 3.0f) : (7.787f * Z + 0.137931f);
        arr[0] = Y > 0.008856f ? (116.0f * FY - 16.0f) : (903.3f * Y);
        arr[1] = 500f * (FX - FY);
        arr[2] = 200f * (FY - FZ);
        return arr;
    }

    private static float Gamma(float x)
    {
        return x > 0.04045f ? MathF.Pow((x + 0.055f) / 1.055f, 2.4f) : x / 12.92f;
    }

    public static float CIELAB(Color a, Color b)
    {
        var la = Color2Lab(a);
        var lb = Color2Lab(b);
        return MathF.Sqrt(MathF.Pow(la[0] - lb[0], 2) + MathF.Pow(la[1] - lb[1], 2) + MathF.Pow(la[2] - lb[2], 2));
    }

}
