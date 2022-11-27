namespace WhatsStatusApp.Models;
public class Status
{
    [PrimaryKey]
    public string Id { get; set; }
    public string Text { get; set; }
    public string ColorHex { get; set; }
    public TextTransform TextTransform { get; set; }
    public string Font { get; set; }
    public DateTime DateCreated { get; set; }

    [Ignore]
    public Color Color { get; private set; }

    public Status CreateNewStatusModel(MainPageViewModel vm)
    {
        Id = ShortGuid.NewGuid().ToString();
        Text = vm.StatusText;
        ColorHex = vm.StatusBackgroundColor.ToArgbHex();
        TextTransform = vm.StatusTextTransform;
        TextTransform = vm.StatusTextTransform;
        Font = vm.StatusFont;
        DateCreated = DateTime.Now;
        return this;
    }

    public void SetStatusBackGroundColor()
        => Color = Color.FromArgb(ColorHex);
}
