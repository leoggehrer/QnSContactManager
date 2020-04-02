//@QnSCodeCopy
//MdStart

using System.ComponentModel.DataAnnotations;

namespace QnSContactManager.AspMvc.Models
{
	public partial class IdentityModel : ModelObject, Contracts.IIdentifiable
    {
		private System.Int32 _id;
		public virtual System.Int32 Id
		{
			get
			{
				OnIdReading();
				return _id;
			}
			set
			{
				bool handled = false;
				OnIdChanging(ref handled, ref _id);
				if (handled == false)
				{
					this._id = value;
				}
				OnIdChanged();
			}
		}
		partial void OnIdReading();
		partial void OnIdChanging(ref bool handled, ref System.Int32 _id);
		partial void OnIdChanged();

		private byte[] _timestamp;
		[ScaffoldColumn(false)]
		public virtual byte[] Timestamp
		{
			get
			{
				OnTimestampReading();
				return _timestamp;
			}
			set
			{
				bool handled = false;
				OnTimestampChanging(ref handled, ref _timestamp);
				if (handled == false)
				{
					this._timestamp = value;
				}
				OnTimestampChanged();
			}
		}
		partial void OnTimestampReading();
		partial void OnTimestampChanging(ref bool handled, ref byte[] _timestamp);
		partial void OnTimestampChanged();

	}
}
//MdEnd
