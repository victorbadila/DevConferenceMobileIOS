
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RectangleDraw
{
	public partial class SecondViewController : UIViewController
	{
		public SecondViewController () : base ("SecondViewController", null)
		{
		}

		public SecondViewController (IntPtr p) : base(p) { }

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var itsworking = new UILabel (new RectangleF (50, 50, 100, 50));
			itsworking.BackgroundColor = UIColor.Black;
			View.AddSubview (itsworking);
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

