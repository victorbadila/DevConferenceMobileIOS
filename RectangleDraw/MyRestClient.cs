using System;
using System.Collections.Generic;

using RestSharp;
using Newtonsoft.Json;

namespace RectangleDraw
{
	/// <summary>
	/// This class is a wrapper over the RestSharp RestClient class used to make requests and handle response errors.
	/// </summary>
	public static class MyRestClient
	{

		private static string TestUrl = "http://enigmatic-oasis-8124.herokuapp.com";

		private static RestClient Client;

		static MyRestClient ()
		{
			Client = new RestClient (TestUrl);
		}

		/// <summary>
		/// This method list all conferences.
		/// </summary>
		public static IList<ConferenceEvent> LIST ()
		{
			var request = new RestRequest("conferences.json", Method.GET);

			request.AddHeader("Accepts", "application/json");

			//TODO should handle async
			//TODO should handle server errors
			var response = Client.Execute (request);
			var resultList = JsonConvert.DeserializeObject<List<ConferenceEvent>>(response.Content);
			return resultList;
		}

		/// <summary>
		/// This method creates a new conference.
		/// </summary>
		/// <param name="conf">The new conference.</param>
		public static void POST (ConferenceEvent conf)
		{
			var request = new RestRequest ("conferenes.json", Method.POST);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accepts", "application/json");

			var stringJson = JsonConvert.SerializeObject (conf);

			request.AddBody (stringJson);

			var response = Client.Execute(request);
		}

		/// <summary>
		/// This method updates the method with the id corresponding to the Id of the given argument.
		/// </summary>
		/// <param name="conf">The new values of the conference event.</param>
		public static void PUT (ConferenceEvent conf)
		{
			var request = new RestRequest ("conferenes/" + conf.Id + ".json", Method.PUT);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accepts", "application/json");

			var stringJson = JsonConvert.SerializeObject (conf);

			request.AddBody (stringJson);

			var response = Client.Execute (request);
		}

		/// <summary>
		/// Deletes the conference event with the given id parameter.
		/// </summary>
		public static bool DELETE (int id)
		{
			var request = new RestRequest ("conferenes/" + id + ".json", Method.DELETE);

			request.AddHeader("Accepts", "application/json");

			var response = Client.Execute (request);

			// TODO see if succesful.

			return true;
		}


	}
}

