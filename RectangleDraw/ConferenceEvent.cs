using System;

using Newtonsoft.Json;

namespace RectangleDraw
{
	public class ConferenceEvent
	{
		[JsonProperty("id")]
		public int Id { get; set;}

		[JsonProperty("title")]
		public String Title { get; set; }

		[JsonProperty("create_by")]
		public String CreateBy { get; set; }

		[JsonProperty("start_date")]
		public DateTime StartDate { get; set; }

		[JsonProperty("end_date")]
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

