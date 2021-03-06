//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoWork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Photographer:AuthenticatedUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Photographer()
        {
            this.Histories = new HashSet<History>();
            this.PhotographerSkills = new HashSet<PhotographerSkill>();
            this.Services = new HashSet<Service>();
        }

        public string Username { get; set; }
        public Nullable<int> TotalProjectDone { get; set; }
        [DisplayName("Bạn sẵn sàng để nhận dự án")]
        public Nullable<bool> isAvaiable { get; set; }
        [DisplayName("Những dự án bạn đã làm")]
        public string LinkProject { get; set; }
        [DisplayName("Mô tả bản thân")]
        public string Bio { get; set; }
        [DisplayName("Link mạng xã hội")]
        public string LinkSocialMedia { get; set; }
        public byte[] updateDate { get; set; }
        [DisplayName("Số tiền hiện tại")]
        public Nullable<decimal> CurrentMoney { get; set; }
        public string AdminID { get; set; }

        public virtual AuthenticatedUser AuthenticatedUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> Histories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhotographerSkill> PhotographerSkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
