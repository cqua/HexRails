using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers {
	public class RailSplineExit {
		public bool IsDeadEnd;
		public RailSpline From, To;
		public bool FromAnIntersection, ToAnIntersection;

		public RailSplineExit() {
			IsDeadEnd = true;
		}

		public RailSplineExit(RailSpline _from, RailSpline _to, bool _fromInters, bool _toInters) {
			From = _from;
			To = _to;
			FromAnIntersection = _fromInters;
			ToAnIntersection = _toInters;
			IsDeadEnd = false;
		}
	}
}
