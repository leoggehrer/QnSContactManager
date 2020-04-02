namespace QnSContactManager.Logic.Entities.Business.Account
{
	using System;
	partial class AppAccess : QnSContactManager.Contracts.Business.Account.IAppAccess
	{
		static AppAccess()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		public AppAccess()
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		public QnSContactManager.Contracts.Persistence.Account.IIdentity Identity
		{
			get
			{
				OnIdentityReading();
				return _identity;
			}
			set
			{
				bool handled = false;
				OnIdentityChanging(ref handled, ref _identity);
				if (handled == false)
				{
					this._identity = value;
				}
				OnIdentityChanged();
			}
		}
		private QnSContactManager.Contracts.Persistence.Account.IIdentity _identity;
		partial void OnIdentityReading();
		partial void OnIdentityChanging(ref bool handled, ref QnSContactManager.Contracts.Persistence.Account.IIdentity _identity);
		partial void OnIdentityChanged();
		public System.Collections.Generic.IEnumerable<QnSContactManager.Contracts.Persistence.Account.IRole> Roles
		{
			get
			{
				OnRolesReading();
				return _roles;
			}
			set
			{
				bool handled = false;
				OnRolesChanging(ref handled, ref _roles);
				if (handled == false)
				{
					this._roles = value;
				}
				OnRolesChanged();
			}
		}
		private System.Collections.Generic.IEnumerable<QnSContactManager.Contracts.Persistence.Account.IRole> _roles;
		partial void OnRolesReading();
		partial void OnRolesChanging(ref bool handled, ref System.Collections.Generic.IEnumerable<QnSContactManager.Contracts.Persistence.Account.IRole> _roles);
		partial void OnRolesChanged();
		public void CopyProperties(QnSContactManager.Contracts.Business.Account.IAppAccess other)
		{
			if (other == null)
			{
				throw new System.ArgumentNullException(nameof(other));
			}
			bool handled = false;
			BeforeCopyProperties(other, ref handled);
			if (handled == false)
			{
				Id = other.Id;
				Timestamp = other.Timestamp;
				Identity = other.Identity;
				Roles = other.Roles;
			}
			AfterCopyProperties(other);
		}
		partial void BeforeCopyProperties(QnSContactManager.Contracts.Business.Account.IAppAccess other, ref bool handled);
		partial void AfterCopyProperties(QnSContactManager.Contracts.Business.Account.IAppAccess other);
		public override bool Equals(object obj)
		{
			if (!(obj is QnSContactManager.Contracts.Business.Account.IAppAccess instance))
			{
				return false;
			}
			return base.Equals(instance) && Equals(instance);
		}
		protected bool Equals(QnSContactManager.Contracts.Business.Account.IAppAccess other)
		{
			if (other == null)
			{
				return false;
			}
			return Id == other.Id && IsEqualsWith(Timestamp, other.Timestamp) && IsEqualsWith(Identity, other.Identity) && IsEqualsWith(Roles, other.Roles);
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Timestamp, Identity, Roles);
		}
	}
}
namespace QnSContactManager.Logic.Entities.Business.Account
{
	partial class AppAccess : IdentityObject
	{
	}
}
