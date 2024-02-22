using System.Net;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

public static class Extensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }

    public static ImageDescriptor ImageFromUrl(this IContainer container, string filePath)
    {

        byte[] imageBytes = null;
        try
        {
            var webClient = new WebClient();
            if (webClient == null) throw new ArgumentNullException(nameof(webClient));
            imageBytes =
            webClient.DownloadData(filePath);
        }
        catch (Exception ex)
        {
            return container.Image("./Templates/No_Image_Available.jpg");
        }
        return container.Image(imageBytes);
    }
}