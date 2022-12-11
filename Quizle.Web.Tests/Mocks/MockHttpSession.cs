﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Web.UnitTests.Mocks
{
	public class MockHttpSession : ISession
	{
		Dictionary<string, object> sessionStorage = new Dictionary<string, object>();
		public object this[string name]
		{
			get { return sessionStorage[name]; }
			set { sessionStorage[name] = value; }
		}
		string ISession.Id
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		bool ISession.IsAvailable
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		IEnumerable<string> ISession.Keys
		{
			get { return sessionStorage.Keys; }
		}

		public Task CommitAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task LoadAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		void ISession.Clear()
		{
			sessionStorage.Clear();
		}

		
		void ISession.Remove(string key)
		{
			sessionStorage.Remove(key);
		}

		void ISession.Set(string key, byte[] value)
		{
			sessionStorage[key] = value;
		}

		bool ISession.TryGetValue(string key, out byte[] value)
		{
			if (sessionStorage[key] != null)
			{
				value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString() ?? string.Empty);
				if (value == Array.Empty<byte>())
				{
					return false;
				}
				else
				{
                    return true;

                }
            }
			else
			{
				value = Array.Empty<byte>();
				return false;
			}
		}
	}
}
