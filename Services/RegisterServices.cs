using Roof_Care.ExmxDbSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roof_Care.Models;
using Roof_Care.Helper;
using Roof_Care.Declarations;
using Roof_Care.AuthenticationDecode;
using System.IO;

namespace Roof_Care.Services
{
    public class RegisterServices
    {
        private readonly RoofCareEntities _dbContext;
        public RegisterServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal LogInModelResponse RegisterNewUser(NewRegisterModel new_user)
        {
            try
            {
                using (RoofCareEntities context = new RoofCareEntities())
                {
                    if (CheckUserExistance(new_user.Username) == false)
                    {
                        return HelperClass.LogInValid(false, GlobalDecleration._alreadyExits, "User already Exists!");
                    }
                    else
                    {
                        User user = new User
                        {
                            Username = new_user.Username,
                            Password = new_user.Password,
                            FullName = new_user.FullName,
                            Gender = new_user.Gender,
                            UserType = new_user.UserType,
                            UserProfileImage = ParseImage(new_user.UserProfileImage),
                            UserRating = 0,
                        };
                        context.Users.Add(user);

                        Address address = new Address
                        {
                            UserId = user.UserId,
                            AddressType = new_user.Addresses.AddressType,
                            DisctrictId = new_user.Addresses.DisctrictId,
                            MunicipalityId = new_user.Addresses.MunicipalityId,
                            CurrentLocation = new_user.Addresses.CurrentLocation
                        };
                        context.Addresses.Add(address);

                        Email email = new Email
                        {
                            UserId = user.UserId,
                            Email1 = new_user.Emails.Email1,
                            EmailType = new_user.Emails.EmailType
                        };
                        context.Emails.Add(email);
                        Contact contact = new Contact
                        {
                            UserId = user.UserId,
                            ContactNumber = new_user.Contacts.ContactNumber,
                            ContactType = new_user.Contacts.ContactType
                        };
                        context.Contacts.Add(contact);

                        foreach (UserProfession userProfession in new_user.UserProfession)
                        {
                            UserProfession userProfession1 = new UserProfession
                            {
                                UserId = user.UserId,
                                ProfessionId = userProfession.ProfessionId
                            };
                            context.UserProfessions.Add(userProfession1);
                        }
                        context.SaveChanges();
                        return HelperClass.LogInValid(true, GlobalDecleration._savedSuccesfully,
                            EncodeDecode.EncodeUserId(user.UserId, user.Username));
                    }
                }

            }
            catch (Exception ex)
            {
                return HelperClass.LogInValid(false, GlobalDecleration._internalServerError, "Server error " + ex);
            }
            throw new NotImplementedException();
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

        private bool CheckUserExistance(string username)
        {
            IQueryable<User> users;

            using (_dbContext)
            {
                users = _dbContext.Users.Where(x => x.Username == username);

                if (users.Any() == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        internal ResponseModel GetProfessions()
        {
            try
            {
                using (_dbContext)
                {
                    var services = (from service in _dbContext.Professions
                                    select new
                                    {
                                        service.ProfessionId,
                                        service.ProfessionName
                                    }).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, services);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Server issue + " + ex);
            }
            throw new NotImplementedException();
        }

    }
}
/*if (new_user.Addresses != null)
                       {
                           foreach (Address address in new_user.Addresses)
                           {
                               address.UserId = user.UserId;
                               context.Addresses.Add(address);
                           }
                       }
                       foreach (Contact contact in new_user.Contacts)
                       {
                           contact.UserId = user.UserId;
                           context.Contacts.Add(contact);
                       }
                       foreach (Email email in new_user.Emails)
                       {
                           email.UserId = user.UserId;
                           context.Emails.Add(email);
                       }
                       foreach (UserProfession userProfession in new_user.UserProfession)
                       {
                           userProfession.UserId = user.UserId;
                           context.UserProfessions.Add(userProfession);
                       }
                       context.SaveChanges();*/
