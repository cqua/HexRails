namespace Helpers {
	public class RailExit {
		public bool IsDeadEnd;
		public RailController From, To;
		public bool FromAnIntersection, ToAnIntersection;

		public RailExit() {
			IsDeadEnd = true;
		}

		public RailExit(RailController _from, RailController _to, bool _fromInters, bool _toInters) {
			From = _from;
			To = _to;
			FromAnIntersection = _fromInters;
			ToAnIntersection = _toInters;
			IsDeadEnd = false;
		}
	}
}
