﻿namespace WhatsStatusApp;

public partial class MainPage : ContentPage
{
	readonly MainPageViewModel vm = new();
	public MainPage()
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

