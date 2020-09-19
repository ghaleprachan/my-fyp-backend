using Microsoft.AspNet.SignalR;
using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class ChatsServices
    {
        private readonly RoofCareEntities _dbContext;
        public ChatsServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }
        internal ResponseModel GetChats(string senderId, string receiverId)
        {
            try
            {
                using (_dbContext)
                {
                    int sendId = EncodeDecode.GetUserId(senderId);
                    int receiveId = EncodeDecode.GetUserId(receiverId);
                    var userChats = (from chats in _dbContext.ChatSystems
                                     select new
                                     {
                                         chats.SenderId,
                                         chats.ReceiverId,
                                         SenderUsername = chats.User.Username,
                                         SenderName = chats.User.FullName,
                                         ReceiverUsername = chats.User1.Username,
                                         ReceiverName = chats.User1.FullName,
                                         SenderPhoto = chats.User.UserProfileImage,
                                         ReceiverPhoto = chats.User1.UserProfileImage,
                                         chats.Chat_id,
                                         chats.SendDate,
                                         chats.Message,
                                     }).ToList()
                             .OrderBy(x => x.SendDate)
                             .Where(s => s.SenderId == sendId && s.ReceiverId == receiveId
                             || s.SenderId == receiveId && s.ReceiverId == sendId);
                    if (userChats == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._notChats, "Start Conversation");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, userChats);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Failed to get messages \n" + ex);
            }
            throw new NotImplementedException();
        }
        internal ResponseModel AddChats(NewChatModel chat)
        {
            try
            {
                using (_dbContext)
                {
                    ChatSystem new_chat = new ChatSystem
                    {
                        SenderId = EncodeDecode.GetUserId(chat.senderId),
                        ReceiverId = EncodeDecode.GetUserId(chat.receiverid),
                        Message = chat.message,
                        SendDate = DateTime.Now
                    };

                    _dbContext.ChatSystems.Add(new_chat);

                    // This part is for updating last message into the database
                    int p1 = EncodeDecode.GetUserId(chat.senderId);
                    int p2 = EncodeDecode.GetUserId(chat.receiverid);
                    LastMessage old_one = _dbContext.LastMessages.Where(lastchat => lastchat.ParticipantOne == p1 &&
                    lastchat.ParticipantTwo == p2 || lastchat.ParticipantOne == p2 && lastchat.ParticipantTwo == p1).FirstOrDefault();

                    if (old_one != null)
                    {
                        old_one.LastMessage1 = chat.message;
                        old_one.SendDate = DateTime.Now;
                        old_one.Seen = "false";
                        old_one.SenderId = chat.senderId;

                        _dbContext.Entry(old_one).State = EntityState.Modified;
                        LastMessage new_one = _dbContext.LastMessages.Find(old_one.LastChatId);
                    }
                    else
                    {
                        LastMessage add_new = new LastMessage
                        {
                            ParticipantOne = EncodeDecode.GetUserId(chat.senderId),
                            ParticipantTwo = EncodeDecode.GetUserId(chat.receiverid),
                            LastMessage1 = chat.message,
                            SendDate = DateTime.Now,
                            Seen = "false",
                            SenderId = chat.senderId,
                        };
                        _dbContext.LastMessages.Add(add_new);
                    }
                    _dbContext.SaveChanges();

                    var userChats = (from chats in _dbContext.ChatSystems
                                     select new
                                     {
                                         chats.SenderId,
                                         chats.ReceiverId,
                                         SenderUsername = chats.User.Username,
                                         SenderName = chats.User.FullName,
                                         ReceiverUsername = chats.User1.Username,
                                         ReceiverName = chats.User1.FullName,
                                         SenderPhoto = chats.User.UserProfileImage,
                                         ReceiverPhoto = chats.User1.UserProfileImage,
                                         chats.Chat_id,
                                         chats.SendDate,
                                         chats.Message,
                                     }).ToList().Where(c => c.Chat_id == new_chat.Chat_id);

                    return HelperClass.Response(true, GlobalDecleration._successAction, userChats);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Meesage not send!" + ex);
            }
            throw new NotImplementedException();
        }
        internal ResponseModel UpdateSeen(LastChatUpdateModel model)
        {
            try
            {
                int p1 = EncodeDecode.GetUserId(model.participantOne);
                int p2 = EncodeDecode.GetUserId(model.participantTwo);
                LastMessage old_one = _dbContext.LastMessages.Where(chat => chat.ParticipantOne == p1 &&
                chat.ParticipantTwo == p2 || chat.ParticipantOne == p2 && chat.ParticipantTwo == p1).FirstOrDefault();

                if (old_one != null)
                {
                    old_one.Seen = "true";
                    _dbContext.Entry(old_one).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    LastMessage new_one = _dbContext.LastMessages.Find(old_one.LastChatId);
                    return HelperClass.Response(true, GlobalDecleration._successAction, new_one.LastChatId);
                }
                else
                {
                    return HelperClass.Response(false, GlobalDecleration._notChats, "Not chats found");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Failed to update" + ex);
            }
        }
       /* internal ResponseModel UpDateLastChat(LastChatUpdateModel lastMessage)
        {
            try
            {
                int p1 = EncodeDecode.GetUserId(lastMessage.participantOne);
                int p2 = EncodeDecode.GetUserId(lastMessage.participantTwo);
                LastMessage old_one = _dbContext.LastMessages.Where(chat => chat.ParticipantOne == p1 &&
                chat.ParticipantTwo == p2 || chat.ParticipantOne == p2 && chat.ParticipantTwo == p1).FirstOrDefault();

                if (old_one != null)
                {
                    old_one.LastMessage1 = lastMessage.lastMessage;
                    old_one.SendDate = DateTime.Now;
                    old_one.Seen = "false";
                    old_one.SenderId = lastMessage.senderId;

                    _dbContext.Entry(old_one).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    LastMessage new_one = _dbContext.LastMessages.Find(old_one.LastChatId);

                    return HelperClass.Response(true, GlobalDecleration._successAction, new_one.LastChatId);
                }
                else
                {
                    LastMessage add_new = new LastMessage
                    {
                        ParticipantOne = EncodeDecode.GetUserId(lastMessage.participantOne),
                        ParticipantTwo = EncodeDecode.GetUserId(lastMessage.participantTwo),
                        LastMessage1 = lastMessage.lastMessage,
                        SendDate = DateTime.Now,
                        Seen = "false",
                        SenderId = lastMessage.senderId,
                    };

                    _dbContext.LastMessages.Add(add_new);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._savedSuccesfully, add_new.LastChatId);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Errror" + ex);
            }
        }*/
        internal ResponseModel GetChatHeading(string userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    int id = EncodeDecode.GetUserId(userId);
                    var userLastChats = (from lastChat in _dbContext.LastMessages
                                         select new
                                         {
                                             lastChat.LastChatId,
                                             ParticipantOneId = lastChat.User.UserId,
                                             ParticipantOneUsername = lastChat.User.Username,
                                             ParticipantOneName = lastChat.User.FullName,
                                             ParticipantOneImage = lastChat.User.UserProfileImage,

                                             ParticipantTwoId = lastChat.User1.UserId,
                                             ParticipantTwoUsername = lastChat.User1.Username,
                                             ParticipantTwoName = lastChat.User1.FullName,
                                             ParticipantTwoImage = lastChat.User1.UserProfileImage,

                                             lastChat.SendDate,
                                             lastChat.LastMessage1,
                                             lastChat.Seen,
                                             lastChat.SenderId,
                                         })
                                         .OrderByDescending(x => x.SendDate)
                                         .Where(p => p.ParticipantOneId == id || p.ParticipantTwoId == id)
                                         .ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, userLastChats);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Server Issaues.." + ex);
            }
        }
    }
}

