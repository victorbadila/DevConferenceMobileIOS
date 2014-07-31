using System;

using Newtonsoft.Json;

namespace RectangleDraw
{
	public class ConferenceEvent
	{
		[JsonProperty("id")]
		public int Id { get; set;}

		[JsonProperty("title")]
		public String title { get; set; }

		[JsonProperty("create_by")]
		public String create_by { get; set; }

		[JsonProperty("start_date")]
		public DateTime start_date { get; set; }

		[JsonProperty("end_date")]
		public DateTime end_date { get; set; }

		public ConferenceEvent (String title, String createBy, DateTime startDate, DateTime endDate)
		{
			title = title;
			create_by = createBy;
			start_date = startDate;
			end_date = endDate;
		}

		public ConferenceEvent() { }

	}
}

