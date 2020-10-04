using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NotifyService.Models
{
    public partial class CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext : DbContext
    {
        public CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext()
        {
        }

        public CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext(DbContextOptions<CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Badge> Badge { get; set; }
        public virtual DbSet<BadgeDetails> BadgeDetails { get; set; }
        public virtual DbSet<CollectionType> CollectionType { get; set; }
        public virtual DbSet<CollectionsDetail> CollectionsDetail { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<CommentEmojiDetails> CommentEmojiDetails { get; set; }
        public virtual DbSet<Emoji> Emoji { get; set; }
        public virtual DbSet<FollowList> FollowList { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<IssueType> IssueType { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberProfile> MemberProfile { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageEmojiDetails> MessageEmojiDetails { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Spot> Spot { get; set; }
        public virtual DbSet<TourismPhoto> TourismPhoto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\audre\\Desktop\\TravelCAT\\TravelCat\\App_Data\\travel_cat_v1.mdf;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("activity_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("([dbo].[GetactivityId]())");

                entity.Property(e => e.ActivityIntro)
                    .HasColumnName("activity_intro")
                    .HasColumnType("ntext");

                entity.Property(e => e.ActivityTel)
                    .HasColumnName("activity_tel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityTitle)
                    .IsRequired()
                    .HasColumnName("activity_title")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.AddressDetail)
                    .HasColumnName("address_detail")
                    .HasMaxLength(100);

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(10);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(10);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Organizer)
                    .HasColumnName("organizer")
                    .HasMaxLength(50);

                entity.Property(e => e.PageStatus).HasColumnName("page_status");

                entity.Property(e => e.TransportInfo)
                    .HasColumnName("transport_info")
                    .HasMaxLength(1000);

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.AdminAccount)
                    .IsRequired()
                    .HasColumnName("admin_account")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.AdminEmail)
                    .IsRequired()
                    .HasColumnName("admin_email")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.AdminPassword)
                    .IsRequired()
                    .HasColumnName("admin_password")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirmed)
                    .HasColumnName("emailConfirmed")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.ToTable("badge");

                entity.Property(e => e.BadgeId).HasColumnName("badge_id");

                entity.Property(e => e.BadgePhoto)
                    .IsRequired()
                    .HasColumnName("badge_photo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BadgeTitle)
                    .IsRequired()
                    .HasColumnName("badge_title")
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<BadgeDetails>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberId, e.BadgeId })
                    .HasName("PK__badge_de__3EDDC8FA0D1FDA0D");

                entity.ToTable("badge_details");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BadgeId).HasColumnName("badge_id");

                entity.HasOne(d => d.Badge)
                    .WithMany(p => p.BadgeDetails)
                    .HasForeignKey(d => d.BadgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__badge_det__badge__5441852A");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.BadgeDetails)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__badge_det__membe__4CA06362");
            });

            modelBuilder.Entity<CollectionType>(entity =>
            {
                entity.ToTable("collection_type");

                entity.Property(e => e.CollectionTypeId)
                    .HasColumnName("collection_type_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CollectionTypeTitle)
                    .IsRequired()
                    .HasColumnName("collection_type_title")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CollectionsDetail>(entity =>
            {
                entity.HasKey(e => new { e.CollectionId, e.MemberId })
                    .HasName("PK__collecti__08FA1D994A84366E");

                entity.ToTable("collections_detail");

                entity.Property(e => e.CollectionId)
                    .HasColumnName("collection_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CollectionTypeId).HasColumnName("collection_type_id");

                entity.Property(e => e.Privacy).HasColumnName("privacy");

                entity.Property(e => e.TourismId)
                    .IsRequired()
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.CollectionType)
                    .WithMany(p => p.CollectionsDetail)
                    .HasForeignKey(d => d.CollectionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__collectio__colle__5535A963");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CollectionsDetail)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__collectio__membe__4E88ABD4");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentContent)
                    .IsRequired()
                    .HasColumnName("comment_content")
                    .HasColumnType("ntext");

                entity.Property(e => e.CommentDate)
                    .HasColumnName("comment_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CommentPhoto)
                    .HasColumnName("comment_photo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CommentRating).HasColumnName("comment_rating");

                entity.Property(e => e.CommentStatus).HasColumnName("comment_status");

                entity.Property(e => e.CommentStayTotal).HasColumnName("comment_stay_total");

                entity.Property(e => e.CommentTitle)
                    .IsRequired()
                    .HasColumnName("comment_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TourismId)
                    .IsRequired()
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TravelMonth)
                    .IsRequired()
                    .HasColumnName("travel_month")
                    .HasMaxLength(10);

                entity.Property(e => e.TravelPartner)
                    .IsRequired()
                    .HasColumnName("travel_partner")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment__member___48CFD27E");
            });

            modelBuilder.Entity<CommentEmojiDetails>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberId, e.CommentId })
                    .HasName("PK__comment___EFDDC51A81DEC4D1");

                entity.ToTable("comment_emoji_details");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.EmojiId).HasColumnName("emoji_id");

                entity.Property(e => e.TourismId)
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentEmojiDetails)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment_e__comme__5165187F");

                entity.HasOne(d => d.Emoji)
                    .WithMany(p => p.CommentEmojiDetails)
                    .HasForeignKey(d => d.EmojiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment_e__emoji__571DF1D5");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CommentEmojiDetails)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment_e__membe__4AB81AF0");
            });

            modelBuilder.Entity<Emoji>(entity =>
            {
                entity.ToTable("emoji");

                entity.Property(e => e.EmojiId)
                    .HasColumnName("emoji_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmojiPic)
                    .IsRequired()
                    .HasColumnName("emoji_pic")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmojiTitle)
                    .IsRequired()
                    .HasColumnName("emoji_title")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FollowList>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberId })
                    .HasName("PK__follow_l__693A506CC940328A");

                entity.ToTable("follow_list");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FollowDate)
                    .HasColumnName("follow_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FollowedId)
                    .IsRequired()
                    .HasColumnName("followed_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FollowList)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follow_li__membe__4F7CD00D");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("hotel");

                entity.Property(e => e.HotelId)
                    .HasColumnName("hotel_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("([dbo].[GethotelId]())");

                entity.Property(e => e.AddressDetail)
                    .HasColumnName("address_detail")
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(10);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(5);

                entity.Property(e => e.HotelIntro)
                    .HasColumnName("hotel_intro")
                    .HasColumnType("ntext");

                entity.Property(e => e.HotelTel)
                    .HasColumnName("hotel_tel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HotelTitle)
                    .IsRequired()
                    .HasColumnName("hotel_title")
                    .HasMaxLength(30);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PageStatus).HasColumnName("page_status");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberId, e.AdminId, e.IssueId })
                    .HasName("PK__issue__EAF49BA89E2A03D1");

                entity.ToTable("issue");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.IssueContent)
                    .IsRequired()
                    .HasColumnName("issue_content")
                    .HasMaxLength(1000);

                entity.Property(e => e.IssueResult)
                    .HasColumnName("issue_result")
                    .HasMaxLength(1000);

                entity.Property(e => e.IssueStatus).HasColumnName("issue_status");

                entity.Property(e => e.ProblemId)
                    .HasColumnName("problem_id")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ReportDate)
                    .HasColumnName("report_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResolveDate)
                    .HasColumnName("resolve_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__issue__admin_id__534D60F1");

                entity.HasOne(d => d.IssueNavigation)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__issue__issue_id__5812160E");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__issue__member_id__4D94879B");
            });

            modelBuilder.Entity<IssueType>(entity =>
            {
                entity.HasKey(e => e.IssueId)
                    .HasName("PK__issue_ty__D6185C399DAC0D66");

                entity.ToTable("issue_type");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.IssueName)
                    .IsRequired()
                    .HasColumnName("issue_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("([dbo].[GetmemberId]())");

                entity.Property(e => e.MemberAccount)
                    .IsRequired()
                    .HasColumnName("member_account")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MemberPassword)
                    .IsRequired()
                    .HasColumnName("member_password")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.MemberStatus).HasColumnName("member_status");
            });

            modelBuilder.Entity<MemberProfile>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__member_p__B29B85341BF7715D");

                entity.ToTable("member_profile");

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AddressDetail)
                    .HasColumnName("address_detail")
                    .HasMaxLength(60);

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.BriefIntro)
                    .HasColumnName("brief_intro")
                    .HasMaxLength(300);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(30);

                entity.Property(e => e.CoverPhoto)
                    .HasColumnName("cover_photo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirmed)
                    .HasColumnName("emailConfirmed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasColumnName("member_name")
                    .HasMaxLength(20);

                entity.Property(e => e.MemberScore).HasColumnName("member_score");

                entity.Property(e => e.Nation)
                    .IsRequired()
                    .HasColumnName("nation")
                    .HasMaxLength(30);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasColumnName("nickname")
                    .HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProfilePhoto)
                    .HasColumnName("profile_photo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WebsiteLink)
                    .HasColumnName("website_link")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithOne(p => p.MemberProfile)
                    .HasForeignKey<MemberProfile>(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__member_pr__membe__47DBAE45");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MsgId)
                    .HasName("PK__message__9CA9728D96A610CF");

                entity.ToTable("message");

                entity.Property(e => e.MsgId).HasColumnName("msg_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MsgContent)
                    .IsRequired()
                    .HasColumnName("msg_content")
                    .HasMaxLength(1000);

                entity.Property(e => e.MsgStatus).HasColumnName("msg_status");

                entity.Property(e => e.MsgTime)
                    .HasColumnName("msg_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.TourismId)
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__message__comment__5070F446");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__message__member___4BAC3F29");
            });

            modelBuilder.Entity<MessageEmojiDetails>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberId, e.MsgId })
                    .HasName("PK__message___E5A6F91EFD15D3FA");

                entity.ToTable("message_emoji_details");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MsgId).HasColumnName("msg_id");

                entity.Property(e => e.EmojiId).HasColumnName("emoji_id");

                entity.Property(e => e.TourismId)
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Emoji)
                    .WithMany(p => p.MessageEmojiDetails)
                    .HasForeignKey(d => d.EmojiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__message_e__emoji__5629CD9C");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MessageEmojiDetails)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__message_e__membe__49C3F6B7");

                entity.HasOne(d => d.Msg)
                    .WithMany(p => p.MessageEmojiDetails)
                    .HasForeignKey(d => d.MsgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__message_e__msg_i__52593CB8");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("restaurant");

                entity.Property(e => e.RestaurantId)
                    .HasColumnName("restaurant_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("([dbo].[GetrestId]())");

                entity.Property(e => e.AddressDetail)
                    .HasColumnName("address_detail")
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(10);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(5);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OpenTime)
                    .HasColumnName("open_time")
                    .HasMaxLength(150);

                entity.Property(e => e.PageStatus).HasColumnName("page_status");

                entity.Property(e => e.RestaurantIntro)
                    .HasColumnName("restaurant_intro")
                    .HasColumnType("ntext");

                entity.Property(e => e.RestaurantTel)
                    .HasColumnName("restaurant_tel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RestaurantTitle)
                    .IsRequired()
                    .HasColumnName("restaurant_title")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Spot>(entity =>
            {
                entity.ToTable("spot");

                entity.Property(e => e.SpotId)
                    .HasColumnName("spot_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("([dbo].[GetspotId]())");

                entity.Property(e => e.AdditionNote)
                    .HasColumnName("addition_note")
                    .HasMaxLength(250);

                entity.Property(e => e.AddressDetail)
                    .HasColumnName("address_detail")
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(10);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(5);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OpenTime)
                    .HasColumnName("open_time")
                    .HasMaxLength(500);

                entity.Property(e => e.PageStatus).HasColumnName("page_status");

                entity.Property(e => e.SpotIntro)
                    .HasColumnName("spot_intro")
                    .HasColumnType("ntext");

                entity.Property(e => e.SpotTel)
                    .HasColumnName("spot_tel")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SpotTitle)
                    .IsRequired()
                    .HasColumnName("spot_title")
                    .HasMaxLength(100);

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasMaxLength(150);

                entity.Property(e => e.TravellingInfo)
                    .HasColumnName("travelling_info")
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TourismPhoto>(entity =>
            {
                entity.ToTable("tourism_photo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TourismId)
                    .IsRequired()
                    .HasColumnName("tourism_id")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TourismPhoto1)
                    .HasColumnName("tourism_photo")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
