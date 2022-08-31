using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers {
	public class RailExit {
		public bool IsDeadEnd;
		public Railway From, To;
		public bool FromAnIntersection, ToAnIntersection;

		public RailExit() {
			IsDeadEnd = true;
		}

		public RailExit(Railway _from, Railway _to, bool _fromInters, bool _toInters) {
			From = _from;
			To = _to;
			FromAnIntersection = _fromInters;
			ToAnIntersection = _toInters;
			IsDeadEnd = false;
		}
	}
}
