namespace WhatsStatusApp.Models;
internal class Status
{
    public string Id { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; }
    public TextTransform TextTransform { get; set; }
    public string FontFamily { get; set; }
    public DateTime DateCreated { get; set; }
}
