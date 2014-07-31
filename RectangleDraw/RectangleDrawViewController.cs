using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;

using RestSharp;
using Newtonsoft.Json;

using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace RectangleDraw
{
	public partial class RectangleDrawViewController : UIViewController
	{

		private int _pixelsPerHour = 25;

		private static ConferenceEventButton _selectedConferenceButton;

		private IList<String> _dates;

		private IList<ConferenceEvent> _conferenceEvents;

		private IList<ConferenceEventButton> _conferenceEventsGraphicElements = new List<ConferenceEventButton>();

		private String testUrl = "http://enigmatic-oasis-8124.herokuapp.com";

		private Int16 verticalIntervalStart = 50;

		public RectangleDrawViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		#region Controller logic

		/// <summary>
		/// Initializes the dates available for editing conference events.
		/// </summary>
		private void InitDates()
		{
			_dates = new List<String> ();
			var date = DateTime.Now;
			for (var i = 0; i < 7; i++) {
				var weekDay = date.DayOfWeek.ToString();
				weekDay = weekDay.Substring (0, 3);

				var dayTitle = String.Format("{0} {1}", weekDay, date.Day);
				_dates.Add (dayTitle);
				date = date.AddDays (1);
			}
		}

		/// <summary>
		/// This method inits the conferences by getting them from the server. Arguments are for the case
		/// in which method is delegate.
		/// </summary>
		private void InitConferencesFromServer(object sender, EventArgs ea)  
		{
			var resultList = MyRestClient.LIST();
			_conferenceEvents = resultList;
			foreach (var confButton in _conferenceEventsGraphicElements) 
			{
				confButton.RemoveFromSuperview ();
				// TODO dealloc?
			}
			_conferenceEventsGraphicElements.Clear ();
			foreach (var conf in _conferenceEvents)
			{
				var button = GenerateButtonFromConferenceEvent (conf);
				if (button != null) 
				{
					_conferenceEventsGraphicElements.Add (button);
					View.AddSubview (button);
				}
			}
		}

		public ConferenceEventButton GenerateButtonFromConferenceEvent(ConferenceEvent conf)
		{
			int day = conf.start_date.DayOfYear - DateTime.Now.DayOfYear;
			if (day < 0) 
			{
				return null;
			}
			var xCoord = day * 38 + 37;
			var yCoord = Convert.ToInt16(DateTimeToPixel (conf.start_date) + verticalIntervalStart);

			var eventDurationToPixels = Convert.ToInt16 (TimeIntervalToPixel (conf.start_date, conf.end_date));

			var reactangle = new RectangleF(xCoord, yCoord, 38, eventDurationToPixels);
			var confButton = new ConferenceEventButton (conf, reactangle);

			confButton.TouchUpInside += delegate(object sender, EventArgs e) {
				if (confButton.IsSelected) {
					confButton.IsSelected = false;
					confButton.BackgroundColor = ConferenceEventButton.UnselectedColor;
					_selectedConferenceButton = null;
				} else {
					if (_selectedConferenceButton != null) {
						_selectedConferenceButton.BackgroundColor = ConferenceEventButton.UnselectedColor;
						_selectedConferenceButton.IsSelected = false;
					}
					confButton.IsSelected = true;
					confButton.BackgroundColor = ConferenceEventButton.SelectedColor;
					_selectedConferenceButton = confButton;
				}
			};			

			return confButton;
		}

		public int TimeIntervalToPixel(DateTime startDate, DateTime endDate)
		{
			var hours = endDate.Hour - startDate.Hour;
			var minutes = endDate.Minute - startDate.Minute;
			var result = hours * _pixelsPerHour + ((minutes * _pixelsPerHour) / 60);
			return result;
		}

		public int DateTimeToPixel(DateTime date)
		{
			return (date.Hour - 8) * _pixelsPerHour + date.Minute * _pixelsPerHour / 60;
		}

		#endregion

		#region Visual elements drawing

		/// <summary>
		/// Draws Add/Edit and Delete buttons.
		/// </summary>
		private void DrawInteractionButtons() 
		{
			var addEditButton = CreateMenuButton ("Add/Edit Selected", 20, 415);
			addEditButton.TouchUpInside += delegate {
				UIStoryboard board = UIStoryboard.FromName ("MainStoryboard", null);
				var secondViewController = (SecondViewController)board.InstantiateViewController ("SecondViewController");
				secondViewController.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				if (_selectedConferenceButton != null) {
					secondViewController._event = _selectedConferenceButton.conferenceEvent;
				}
				this.PresentViewController (secondViewController, true, null);
			};
			var deleteButton = CreateMenuButton ("Delete Selected", 120, 415);
			deleteButton.TouchUpInside += delegate {
				try {
					MyRestClient.DELETE(_selectedConferenceButton.conferenceEvent.Id);
					InitConferencesFromServer(null, null);
				} catch (Exception e) {
					// should not get any errors at delete time
				}
				_selectedConferenceButton = null;
			};
			var refreshButton = CreateMenuButton ("Refresh list", 220, 415); 
			refreshButton.TouchUpInside += InitConferencesFromServer;

			View.AddSubview (addEditButton);
			View.AddSubview (refreshButton);
			View.AddSubview (deleteButton);
		}

		/// <summary>
		/// Draws the horizontal bar containing the whole week.
		/// </summary>
		private void DrawUpperHorizontalBar()
		{
			var colorFlag = UIColor.Yellow;
			var startPosX = 37;
			for (var i = 0; i < 7; i++) {
				var rect = new RectangleF (startPosX, 20, 38, 30);
				var label = new UILabel (rect);
				label.BackgroundColor = colorFlag;
				if (colorFlag == UIColor.Yellow) {
					colorFlag = UIColor.Green;
				} else {
					colorFlag = UIColor.Yellow;
				}
				label.Font = UIFont.FromName ("Helvetica", 11f);
				label.Text = _dates [i];
				label.TextAlignment = UITextAlignment.Center;
				View.AddSubview (label);
				startPosX += 38;
			}
		}

		/// <summary>
		/// Draws the vertical bar containing the hours from 8:00 to 20:00.
		/// </summary>
		private void DrawLeftVerticalBar()
		{
			int startPosY = verticalIntervalStart;
			var hour = 8;
			for (var i = 0; i < 7; i++) {
				var rect = new RectangleF (1, startPosY, 34, 22);
				var label = new UILabel (rect);
				label.BackgroundColor = UIColor.Brown;
				label.Font = UIFont.FromName ("Helvetica", 13f);
				label.Text = hour.ToString() + ":00";
				label.TextAlignment = UITextAlignment.Center;
				View.AddSubview (label);

				startPosY += 2 * _pixelsPerHour;
				hour += 2;
			}
		}


		/// <summary>
		/// Creates and returns an instance of UIButton with certain attributes for custom menu purposes.
		/// </summary>
		/// <returns>The menu button.</returns>
		/// <param name="title">Button title..</param>
		/// <param name="rectangleFrame">Button's rectangle position frame.</param> 
		private UIButton CreateMenuButton(String title, float x, float y) {
			//TODO buttons in second view should have the same look.
			var rectangleFrame = new RectangleF (x, y, 90, 25);
			var button = UIButton.FromType (UIButtonType.System);
			button.Font = UIFont.FromName("Helvetica", 10f);
			button.BackgroundColor = UIColor.FromRGBA(161, 100, 50, 60);
			button.SetTitle (title, UIControlState.Normal);
			button.SetTitleColor (UIColor.Black, UIControlState.Normal);
			button.Frame = rectangleFrame;
			return button;
		}
		#endregion

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.FromRGB (223, 238, 206);

			// Perform any additional setup after loading the view, typically from a nib.

			InitDates ();

			DrawUpperHorizontalBar ();
			DrawLeftVerticalBar ();
			DrawInteractionButtons ();

			InitConferencesFromServer (null, null);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

