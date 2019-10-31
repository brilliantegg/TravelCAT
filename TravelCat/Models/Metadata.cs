using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelCat.Models
{
    [MetadataType(typeof(Metadata_admin))]
    public partial class admin { }
    public class Metadata_admin
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(15, ErrorMessage = "最多15個字")]
        public string admin_account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(40, ErrorMessage = "最多40個字")]
        public string admin_password { get; set; }
    }

    [MetadataType(typeof(Metadata_member))]
    public partial class member { }
    public class Metadata_member
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(15, ErrorMessage = "最多15個字")]
        public string member_account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(40, ErrorMessage = "最多40個字")]
        public string member_password { get; set; }

        [DisplayName("會員狀態")]
        [DefaultValue(true)]
        public bool member_status { get; set; }
    }

    [MetadataType(typeof(Metadata_issue_type))]
    public partial class issue_type { }
    public class Metadata_issue_type
    {
        [DisplayName("問題名稱")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string issue_name { get; set; }
    }

    [MetadataType(typeof(Metadata_issue))]
    public partial class issue { }
    public class Metadata_issue
    {
        [DisplayName("會員編號")]
        public string member_id { get; set; }

        [DisplayName("管理員編號")]
        public int admin_id { get; set; }

        [DisplayName("問題編號")]
        public int issue_id { get; set; }

        [DisplayName("回報日期")]
        [Required(ErrorMessage = "此欄位為必填")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入日期錯誤")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime report_date { get; set; }

        [DisplayName("回報內容")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(1000, ErrorMessage = "最多1000個字")]
        public string issue_content { get; set; }

        [DisplayName("問題處理結果")]
        [StringLength(1000, ErrorMessage = "最多1000個字")]
        public string issue_result { get; set; }

        [DisplayName("問題狀態")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string issue_status { get; set; }

        [DisplayName("處理日期")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入日期錯誤")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> resolve_date { get; set; }

    }

    [MetadataType(typeof(Metadata_follow_list))]
    public partial class follow_list { }
    public class Metadata_follow_list
    {
        [DisplayName("會員編號")]
        public string member_id { get; set; }
        [DisplayName("被追蹤會員編號")]
        public string followed_id { get; set; }

        [DisplayName("追蹤日期")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入日期錯誤")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime follow_date { get; set; }
    }






    [MetadataType(typeof(Metadata_activity))]
    public partial class activity
    { }
    public class Metadata_activity
    {
        [DisplayName("活動編號")]
        [Key]
        [Required]
        [StringLength(7)]
        [RegularExpression("[A-Z][0-9]{6}", ErrorMessage = "格式有誤")]
        public string activity_id { get; set; }

        [DisplayName("標題")]
        [Required]
        [StringLength(30)]
        public string activity_title { get; set; }

        [DisplayName("電話")]
        [StringLength(20)]
        public string activity_tel { get; set; }

        [DisplayName("活動簡介")]
        [Column(TypeName = "ntext")]
        [Required]
        public string activity_intro { get; set; }

        [DisplayName("經度")]
        [Required]
        [StringLength(25)]
        public string longitude { get; set; }

        [DisplayName("緯度")]
        [Required]
        [StringLength(25)]
        public string latitude { get; set; }

        [DisplayName("所在城市")]
        [Required]
        [StringLength(10)]
        public string city { get; set; }

        [DisplayName("行政區")]
        [Required]
        [StringLength(10)]
        public string district { get; set; }

        [DisplayName("地址")]
        [Required]
        [StringLength(60)]
        public string address_detail { get; set; }

        [DisplayName("結束時間")]
        [Required]
        [StringLength(30)]
        public string end_date { get; set; }

        [DisplayName("開始時間")]
        [Required]
        [StringLength(30)]
        public string begin_date { get; set; }

        [DisplayName("主辦單位")]
        [StringLength(50)]
        public string organizer { get; set; }

        [DisplayName("活動網站")]
        [StringLength(150)]
        public string website { get; set; }

        [DisplayName("交通資訊")]
        [StringLength(200)]
        public string transport_info { get; set; }

        [DisplayName("頁面狀態")]
        public bool page_status { get; set; }
    }

    [MetadataType(typeof(Metadata_member_profile))]
    public partial class member_profile
    { }
    public class Metadata_member_profile
    {
        [DisplayName("會員編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string member_id { get; set; }
        [DisplayName("會員編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "此欄為最多20個字")]
        public string member_name { get; set; }
        [DisplayName("性別")]
        public Nullable<bool> gender { get; set; }
        [DisplayName("生日")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入日期格式有誤")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime birthday { get; set; }
        [DisplayName("暱稱")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "此欄為最多20個字")]
        public string nickname { get; set; }
        [DisplayName("加入日期")]
        public System.DateTime create_time { get; set; }
        [DisplayName("國家")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "此欄為最多30個字")]
        public string nation { get; set; }
        [DisplayName("詳細地址")]
        [StringLength(60, ErrorMessage = "此欄為最多60個字")]
        public string address_detail { get; set; }
        [DisplayName("城市")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "此欄為最多30個字")]
        public string city { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(60, ErrorMessage = "此欄為最多60個字")]
        public string email { get; set; }
        [DisplayName("行動電話")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "此欄為最多10個字")]
        public string phone { get; set; }
        [DisplayName("個人照片")]
        [StringLength(100, ErrorMessage = "此欄為最多100個字")]
        public string profile_photo { get; set; }
        [DisplayName("網頁連結")]
        [StringLength(150, ErrorMessage = "此欄為最多150個字")]
        public string website_link { get; set; }
        [DisplayName("個人簡介")]
        [StringLength(300, ErrorMessage = "此欄為最多300個字")]
        public string brief_intro { get; set; }
        [DisplayName("背景照片")]
        [StringLength(100, ErrorMessage = "此欄為最多100個字")]
        public string cover_photo { get; set; }
        [DisplayName("積分")]
        [Required(ErrorMessage = "此欄位為必填")]
        public Nullable<int> member_score { get; set; }
    }

    [MetadataType(typeof(Metadata_badge))]
    public partial class badge
    { }
    public class Metadata_badge
    {
        [DisplayName("徽章名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        [StringLength(15, ErrorMessage = "名稱最多15字")]
        public string badge_title { get; set; }

        [DisplayName("徽章圖片")]
        [Required(ErrorMessage = "請上傳圖片")]
        public string badge_photo { get; set; }

    }

    [MetadataType(typeof(Metadata_collections))]
    public partial class collections
    { }
    public class Metadata_collections
    {
        [DisplayName("收藏名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        public int collection_type_id { get; set; }

        [DisplayName("收藏類別")]
        [Required(ErrorMessage = "請輸入類別名稱")]
        [StringLength(20, ErrorMessage = "名稱最多20字")]
        public string collection_type_title { get; set; }
    }

    [MetadataType(typeof(Metadata_hotel))]
    public partial class hotel
    { }
    public class Metadata_hotel
    {
        [DisplayName("飯店編號")]
        [Required(ErrorMessage = "請輸入編號")]
        [StringLength(7, ErrorMessage = "編號最多7字")]
        public string hotel_id { get; set; }

        [DisplayName("飯店名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        [StringLength(30, ErrorMessage = "名稱最多30字")]
        public string hotel_title { get; set; }

        [DisplayName("電話")]
        [Required(ErrorMessage = "請輸入電話")]
        [StringLength(20, ErrorMessage = "名稱最多20字")]
        public string hotel_tel { get; set; }

        [DisplayName("經度")]
        [Required(ErrorMessage = "請輸入座標緯度")]
        [StringLength(25, ErrorMessage = "名稱最多25字")]
        public string longitude { get; set; }

        [DisplayName("緯度")]
        [Required(ErrorMessage = "請輸入座標經度")]
        [StringLength(25, ErrorMessage = "名稱最多25字")]
        public string latitude { get; set; }

        [DisplayName("飯店介紹")]
        public string hotel_intro { get; set; }

        [DisplayName("飯店網站")]
        [StringLength(150, ErrorMessage = "網址最多150字")]
        public string website { get; set; }

        [DisplayName("所在地縣市")]
        [Required(ErrorMessage = "請輸入所在地縣市")]
        [StringLength(10, ErrorMessage = "名稱最多10字")]
        public string city { get; set; }

        [DisplayName("區域名稱")]
        [Required(ErrorMessage = "請輸入所在地區域")]
        [StringLength(5, ErrorMessage = "名稱最多5字")]
        public string district { get; set; }

        [DisplayName("詳細地址")]
        [Required(ErrorMessage = "請輸入地址")]
        [StringLength(60, ErrorMessage = "名稱最多60字")]
        public string address_detail { get; set; }

        [DisplayName("頁面狀態")]
        [Required(ErrorMessage = "請輸入0或1")]
        public bool page_status { get; set; }
    }
    [MetadataType(typeof(Metadata_comment))]
    public partial class comment
    { }
    public class Metadata_comment
    {
        [DisplayName("評論編號")]
        [Key]
        public long comment_id { get; set; }
        [DisplayName("觀光編號")]
        [Required]
        [StringLength(7)]
        public string tourism_id { get; set; }
        [DisplayName("評論標題")]
        [Required]
        [StringLength(20)]
        public string comment_title { get; set; }
        [DisplayName("評論內容")]
        [Column(TypeName = "ntext")]
        [Required]
        public string comment_content { get; set; }
        [DisplayName("評論時間")]
        public DateTime comment_date { get; set; }
        [DisplayName("照片")]
        [Required]
        [StringLength(100)]
        public string comment_photo { get; set; }
        [DisplayName("停留時間")]
        public int comment_stay_total { get; set; }
        [DisplayName("旅伴")]
        [Required]
        [StringLength(10)]
        public string travel_partner { get; set; }
        [DisplayName("評分")]
        public short comment_rating { get; set; }
        [DisplayName("旅遊月份")]
        [Required]
        [StringLength(10)]
        public string travel_month { get; set; }
        [DisplayName("評論狀態")]
        public bool comment_status { get; set; }
        [DisplayName("會員編號")]
        [Required]
        [StringLength(7)]
        public string member_id { get; set; }


    }
    [MetadataType(typeof(Metadata_message))]
    public partial class message
    { }
    public class Metadata_message
    {

        [DisplayName("留言編號")]
        [Key]
        public long msg_id { get; set; }
        [DisplayName("留言時間")]
        public DateTime msg_time { get; set; }
        [DisplayName("留言內容")]
        [Required]
        [StringLength(1000)]
        public string msg_content { get; set; }
        [DisplayName("評論編號")]
        public long comment_id { get; set; }
        [DisplayName("會員編號")]
        [Required]
        [StringLength(7)]
        public string member_id { get; set; }


    }
    [MetadataType(typeof(Metadata_emoji))]
    public partial class emoji
    { }
    public partial class Metadata_emoji
    {
        [DisplayName("表情名稱")]
        [StringLength(10, ErrorMessage = "產品類別最多10個字")]
        public string emoji_title { get; set; }
        [DisplayName("表情圖案")]
        public string emoji_pic { get; set; }

    }
    [MetadataType(typeof(Metadata_restaurant))]
    public partial class restaurant
    { }
    public class Metadata_restaurant
    {
        [DisplayName("餐廳編號")]
        [Key]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(7, ErrorMessage = "餐廳編號最多7碼")]
        [RegularExpression("[A-Z][0-9]{6}", ErrorMessage = "格式有誤")]
        public string restaurant_id { get; set; }
        [DisplayName("名稱")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(30, ErrorMessage = "名稱最多30個字")]
        public string restaurant_title { get; set; }
        [DisplayName("電話")]
        [StringLength(20, ErrorMessage = "電話最多20碼")]
        public string restaurant_tel { get; set; }
        [DisplayName("簡介")]
        [Column(TypeName = "ntext")]
        [Required]
        public string restaurant_intro { get; set; }
        [DisplayName("經度")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(25, ErrorMessage = "經度最多25碼")]
        public string latitude { get; set; }
        [DisplayName("緯度")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(25, ErrorMessage = "緯度最多25碼")]
        public string longitude { get; set; }
        [DisplayName("城市")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(10, ErrorMessage = "字數最多10個字")]

        public string city { get; set; }
        [DisplayName("行政區")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(5, ErrorMessage = "最多5碼")]
        public string district { get; set; }
        [DisplayName("街道地址")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(60, ErrorMessage = "街道地址最多60個字")]
        public string address_detail { get; set; }
        [DisplayName("營業時間")]
        [StringLength(80, ErrorMessage = "營業時間最多80個字")]
        public string open_time { get; set; }
        [DisplayName("頁面狀態")]
        [Required(ErrorMessage = "不可空白")]
        public bool page_status { get; set; }
    }
    [MetadataType(typeof(Metadata_spot))]
    public partial class spot
    { }
    public class Metadata_spot
    {
        [DisplayName("景點編號")]
        [Required(ErrorMessage = "不可空白")]
        [Key]
        [StringLength(7, ErrorMessage = "餐廳編號最多7碼")]
        [RegularExpression("[A-Z][0-9]{6}", ErrorMessage = "格式有誤")]
        public string spot_id { get; set; }
        [DisplayName("名稱")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(30, ErrorMessage = "名稱最多30個字")]
        public string spot_title { get; set; }
        [DisplayName("電話")]
        [StringLength(20, ErrorMessage = "電話最多20碼")]
        public string spot_tel { get; set; }
        [DisplayName("簡介")]
        [Column(TypeName = "ntext")]
        [Required]

        public string spot_intro { get; set; }
        [DisplayName("經度")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(25, ErrorMessage = "經度最多25碼")]
        public string longitude { get; set; }
        [DisplayName("緯度")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(25, ErrorMessage = "緯度最多25碼")]
        public string latitude { get; set; }
        [DisplayName("備註")]

        public string addition_note { get; set; }
        [DisplayName("城市")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(10, ErrorMessage = "字數最多10個字")]
        public string city { get; set; }
        [DisplayName("行政區")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(5, ErrorMessage = "最多5碼")]
        public string district { get; set; }
        [DisplayName("街道地址")]
        [Required(ErrorMessage = "不可空白")]
        [StringLength(60, ErrorMessage = "街道地址最多60個字")]
        public string address_detail { get; set; }

        [DisplayName("營業時間")]
        [StringLength(80, ErrorMessage = "營業時間最多80個字")]

        public string open_time { get; set; }
        [DisplayName("門票資訊")]

        [StringLength(80, ErrorMessage = "門票資訊最多30個字")]
        public string ticket_info { get; set; }
        [DisplayName("更新時間")]
        public Nullable<System.DateTime> update_date { get; set; }

        [DisplayName("頁面狀態")]
        [Required(ErrorMessage = "不可空白")]
        public bool page_status { get; set; }


    }

























}