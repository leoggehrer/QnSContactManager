//@QnSCodeCopy
//MdStart
using System;
using System.Collections;
using System.Linq;

namespace QnSContactManager.Transfer
{
	public abstract partial class TransferModel
    {
		protected static bool IsEqualsWith(object obj1, object obj2)
		{
			bool result = false;

			if (obj1 == null && obj2 == null)
			{
				result = true;
			}
			else if (obj1 != null && obj2 != null)
			{
				if (obj1 is IEnumerable && obj2 is IEnumerable)
				{
					var enumerable1 = ((IEnumerable)obj1).Cast<object>().ToList();
					var enumerable2 = ((IEnumerable)obj2).Cast<object>().ToList();

					result = enumerable1.SequenceEqual(enumerable2);
				}
				else
				{
					result = obj1.Equals(obj2);
				}
			}
			return result;
		}
	}
}
//MdEnd
