namespace WhatsStatusApp.Models;
internal class Status
{
    public string Id { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; }
    public TextTransform TextTransform { get; set; }
    public string FontFamily { get; set; }
    public DateTime DateCreated { get; set; }

    public Status CreateNewStatusModel(MainPageViewModel vm)
    {
        Id = Guid.NewGuid().ToString();
        Text = vm.StatusText;
        Color = vm.StatusBackgroundColor;
        TextTransform = vm.StatusTextTransform;
        TextTransform = vm.StatusTextTransform;
        FontFamily = vm.StatusTextFamily;
        DateCreated = DateTime.Now;
        return this;
    }
}
