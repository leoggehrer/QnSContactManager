namespace QnSContactManager.Logic.DataContext.Db
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	partial class QnSContactManagerDbContext : GenericDbContext
	{
		protected DbSet<Entities.Persistence.App.Contact> ContactSet
		{
			get;
			set;
		}
		protected DbSet<Entities.Persistence.Account.ActionLog> ActionLogSet
		{
			get;
			set;
		}
		protected DbSet<Entities.Persistence.Account.Identity> IdentitySet
		{
			get;
			set;
		}
		protected DbSet<Entities.Persistence.Account.IdentityXRole> IdentityXRoleSet
		{
			get;
			set;
		}
		protected DbSet<Entities.Persistence.Account.LoginSession> LoginSessionSet
		{
			get;
			set;
		}
		protected DbSet<Entities.Persistence.Account.Role> RoleSet
		{
			get;
			set;
		}
		public override DbSet<E> Set<I, E>()
		{
			DbSet<E> result = null;
			if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.App.IContact))
			{
				result = ContactSet as DbSet<E>;
			}
			else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IActionLog))
			{
				result = ActionLogSet as DbSet<E>;
			}
			else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IIdentity))
			{
				result = IdentitySet as DbSet<E>;
			}
			else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IIdentityXRole))
			{
				result = IdentityXRoleSet as DbSet<E>;
			}
			else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.ILoginSession))
			{
				result = LoginSessionSet as DbSet<E>;
			}
			else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IRole))
			{
				result = RoleSet as DbSet<E>;
			}
			return result;
		}
		partial void DoModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Entities.Persistence.App.Contact>().ToTable(nameof(Entities.Persistence.App.Contact), nameof(Entities.Persistence.App)).HasKey(nameof(Entities.Persistence.App.Contact.Id));
			modelBuilder.Entity<Entities.Persistence.App.Contact>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.App.Contact>());
			modelBuilder.Entity<Entities.Persistence.Account.ActionLog>().ToTable(nameof(Entities.Persistence.Account.ActionLog), nameof(Entities.Persistence.Account)).HasKey(nameof(Entities.Persistence.Account.ActionLog.Id));
			modelBuilder.Entity<Entities.Persistence.Account.ActionLog>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.Account.ActionLog>());
			modelBuilder.Entity<Entities.Persistence.Account.Identity>().ToTable(nameof(Entities.Persistence.Account.Identity), nameof(Entities.Persistence.Account)).HasKey(nameof(Entities.Persistence.Account.Identity.Id));
			modelBuilder.Entity<Entities.Persistence.Account.Identity>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.Account.Identity>());
			modelBuilder.Entity<Entities.Persistence.Account.IdentityXRole>().ToTable(nameof(Entities.Persistence.Account.IdentityXRole), nameof(Entities.Persistence.Account)).HasKey(nameof(Entities.Persistence.Account.IdentityXRole.Id));
			modelBuilder.Entity<Entities.Persistence.Account.IdentityXRole>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.Account.IdentityXRole>());
			modelBuilder.Entity<Entities.Persistence.Account.LoginSession>().ToTable(nameof(Entities.Persistence.Account.LoginSession), nameof(Entities.Persistence.Account)).HasKey(nameof(Entities.Persistence.Account.LoginSession.Id));
			modelBuilder.Entity<Entities.Persistence.Account.LoginSession>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.Account.LoginSession>());
			modelBuilder.Entity<Entities.Persistence.Account.Role>().ToTable(nameof(Entities.Persistence.Account.Role), nameof(Entities.Persistence.Account)).HasKey(nameof(Entities.Persistence.Account.Role.Id));
			modelBuilder.Entity<Entities.Persistence.Account.Role>().Property(p => p.Timestamp).IsRowVersion();
			ConfigureEntityType(modelBuilder.Entity<Entities.Persistence.Account.Role>());
		}
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.App.Contact> entityTypeBuilder);
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.Account.ActionLog> entityTypeBuilder);
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.Account.Identity> entityTypeBuilder);
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.Account.IdentityXRole> entityTypeBuilder);
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.Account.LoginSession> entityTypeBuilder);
		partial void ConfigureEntityType(EntityTypeBuilder<Entities.Persistence.Account.Role> entityTypeBuilder);
	}
}