//internal ResponseModel onDbChange(ChatSystem chat)
//{
//    ChatSystem new_chat = new ChatSystem
//    {
//        SenderId = chat.SenderId,
//        ReceiverId = chat.ReceiverId,
//        Message = chat.Message,
//        SendDate = DateTime.Now
//    };
//    _dbContext.ChatSystems.Add(new_chat);
//    _dbContext.SaveChanges();

//    /* var notificationHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
//     notificationHub.Clients.All.Announce(chat.SenderId, chat.ReceiverId, chat.Message);*/

//    /*var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
//    context.Clients.All.Send("Announce", chat.SenderId, chat.ReceiverId, chat.Message);*/


//    /*ChatHub chatHub1 = new ChatHub();
//    ChatHub chatHub = chatHub1;
//    chatHub.Announce(HelperClass.Response(true, GlobalDecleration._successAction, chat));*/
//    return HelperClass.Response(true, GlobalDecleration._successAction, "Success");

//}

/*var userChats = (from userChat in _dbContext.ChatSystems
                                 select new
                                 {
                                     SenderId = userChat.User.UserId,
                                     SenderName = userChat.User.FullName,
                                     SenderUsername = userChat.User.Username,
                                     LastMessage = userChat.Message,
                                     userChat.SendDate,
                                     ReceiverId = userChat.User1.UserId,
                                     ReceiverUsername = userChat.User1.Username,
                                     ReceiverName = userChat.User1.FullName,
                                     ReceiverImage = userChat.User1.UserProfileImage,
                                 }).OrderByDescending(o => o.SendDate).FirstOrDefault();*/
