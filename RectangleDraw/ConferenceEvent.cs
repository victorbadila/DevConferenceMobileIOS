using System;

namespace RectangleDraw
{
	public class ConferenceEvent
	{

		public int Id { get; set;}

		public String Title { get; set; }

		public String CreateBy { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public ConferenceEvent (String title, String createBy, DateTime startDate, DateTime endDate)
		{
			Title = title;
			CreateBy = createBy;
			StartDate = startDate;
			EndDate = endDate;
		}

		public ConferenceEvent() { }

	}
}

