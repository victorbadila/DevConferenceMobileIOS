﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using RestSharp;

using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace RectangleDraw
{
	public partial class RectangleDrawViewController : UIViewController
	{

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

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.

			InitDates ();
			InitUpperHorizontalBar ();
			InitLeftVerticalBar ();

			CreateHttpTestContent ();
		}

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

		private void InitUpperHorizontalBar()
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

		private void InitLeftVerticalBar()
		{
			var startPosY = verticalIntervalStart;
			var hour = 8;
			for (var i = 0; i < 7; i++) {
				var rect = new RectangleF (1, startPosY, 34, 22);
				var label = new UILabel (rect);
				label.BackgroundColor = UIColor.Brown;
				label.Font = UIFont.FromName ("Helvetica", 13f);
				label.Text = hour.ToString() + ":00";
				label.TextAlignment = UITextAlignment.Center;
				View.AddSubview (label);

				startPosY += 50;
				hour += 2;
			}
		}

		private void CreateHttpTestContent() 
		{
			var rect = new RectangleF(100, 400, 140, 50);
			var button = UIButton.FromType(UIButtonType.System);

			button.Font = UIFont.FromName("Helvetica", 13f);
			button.SetTitle ("GET Conferences", UIControlState.Normal);
			button.Frame = rect;
			button.TouchUpInside += delegate {

				var client = new RestClient(testUrl);
				var request = new RestRequest("conferences.json", Method.GET);

				request.AddHeader("Accepts", "application/json");

				//TODO should handle async
				//TODO should handle server errors
				var resultList = client.Execute<List<ConferenceEvent>>(request);
				_conferenceEvents = resultList.Data;
				PopulateViewWithConferenceEvents();
			};

			View.AddSubview (button);
		}

		public void PopulateViewWithConferenceEvents() 
		{
			foreach (var confButton in _conferenceEventsGraphicElements) 
			{
				confButton.RemoveFromSuperview ();
				// TODO dealloc?
			}
			_conferenceEventsGraphicElements.Clear ();
			foreach (var conf in _conferenceEvents)
			{
				var button = GenerateButtonFromConferenceEvent (conf);
				_conferenceEventsGraphicElements.Add (button);
				View.AddSubview (button);
			}
		}

		public ConferenceEventButton GenerateButtonFromConferenceEvent(ConferenceEvent conf)
		{
			var day = conf.StartDate.DayOfYear - DateTime.Now.DayOfYear;
			if (day < 0) 
			{
				//TODO handle exception.
				throw new Exception ("conference no longer belongs in database");
			}
			var xCoord = day * 38 + 37;

			var yCoord = Convert.ToInt16(timeIntervalToPixel(conf.StartDate, conf.EndDate) + verticalIntervalStart);

			var rect = new RectangleF(xCoord, yCoord, 38, 20);
			var button = new ConferenceEventButton ();
				//UIButton.FromType(UIButtonType.System);

			button.SetTitleColor (UIColor.Black, UIControlState.Normal);
			button.Font = UIFont.FromName("Helvetica", 9f);
			button.SetTitle (conf.CreateBy, UIControlState.Normal);
			button.Frame = rect;
			button.BackgroundColor = UIColor.FromRGB(204, 255, 255);
			return button;
		}

		public int timeIntervalToPixel(DateTime startDate, DateTime endDate)
		{
			var hours = endDate.Hour - startDate.Hour;
			var minutes = endDate.Minute - startDate.Minute;
			//TODO plm..
			return hours * 25 + (minutes * 25 / 60) + (startDate.Hour - 8) *  25 + startDate.Minute * 25 / 60;
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
