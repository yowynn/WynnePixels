using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var imgfile = @"D:\WynnePixels\target.png";
            var img = WynnePixels.ReadImage(imgfile);
            img = WynnePixels.ScaleTo(img, 88, 88);
            var colorList = new List<Color>
            {
                WynnePixels.GetColor("f99dc6"), // 01 粉色
                WynnePixels.GetColor("f86776"), // 02 桃色
                WynnePixels.GetColor("d658ac"), // 03 玫红
                WynnePixels.GetColor("f03e3a"), // 04 西瓜红
                WynnePixels.GetColor("c91a17"), // 05 红色
                WynnePixels.GetColor("363638"), // 06 黑色
                WynnePixels.GetColor("f7d0a7"), // 07 肤色
                WynnePixels.GetColor("f4ea30"), // 08 淡黄
                WynnePixels.GetColor("ffbe22"), // 09 橙黄
                WynnePixels.GetColor("fa6a2b"), // 10 橘红
                WynnePixels.GetColor("e0d9c7"), // 11 乳白
                WynnePixels.GetColor("eeedeb"), // 12 白色
                WynnePixels.GetColor("5dcebc"), // 13 青色
                WynnePixels.GetColor("7cb824"), // 14 黄绿
                WynnePixels.GetColor("4bda5a"), // 15 草绿
                WynnePixels.GetColor("1e8b48"), // 16 深绿
                WynnePixels.GetColor("0d683c"), // 17 墨绿
                WynnePixels.GetColor("9e9fa4"), // 18 灰色
                WynnePixels.GetColor("44b4e6"), // 19 淡蓝
                WynnePixels.GetColor("2370d8"), // 20 宝蓝
                WynnePixels.GetColor("2845a9"), // 21 深蓝
                WynnePixels.GetColor("8170d0"), // 22 紫色
                WynnePixels.GetColor("a0603d"), // 23 浅咖
                WynnePixels.GetColor("642f27"), // 24 褐色
                WynnePixels.GetColor("212b4f"), // 25 蓝黑
                WynnePixels.GetColor("e1bdbf"), // 26 淡粉
                WynnePixels.GetColor("bbbde3"), // 17 浅紫
                WynnePixels.GetColor("8ec9eb"), // 28 水蓝
                WynnePixels.GetColor("f7c025"), // 29 土黄
                WynnePixels.GetColor("494b58"), // 30 深灰
                WynnePixels.GetColor("4b1c78"), // 31 深紫
            };
            img = WynnePixels.MapColor(img, colorList, out var usage);
            for (int i = 0; i < usage.Length; i++)
            {
                Console.WriteLine($"{i + 1}\t{usage[i]}");
            }
            img.Save("out.bmp",ImageFormat.Bmp);
        }
    }
}
