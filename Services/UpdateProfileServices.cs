using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roof_Care.Helper;
using Roof_Care.Declarations;
using System.IO;
using System.Data.Entity;

namespace Roof_Care.Services
{
    public class UpdateProfileServices
    {
        private readonly RoofCareEntities _dbContext;
        public UpdateProfileServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        public ResponseModel GetDetailsToUpdate(int userId)
        {
            try
            {
                using (RoofCareEntities context = new RoofCareEntities())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    var neededDetails = (from details in context.Users
                                         select new
                                         {
                                             details.UserId,
                                             details.Username,
                                             details.UserProfileImage,
                                             details.FullName,
                                             details.Gender,
                                             details.Password,
                                             details.UserType,
                                             Contacts = from contact in details.Contacts
                                                        select new
                                                        {
                                                            contact.ContactId,
                                                            contact.ContactNumber,
                                                            contact.ContactType
                                                        },
                                             Emails = from email in details.Emails
                                                      select new
                                                      {
                                                          email.EmailId,
                                                          email.Email1,
                                                          email.EmailType,
                                                      },
                                             Addresses = from address in details.Addresses
                                                         select new
                                                         {
                                                             address.AddressId,
                                                             address.AddressType,
                                                             address.DisctrictId,
                                                             address.District.DistrictName,
                                                             address.Municipality.MunicipalityName,
                                                             address.MunicipalityId,
                                                             address.CurrentLocation,
                                                         },
                                             Professions = from profession in details.UserProfessions
                                                           select new
                                                           {
                                                               profession.ProfessionId,
                                                               profession.Profession.ProfessionName
                                                           }
                                         }).Where(user => user.UserId.Equals(userId)).ToList();

                    return HelperClass.Response(true, GlobalDecleration._successAction, neededDetails);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Sever issue please try again! " + ex);
            }
        }

        internal ResponseModel RemoveEmails(int id)
        {
            try
            {
                using (_dbContext)
                {
                    Email email = _dbContext.Emails.Find(id);
                    if (email != null)
                    {
                        _dbContext.Emails.Remove(email);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Removed Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._notChats, "No Emails");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
        }


        internal ResponseModel RemoveContacts(int id)
        {
            try
            {
                using (_dbContext)
                {
                    Contact contact = _dbContext.Contacts.Find(id);
                    if (contact != null)
                    {
                        _dbContext.Contacts.Remove(contact);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Removed Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._notChats, "No Contacts");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
        }

        internal ResponseModel RemoveAddress(int id)
        {
            try
            {
                using (_dbContext)
                {
                    Address address = _dbContext.Addresses.Find(id);
                    if (address != null)
                    {
                        _dbContext.Addresses.Remove(address);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Removed Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._notChats, "No Address");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
        }

        public ResponseModel UpdateProfileDetails(int userId, ProfileUpdateModel new_user)
        {
            try
            {
                using (_dbContext)
                {
                    User old_user = _dbContext.Users.Find(userId);
                    if (old_user != null)
                    {
                        old_user.AboutUser = new_user.AboutUser;
                        old_user.FullName = new_user.FullName;
                        old_user.Gender = new_user.Gender;
                        old_user.UserProfileImage = ParseImage(new_user.UserProfileImage);
                        _dbContext.Entry(old_user).State = EntityState.Modified;
                    }
                    UpdateContacts(new_user, userId);
                    UpdateEmails(new_user, userId);
                    UpdateAddress(new_user, userId);
                    _dbContext.SaveChanges();

                    return HelperClass.Response(true, GlobalDecleration._successAction, "Profile Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        private void UpdateAddress(ProfileUpdateModel new_user, int userId)
        {
            foreach (Address new_address in new_user.Addresses)
            {
                Address old_address = _dbContext.Addresses.Find(new_address.AddressId);
                if (old_address != null)
                {
                    old_address.DisctrictId = new_address.DisctrictId;
                    old_address.MunicipalityId = new_address.MunicipalityId;
                    old_address.CurrentLocation = new_address.CurrentLocation;
                    old_address.AddressType = new_address.AddressType;
                    _dbContext.Entry(old_address).State = EntityState.Modified;
                }
                else
                {
                    Address address = new Address
                    {
                        DisctrictId = new_address.DisctrictId,
                        MunicipalityId = new_address.MunicipalityId,
                        CurrentLocation = new_address.CurrentLocation,
                        AddressType = new_address.AddressType,
                        UserId = userId
                    };
                    _dbContext.Addresses.Add(address);
                }
            }
        }

        private void UpdateEmails(ProfileUpdateModel new_user, int userId)
        {
            foreach (Email new_email in new_user.Emails)
            {
                Email old_email = _dbContext.Emails.Find(new_email.EmailId);
                if (old_email != null)
                {
                    old_email.Email1 = new_email.Email1;
                    old_email.EmailType = new_email.EmailType;
                    _dbContext.Entry(old_email).State = EntityState.Modified;
                }
                else
                {
                    Email email = new Email
                    {
                        Email1 = new_email.Email1,
                        EmailType = new_email.EmailType,
                        UserId = userId
                    };
                    _dbContext.Emails.Add(email);
                }
            }
        }

        private void UpdateContacts(ProfileUpdateModel new_user, int id)
        {
            foreach (Contact new_contact in new_user.Contacts)
            {
                Contact old_contact = _dbContext.Contacts.Find(new_contact.ContactId);
                if (old_contact != null)
                {
                    old_contact.ContactNumber = new_contact.ContactNumber;
                    old_contact.ContactType = new_contact.ContactType;
                    _dbContext.Entry(old_contact).State = EntityState.Modified;
                }
                else
                {
                    Contact contact = new Contact
                    {
                        ContactNumber = new_contact.ContactNumber,
                        ContactType = new_contact.ContactType,
                        UserId = id,
                    };
                    _dbContext.Contacts.Add(contact);
                }
            }
        }

        private string ParseImage(string bitmapString)
        {
            try
            {
                string folderLocation = "~/MyUploads/ProfileImages/";
                string imageName = ((RandomString(10) + DateTime.Now) + ".jpg").Replace(":", String.Empty);

                byte[] img = LoadImage(bitmapString);

                string filePath = HttpContext.Current.Server
                    .MapPath(folderLocation +
                    Path.GetFileName(imageName));

                File.WriteAllBytes(filePath, img);

                string finalLocation = "MyUploads/ProfileImages/";
                return (finalLocation + imageName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public byte[] LoadImage(string bitmapString)
        {
            byte[] imageBytes = Convert.FromBase64String(bitmapString);
            return imageBytes;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}