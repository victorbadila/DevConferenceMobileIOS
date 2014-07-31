using System;
using System.Collections.Generic;
using System.Net.Http;

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

			request.AddHeader("Accept", "application/json");

			//TODO should handle async
			//TODO should handle server errors
			var response = Client.Execute (request);
			try {
				var resultList = JsonConvert.DeserializeObject<List<ConferenceEvent>>(response.Content);
				return resultList;
			} catch (Exception e) {
				//TODO print something like 'server may be down down'
				return null;
			}

		}

		/// <summary>
		/// This method creates a new conference.
		/// </summary>
		/// <param name="conf">The new conference.</param>
		public static void POST (ConferenceEvent conf)
		{
			var request = new RestRequest ("conferences.json", Method.POST);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json");
			request.RequestFormat = DataFormat.Json;


			//request.AddParameter ("conference", new { title = conf.Title, create_by = conf.CreateBy, start_date = conf.StartDate, end_date = conf.EndDate });

			request.AddBody(new {conference = conf});

//			request.AddParameter ("application/json", SerializeConfEvent (conf), ParameterType.RequestBody);


			//request.AddBody (stringJson);



			//request.AddBody (SerializeConfEvent (conf));

			var response = Client.Execute(request);
			HandleResponse (response);
		}


		/// <summary>
		/// This method updates the method with the id corresponding to the Id of the given argument.
		/// </summary>
		/// <param name="conf">The new values of the conference event.</param>
		public static void PUT (ConferenceEvent conf)
		{
			var request = new RestRequest ("conferences/" + conf.Id + ".json", Method.PUT);

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
			var request = new RestRequest ("conferences/" + id + ".json", Method.DELETE);
			request.AddHeader("Accepts", "application/json");
			var response = Client.Execute (request);
			// HandleResponse (response);

			// TODO see if succesful.
			return true;
		}

		/// <summary>
		/// This method checks response for error messages and if present throws an exception with the error text message.
		/// </summary>
		public static void HandleResponse(IRestResponse response)
		{
			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
				response.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
				response.StatusCode == System.Net.HttpStatusCode.NotFound) {
				var responseBody = JsonConvert.DeserializeObject <Dictionary<String, String>> (response.Content);
				throw new Exception(responseBody ["error"]);
			}
		}

	}
}

