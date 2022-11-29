namespace WhatsStatusApp.Models;
public class Status
{
    [PrimaryKey]
    public string Id { get; set; }
    public string Text { get; set; }
    public string ColorHex { get; set; }
    public string Font { get; set; }
    public double Size { get; set; } = 35.0;
    public DateTime DateCreated { get; set; }

    [Ignore]
    public Color Color { get; private set; }

    public Status CreateNewStatusModel(MainPageViewModel vm)
    {
        Id = ShortGuid.NewGuid().ToString();
        Text = vm.StatusText.Trim();
        ColorHex = vm.StatusBackgroundColor.ToArgbHex();
        Size = vm.StatusFontSize;
        Font = vm.StatusFont;
        DateCreated = DateTime.Now;
        return this;
    }

    public void SetStatusBackGroundColor()
        => Color = Color.FromArgb(ColorHex);
}

public class StatusGroup : List<Status>
{
    public DateTime Date { get; set; }
    public StatusGroup(List<Status> statuses) : base(statuses)
    {
        statuses.ForEach(s => s.SetStatusBackGroundColor());
        Date = statuses.First().DateCreated;
    }
}
