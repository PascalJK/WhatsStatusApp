using OpenGraphNet.Metadata;

namespace WhatsStatusApp.Models;

public class Link
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string URL { get; set; }

    readonly string noDescription = @"no description";
    readonly string defaultImage = @"https://img.icons8.com/emoji/80/000000/link-emoji.png";

    public Link DecodeMetaInformation(OpenGraph og)
    {
        URL = og.OriginalUrl.OriginalString;

        Title = og.Metadata["og:title"].Value();
        if(string.IsNullOrWhiteSpace(Title))
            Title = og.Metadata["og:site_name"].Value();

        Description = og.Metadata["og:description"].Value();
        if(string.IsNullOrWhiteSpace(Description))
            Description = noDescription;

        Image = og.Metadata["og:image"].Value();
        if(string.IsNullOrWhiteSpace(Image))
            Image = defaultImage;
        return this;

    }
}