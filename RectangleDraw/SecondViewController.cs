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

			// Perform any additional setup after loading the view, typically from a nib.

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
				TitleField.Text = _event.Title;
				OwnerField.Text = _event.CreateBy;

				datePicker.Date = DateTime.SpecifyKind (_event.StartDate, DateTimeKind.Utc);
				var duration = (_event.EndDate - _event.StartDate);
				DurationFieldHours.Text = duration.Hours.ToString ();
				DurationFieldMinutes.Text = duration.Minutes.ToString ();
			}


			CancelButton.TouchUpInside += delegate {
				var board = this.Storyboard;
				var ctrl = (RectangleDrawViewController)board.InstantiateViewController ("RectangleDrawViewController");
				ctrl.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				this.PresentViewController (ctrl, true, null);
			};

			SaveButton.TouchUpInside += SaveAction;
			
		}

		public void SaveAction(object sender, EventArgs ea) {
			// had to add additional hours. could not find the cause of the misconversion.
			var startDate = DateTime.SpecifyKind (DatePicker.Date, DateTimeKind.Unspecified).AddHours(3);

			var endDate = startDate;
			endDate = endDate.AddHours(Convert.ToInt16(DurationFieldHours.Text));
			endDate = endDate.AddMinutes (Convert.ToInt16 (DurationFieldMinutes.Text));

			var conferenceEvent = new ConferenceEvent(TitleField.Text, OwnerField.Text, startDate, endDate);

		}
	}
}

