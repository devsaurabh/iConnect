using System;
using System.Windows;
using System.Windows.Media;

namespace iConnect_Client.Utilities
{
    public class HelperFunctions
    {
        public static string GetDefaultImage()
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("../Resources/ImageResources.xaml", UriKind.RelativeOrAbsolute)
            };
            var defaultImage = (ImageSource)resourceDictionary["UserDefaultImage"];
            return defaultImage.ToString();

        }

        public static string GetOnlineImage()
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("../Resources/ImageResources.xaml", UriKind.RelativeOrAbsolute)
            };
            var defaultImage = (ImageSource)resourceDictionary["OnlineIcon"];
            return defaultImage.ToString();

        }
    }
}
