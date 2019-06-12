using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using LabelHtml.Forms.Plugin.Droid;
//using LabelHtml.Forms.Plugin.Droid;

namespace BdP_MV.Droid
{
	[Activity (Label = "BdP MV", Icon = "@drawable/lilie", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);
            HtmlLabelRenderer.Initialize();
            global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new BdP_MV.App ());
		}
	}
}

