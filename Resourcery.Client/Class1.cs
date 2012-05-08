using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Resourcery.Client
{


	public static class Remote
	{
		public static ResourceryClient Resource(string uri)
		{
			var client =  new ResourceryClient(new Uri(uri));

			client.Get();

			return client;
		}
	}

	public class ResourceryClient
	{
		readonly Uri uri;
		public ResourceryClient(Uri uri) { this.uri = uri; }


		public void Get()
		{
			WebRequestHandler handler;
			using(HttpClient client = new HttpClient(handler = new WebRequestHandler()))
			{
				client.GetAsync(uri).Wait();
			}
		}
	}
}
