using System;

using MonoTouch.UIKit;

namespace RectangleDraw
{
	public class ConferenceEventButton : UIButton
	{

		public static UIColor UnselectedColor = UIColor.FromRGB(204, 255, 255);

		public static UIColor SelectedColor = UIColor.FromRGB(145, 200, 187);

		public bool Selected = false;


		public ConferenceEventButton (ConferenceEvent conf)
		{
			conferenceEvent = conf;
			BackgroundColor = UnselectedColor;
		}

		public ConferenceEvent conferenceEvent { get; set; }
	}
}

