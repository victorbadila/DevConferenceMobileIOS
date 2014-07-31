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


			CancelButton.TouchUpInside += delegate {
				var board = this.Storyboard;
				var ctrl = (RectangleDrawViewController)board.InstantiateViewController ("RectangleDrawViewController");
				ctrl.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				this.PresentViewController (ctrl, true, null);
			};

			SaveButton.TouchUpInside += delegate {
				//SaveAction;
			};
			
		}

		public void SaveAction() {
			/**
			if (_event == null) 
			{
				MyRestClient.POST (
					new ConferenceEvent (TitleField.Text, OwnerField.Text, 
						DayField.Text, StartTimeField.Text, EndTimeField.Text));
			}
			*/


		}
	}
}

