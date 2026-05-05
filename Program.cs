using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManagerApp
{
    public interface IDeletable
    {
        void Delete();
    }
    
    public abstract class FileItem : IDeletable
    {
        public string Name { get; set; }
        public double SizeMB { get; set; }
        public double BaseOpeningSpeed { get; set; }

        protected FileItem(string name, double sizeMB, double baseSpeed)
        {
            Name = name;
            SizeMB = sizeMB;
            BaseOpeningSpeed = baseSpeed;
        }
        
        public abstract double CalculateLoadTime();

        public virtual string GetInfo()
        {
            return $"Файл: {Name}, Розмір: {SizeMB} MB";
        }

        public void Delete()
        {
            Console.WriteLine($"Файл '{Name}' видалено з пам'яті.");
        }
    }

    public class TextFile : FileItem
    {
        public TextFile(string name, double sizeMB) : base(name, sizeMB, 100) { }

        public override double CalculateLoadTime()
        {
            return SizeMB / (BaseOpeningSpeed * 10); 
        }
    }

    public class ImageFile : FileItem
    {
        public int ResolutionPx { get; set; }

        public ImageFile(string name, double sizeMB, int resolutionPx) 
            : base(name, sizeMB, 50) 
        {
            ResolutionPx = resolutionPx;
        }

        public override double CalculateLoadTime()
        {
            return (SizeMB / BaseOpeningSpeed) + (ResolutionPx / 1_000_000.0) * 0.1;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Роздільна здатність: {ResolutionPx} px";
        }
    }

    public class VideoFile : FileItem
    {
        public int DurationSeconds { get; set; }

        public VideoFile(string name, double sizeMB, int durationSeconds) 
            : base(name, sizeMB, 20)
        {
            DurationSeconds = durationSeconds;
        }

        public override double CalculateLoadTime()
        {
            return (SizeMB / BaseOpeningSpeed) + (DurationSeconds / 60.0) * 0.5;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Тривалість: {DurationSeconds} сек";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<FileItem> files = new List<FileItem>
            {
                new TextFile("lab1.txt", 0.5),
                new ImageFile("vacation.jpg", 5.2, 12_000_000),
                new VideoFile("lecture.mp4", 450, 3600),
            };

            double totalTime = 0;
            FileItem maxTimeFile = null;
            double maxTime = -1;

            Console.WriteLine("--- Список файлів ---");
            foreach (var file in files)
            {
                double currentTime = file.CalculateLoadTime();
                totalTime += currentTime;

                Console.WriteLine($"{file.GetInfo()} | Час відкриття: {currentTime:F4} сек");

                if (currentTime > maxTime)
                {
                    maxTime = currentTime;
                    maxTimeFile = file;
                }
            }

            Console.WriteLine("\n--- Аналітика ---");
            if (files.Count > 0)
            {
                Console.WriteLine($"Середній час відкриття: {totalTime / files.Count:F4} сек");
                Console.WriteLine($"Файл з найбільшим часом завантаження: {maxTimeFile.Name} ({maxTime:F4} сек)");
            }
        }
    }
}