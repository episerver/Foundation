using EPiServer.Core;
using Foundation.Cms.Media;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Demo
{
    public class ImageHelper
    {
        private readonly ComputerVisionClient _computerVisionClient;

        public ImageHelper()
        {
            _computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(ConfigurationManager.AppSettings["Vision:Key"]))
            {
                Endpoint = ConfigurationManager.AppSettings["Vision:ApiRoot"]
            };
        }

        public async Task TagImagesAsync(ImageMediaData imageFile)
        {
            var analysisResult = await DescribeImageFromStreamAsync(imageFile);
            ProcessAnalysisResult(analysisResult, imageFile);
        }

        private void ProcessAnalysisResult(ImageAnalysis result, ImageMediaData imageFile)
        {
            if (result == null || imageFile == null)
            {
                return;
            }

            if (result.ImageType != null)
            {
                string clipArtType;
                switch (result.ImageType.ClipArtType)
                {
                    case 0:
                        clipArtType = "Non-clipart";
                        break;
                    case 1:
                        clipArtType = "Ambiguous";
                        break;
                    case 2:
                        clipArtType = "Normal clipart";
                        break;
                    case 3:
                        clipArtType = "Good clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }

                imageFile.ClipArtType = clipArtType;

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType)
                {
                    case 0:
                        lineDrawingType = "Non-lineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }

                imageFile.LineDrawingType = lineDrawingType;
            }

            if (result.Adult != null)
            {
                imageFile.IsAdultContent = result.Adult.IsAdultContent;
                imageFile.IsRacyContent = result.Adult.IsRacyContent;
            }

            if (result.Categories != null && result.Categories.Count > 0)
            {
                imageFile.ImageCategories = result.Categories.Select(c => c.Name).ToArray();
            }

            if (result.Color != null)
            {
                imageFile.AccentColor = result.Color.AccentColor;
                imageFile.DominantColorBackground = result.Color.DominantColorBackground;
                imageFile.DominantColorForeground = result.Color.DominantColorForeground;
                imageFile.IsBwImg = result.Color.IsBWImg;

                if (result.Color.DominantColors != null && result.Color.DominantColors.Count > 0)
                {
                    imageFile.DominantColors = result.Color.DominantColors;
                }
            }

            if ((imageFile.Tags == null || imageFile.Tags.Count == 0) && result.Tags != null)
            {
                imageFile.Tags = result.Tags.Where(t => t.Confidence > 0.5).Select(t => t.Name).ToArray();
            }

            if (result.Description != null)
            {
                imageFile.Caption = result.Description.Captions.OrderByDescending(c => c.Confidence).FirstOrDefault()?.Text;

                if (imageFile.Tags == null || imageFile.Tags.Count == 0)
                {
                    imageFile.Tags = result.Description.Tags;
                }
            }
        }

        private async Task<ImageAnalysis> DescribeImageFromStreamAsync(ImageData imageData)
        {
            return await _computerVisionClient.AnalyzeImageInStreamAsync(imageData.BinaryData.OpenRead(), new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            });
        }

    }
}