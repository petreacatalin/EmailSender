using EmailSender.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmailSender.Data.EmailContext
{
	public class EmailDbContext : DbContext
	{
		public EmailDbContext(DbContextOptions<EmailDbContext> options)
		: base(options)
		{
		}

		public DbSet<EmailCommand> EmailCommands { get; set; }
		public DbSet<EmailCount> EmailCounts { get; set; }
		public DbSet<EmailLog> EmailLogs { get; set; }
		public DbSet<CampaignCommand> CampaignCommands { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EmailCommand>()
				.HasOne(e => e.EmailCount)
				.WithOne()
				.HasForeignKey<EmailCommand>(e => e.EmailCountId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<EmailCommand>()
				.HasOne(el => el.EmailLog)
				.WithOne(ec => ec.EmailCommand)
				.HasForeignKey<EmailLog>(rc => rc.EmailCommandId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<CampaignCommand>()
				.HasOne(rc => rc.EmailLog)
				.WithOne(el => el.CampaignCommand)
				.HasForeignKey<EmailLog>(el => el.CampaignCommandId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<EmailCommand>()
				.HasOne(ec => ec.CampaignCommand)
				.WithMany(rc => rc.EmailCommands)
				.HasForeignKey(ec => ec.CampaignCommandId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CampaignCommand>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();
		}
}
	
}
