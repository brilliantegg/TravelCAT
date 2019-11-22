namespace TravelCat.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbTravelCat : DbContext
    {
        public dbTravelCat()
            : base("name=dbTravelCat")
        {
        }

        public virtual DbSet<activity> activities { get; set; }
        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<badge> badges { get; set; }
        public virtual DbSet<badge_details> badge_details { get; set; }
        public virtual DbSet<collection_type> collection_type { get; set; }
        public virtual DbSet<collections_detail> collections_detail { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<comment_emoji_details> comment_emoji_details { get; set; }
        public virtual DbSet<emoji> emojis { get; set; }
        public virtual DbSet<follow_list> follow_list { get; set; }
        public virtual DbSet<hotel> hotels { get; set; }
        public virtual DbSet<issue> issues { get; set; }
        public virtual DbSet<issue_type> issue_type { get; set; }
        public virtual DbSet<member> members { get; set; }
        public virtual DbSet<member_profile> member_profile { get; set; }
        public virtual DbSet<message> messages { get; set; }
        public virtual DbSet<message_emoji_details> message_emoji_details { get; set; }
        public virtual DbSet<restaurant> restaurants { get; set; }
        public virtual DbSet<spot> spots { get; set; }
        public virtual DbSet<tourism_photo> tourism_photo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<activity>()
                .Property(e => e.activity_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.activity_title)
                .IsFixedLength();

            modelBuilder.Entity<activity>()
                .Property(e => e.activity_tel)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.end_date)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.begin_date)
                .IsUnicode(false);

            modelBuilder.Entity<activity>()
                .Property(e => e.website)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.admin_account)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.admin_password)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .HasMany(e => e.issues)
                .WithRequired(e => e.admin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<badge>()
                .Property(e => e.badge_photo)
                .IsUnicode(false);

            modelBuilder.Entity<badge>()
                .HasMany(e => e.badge_details)
                .WithRequired(e => e.badge)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<badge_details>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<collection_type>()
                .Property(e => e.collection_type_title)
                .IsUnicode(false);

            modelBuilder.Entity<collection_type>()
                .HasMany(e => e.collections_detail)
                .WithRequired(e => e.collection_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<collections_detail>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<collections_detail>()
                .Property(e => e.tourism_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.tourism_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.comment_title)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.comment_photo)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .HasMany(e => e.comment_emoji_details)
                .WithRequired(e => e.comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<comment>()
                .HasMany(e => e.messages)
                .WithRequired(e => e.comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<comment_emoji_details>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<emoji>()
                .Property(e => e.emoji_pic)
                .IsUnicode(false);

            modelBuilder.Entity<emoji>()
                .HasMany(e => e.comment_emoji_details)
                .WithRequired(e => e.emoji)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<emoji>()
                .HasMany(e => e.message_emoji_details)
                .WithRequired(e => e.emoji)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<follow_list>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<follow_list>()
                .Property(e => e.followed_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<hotel>()
                .Property(e => e.hotel_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<hotel>()
                .Property(e => e.hotel_tel)
                .IsUnicode(false);

            modelBuilder.Entity<hotel>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<hotel>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<hotel>()
                .Property(e => e.website)
                .IsUnicode(false);

            modelBuilder.Entity<issue>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<issue>()
                .Property(e => e.issue_status)
                .IsFixedLength();

            modelBuilder.Entity<issue_type>()
                .Property(e => e.issue_name)
                .IsUnicode(false);

            modelBuilder.Entity<issue_type>()
                .HasMany(e => e.issues)
                .WithRequired(e => e.issue_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<member>()
                .Property(e => e.member_account)
                .IsUnicode(false);

            modelBuilder.Entity<member>()
                .Property(e => e.member_password)
                .IsUnicode(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.badge_details)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.collections_detail)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.comment_emoji_details)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.follow_list)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.issues)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.member_profile)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.messages)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member>()
                .HasMany(e => e.message_emoji_details)
                .WithRequired(e => e.member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.profile_photo)
                .IsUnicode(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.website_link)
                .IsUnicode(false);

            modelBuilder.Entity<member_profile>()
                .Property(e => e.cover_photo)
                .IsUnicode(false);

            modelBuilder.Entity<message>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<message>()
                .HasMany(e => e.message_emoji_details)
                .WithRequired(e => e.message)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<message_emoji_details>()
                .Property(e => e.member_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<restaurant>()
                .Property(e => e.restaurant_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<restaurant>()
                .Property(e => e.restaurant_title)
                .IsFixedLength();

            modelBuilder.Entity<restaurant>()
                .Property(e => e.restaurant_tel)
                .IsUnicode(false);

            modelBuilder.Entity<restaurant>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<restaurant>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<spot>()
                .Property(e => e.spot_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<spot>()
                .Property(e => e.spot_tel)
                .IsUnicode(false);

            modelBuilder.Entity<spot>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<spot>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<tourism_photo>()
                .Property(e => e.tourism_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tourism_photo>()
                .Property(e => e.tourism_photo1)
                .IsUnicode(false);
        }
    }
}
