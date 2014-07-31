using System;
using System.Drawing;

using MonoTouch.UIKit;

namespace RectangleDraw
{
	public class ConferenceEventButton : UIButton
	{

		public static UIColor UnselectedColor = UIColor.FromRGB(204, 255, 255);

		public static UIColor SelectedColor = UIColor.FromRGB(145, 200, 187);

		public bool IsSelected = false;

		/// <summary>
		/// Constructor method
		/// </summary>
		/// <param name="conf">Conference event to which button corresponds to.</param>
		/// <param name="rectangle">Rectangle frame with specific positioning.</param>
		public ConferenceEventButton (ConferenceEvent conf, RectangleF rectangle)
		{
			conferenceEvent = conf;
			BackgroundColor = UnselectedColor;
			SetTitleColor (UIColor.Black, UIControlState.Normal);
			Font = UIFont.FromName("Helvetica", 9f);
			SetTitle (conf.CreateBy, UIControlState.Normal);
			Frame = rectangle;
		}

		public ConferenceEvent conferenceEvent { get; set; }
	}
}

