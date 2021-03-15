using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoWork.DTO
{
    public class PhotographerProfile
    {
        public string Username { get; set; }
        public string phoneNumber { get; set; }
        public string FullName { get; set; }
        public int TotalProjectDone { get; set; }
        public string Bio { get; set; }
        public string LinkProject { get; set; }
        public string LinkSocialMedia { get; set; }
        public float CurrentMoney { get; set; }
        public PhotographerProfile(string Username,string phoneNumber,string FullName, int TotalProjectDone, string Bio, string LinkProject, string LinkSocialMedia, float CurrentMoney)
        {
            this.Username = Username;
            this.phoneNumber = phoneNumber;
            this.FullName = FullName;
            this.TotalProjectDone = TotalProjectDone;
            this.Bio = Bio;
            this.LinkProject = LinkProject;
            this.LinkSocialMedia = LinkSocialMedia;
            this.CurrentMoney = CurrentMoney;
        }

        public PhotographerProfile()
        {
        }
    }
}