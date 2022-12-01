namespace WhatsStatusApp.Resources.Templates;

public class TextTemplate : ContentView
{
    public static readonly BindableProperty IconGlyphProperty = BindableProperty.Create(nameof(IconGlyph), typeof(string), typeof(TextTemplate), string.Empty);
    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(TextTemplate), string.Empty);

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }
    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }
}
