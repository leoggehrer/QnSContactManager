//@QnSCodeCopy
//MdStart
using System;
using System.Collections;
using System.Linq;
using CommonBase.Extensions;

namespace QnSContactManager.Logic.Entities.Modules
{
	internal partial class ModuleObject
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
		public void CopyProperties(Object other)
		{
			other.CheckArgument(nameof(other));

			bool handled = false;
			BeforeCopyProperties(other, ref handled);
			if (handled == false)
			{
			}
			AfterCopyProperties(other);
		}
		partial void BeforeCopyProperties(Object other, ref bool handled);
		partial void AfterCopyProperties(Object other);
	}
}
//MdEnd
