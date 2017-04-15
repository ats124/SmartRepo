using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ResolutionGroupName("SmartRepo")]
[assembly: ExportEffect(typeof(Softentertainer.SmartRepo.iOS.NoBorderEffect), "NoBorderEffect")]

namespace Softentertainer.SmartRepo.iOS
{
	public class NoBorderEffect : PlatformEffect
	{
		public NoBorderEffect()
		{
		}

		protected override void OnAttached()
		{
			var uiTxtFieald = (UITextField)Control;
			uiTxtFieald.BorderStyle = UITextBorderStyle.None;
			uiTxtFieald.TextAlignment = UITextAlignment.Right;
		}

		protected override void OnDetached()
		{
			// throw new NotImplementedException();
		}
	}
}
