using System;
using System.Drawing;
using System.Collections.Generic;

using RestSharp;
using Newtonsoft.Json;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RectangleDraw
{
	public partial class SecondViewController : UIViewController
	{

		public ConferenceEvent _event;

		public SecondViewController () : base ("SecondViewController", null) { }

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

			// Perform any additional setup after loading the view, typically from a nib.

			View.BackgroundColor = UIColor.FromRGB (223, 238, 206);

			foreach (var field in new List<UITextField>()
				{ 
					TitleField,
					OwnerField, 
					DurationFieldHours,
					DurationFieldMinutes
					//DayField,
					//StartTimeField,
					//EndTimeField
				}) 
			{
				field.ShouldReturn += ((textField) => {
					textField.ResignFirstResponder ();
					return true;
				});
			};

			var datePicker = DatePicker;
			datePicker.Mode = UIDatePickerMode.DateAndTime;
			datePicker.MinimumDate = DateTime.Today;
			datePicker.MaximumDate = DateTime.Today.AddDays (6);
			datePicker.Hidden = false;

			if (_event != null) {
				TitleField.Text = _event.title;
				OwnerField.Text = _event.create_by;

				datePicker.Date = DateTime.SpecifyKind (_event.start_date, DateTimeKind.Utc);
				var duration = (_event.end_date - _event.start_date);
				DurationFieldHours.Text = duration.Hours.ToString ();
				DurationFieldMinutes.Text = duration.Minutes.ToString ();
			}

			CancelButton.TouchUpInside += JumpToPrimaryView;
			SaveButton.TouchUpInside += SaveAction;
			
		}

		public void JumpToPrimaryView(object sender, EventArgs ea) 
		{
			var board = this.Storyboard;
			var ctrl = (RectangleDrawViewController)board.InstantiateViewController ("RectangleDrawViewController");
			ctrl.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
			this.PresentViewController (ctrl, true, null);
		}

		public void SaveAction(object sender, EventArgs ea) 
		{
			// TODO had to add additional hours. could not find the cause of the misconversions. possibly to do with server time
			var startDate = DateTime.SpecifyKind (DatePicker.Date, DateTimeKind.Unspecified).AddHours(6);

			var endDate = startDate;
			endDate = endDate.AddHours(Convert.ToInt16(DurationFieldHours.Text));
			endDate = endDate.AddMinutes (Convert.ToInt16 (DurationFieldMinutes.Text));

			var conferenceEvent = new ConferenceEvent(TitleField.Text, OwnerField.Text, startDate, endDate);
			try {
				if (_event != null) {
					conferenceEvent.Id = _event.Id;
					MyRestClient.PUT (conferenceEvent);
				} else {
					MyRestClient.POST (conferenceEvent);
				}
				JumpToPrimaryView(null, null);
			} catch (Exception e) {
				ShowErrorAlert (e.Message);
			}
		}

		/// <summary>
		/// This method draws an error alert on the screen.
		/// </summary>
		/// <param name="message">The error message.</param>
		public void ShowErrorAlert(String message) 
		{
			UIAlertView alert = new UIAlertView () {
				Title = "Error processing the request",
				Message = message
			};
			alert.AddButton ("Ok");
			alert.Clicked += (sender, e) => {
				alert.DismissWithClickedButtonIndex(0, true);
			};
			alert.Show();
		}
	}
}

