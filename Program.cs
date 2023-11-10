
using System;
using System.Net.Security;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace ComputerVIsionDemo
{

    class Program
    {

        static string subscriptionKey = "YOUR-KEY-HERE";
        static string endpoint = "YOUR-ENDPOINT-HERE";
        private const string ANALYZE_URL_IMAGE = "YOUR-IMG-URL-HERE";

        static void Main(string[] args)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            AnalyzeImageURL(client, ANALYZE_URL_IMAGE).Wait();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        public static async Task AnalyzeImageURL(ComputerVisionClient client, string imageUrl)
        {
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color,
                VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            //Analyze the URL image
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            Console.WriteLine("Summary: ");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence{caption.Confidence}");
            }
            Console.WriteLine("");

            Console.WriteLine("Categories: ");
            foreach (var category in results.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            Console.WriteLine("");

            Console.WriteLine("Tags: ");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} with confidence {tag.Confidence}");
            }
            Console.WriteLine("");

            Console.WriteLine("Objects: ");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");
            }
            Console.WriteLine("End");
        }

    }
}
