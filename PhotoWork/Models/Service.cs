﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            this.Invoices = new HashSet<Invoice>();
            this.MediaFiles = new HashSet<MediaFile>();
            this.Skills = new HashSet<Skill>();
        }
    
        public string ID { get; set; }
        [Required(ErrorMessage = "Hãy nhập tên dịch vụ")]
        [Display(Name = "Tên dịch vụ")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Hãy nhập mô tả")]
        [MinLength(10, ErrorMessage = "Mô tả ít nhất 10 kí tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public Nullable<bool> isAvaiable { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<System.DateTime> deleteDate { get; set; }
        public string PhotographerID { get; set; }
        public Nullable<double> Rating { get; set; }
        //Extra 
        public string FullName { get; set; }
        [Required(ErrorMessage = "Hãy nhập giá cơ bản")]
        [Display(Name = "Giá cơ bản")]
        public Decimal StartingPrice { get; set; }
        public string photographerID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
        public virtual Photographer Photographer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
