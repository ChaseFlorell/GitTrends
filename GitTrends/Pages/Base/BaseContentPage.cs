﻿using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GitTrends
{
    public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel
    {
        protected BaseContentPage(in string title, in T viewModel)
        {
            SetDynamicResource(BackgroundColorProperty, nameof(BaseTheme.PageBackgroundColor));
            BindingContext = ViewModel = viewModel;
            Title = title;

            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            SetSafeArea();

            DeviceDisplay.MainDisplayInfoChanged += HandleDisplayInfoChanged;
        }

        protected T ViewModel { get; }

        protected Task OpenBrowser(Uri uri) => OpenBrowser(uri.ToString());

        protected Task OpenBrowser(string url)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                var browserOptions = new BrowserLaunchOptions
                {
                    PreferredToolbarColor = (Color)Xamarin.Forms.Application.Current.Resources[nameof(BaseTheme.NavigationBarBackgroundColor)],
                    PreferredControlColor = (Color)Xamarin.Forms.Application.Current.Resources[nameof(BaseTheme.NavigationBarTextColor)],
                };

                return Browser.OpenAsync(url, browserOptions);
            });
        }

        protected virtual void HandleDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            SetSafeArea();
            ForceLayout();
        }

        void SetSafeArea()
        {
            var isLandscape = DeviceDisplay.MainDisplayInfo.Orientation is DisplayOrientation.Landscape;
            On<iOS>().SetUseSafeArea(isLandscape);
        }
    }
}
