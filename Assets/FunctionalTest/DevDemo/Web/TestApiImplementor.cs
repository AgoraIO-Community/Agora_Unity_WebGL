using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace agora_gaming_rtc
{
	public class TestApiImplementor : IRtcEngineNative
	{

		// Use this for initialization
		void Start()
		{

		}

		public int call_setLogFilter(uint filter)
        {
			return setLogFilter(filter);
        }

	}

}
