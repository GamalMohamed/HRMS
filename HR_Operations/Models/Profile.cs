using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.WebPages;

namespace CoEX_HRMS.Models
{
    public class Profile
    {
        [Key, ForeignKey("Employee")]
        public int Id { get; set; }

        public virtual Employee Employee { get; set; }

        public string ProfilePic { get; set; }

        public string MilitaryCertificate { get; set; }

        public string BirthCertificate { get; set; }

        public string IdCard { get; set; }

        public string Passport { get; set; }

        public string GraduationCertificate { get; set; }

        public string ExtraDocument { get; set; }

        public bool PassportCheck()
        {
            return Passport != null;
        }
        public bool ImageCheck()
        {
            return ProfilePic != null;
        }
        public bool IdCardCheck()
        {
            return IdCard != null;
        }
        public bool MilitaryCertificateCheck()
        {

            return MilitaryCertificate != null;
        }
        public bool BirthCertificateCheck()
        {
            return BirthCertificate != null;
        }
        public bool GraduationCertificateCheck()
        {
            return GraduationCertificate != null;
        }

        public string MissedItems()
        {
            string res = "";
            if (this.Passport == null)
            {
                res += "Passport";
            }
            if (this.BirthCertificate == null)
            {
                if (res.IsEmpty())
                {
                    res += "Birth Certificate";
                }
                else
                {
                    res += "," + "Birth Certificate";
                }
            }
            if (this.GraduationCertificate == null)
            {
                if (res.IsEmpty())
                {
                    res += "Graduation Certificate";
                }
                else
                {
                    res += "," + "Graduation Certificate";
                }
            }
            if (this.IdCard == null)
            {
                if (res.IsEmpty())
                {
                    res += "Id Card";
                }
                else
                {
                    res += "," + "Id Card";
                }
            }
            if (this.MilitaryCertificate == null && this.Employee.Gender?.ToLower() != "female")
            {
                if (res.IsEmpty())
                {
                    res += "Military Certificate";
                }
                else
                {
                    res += "," + "Military Certificate";
                }
            }
            return res;
        }

        public string GetExistingDocuments()
        {
            var result = "";
            if (ProfilePic!=null)
            {
                result += ProfilePic;
                result += " ";
            }
            if (IdCard != null)
            {
                result += IdCard;
                result += " ";
            }
            if (BirthCertificate != null)
            {
                result += BirthCertificate;
                result += " ";
            }
            if (GraduationCertificate != null)
            {
                result += GraduationCertificate;
                result += " ";
            }
            if (Passport != null)
            {
                result += Passport;
                result += " ";
            }
            if (MilitaryCertificate != null)
            {
                result += MilitaryCertificate;
                result += " ";
            }
            if (ExtraDocument != null)
            {
                result += ExtraDocument;
                result += " ";
            }


            return result.Trim();
        }

        public string GetFileExtension(string docLink)
        {
            return docLink == null ? "" : Path.GetExtension((docLink.Split('/')[docLink.Split('/').Length - 1]));
        }

    }
}