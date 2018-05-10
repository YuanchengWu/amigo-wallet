using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmigoWalletDAL;
using AutoMapper;

namespace AmigoWalletMVCApp.Repository
{
    /// <summary>
    /// Mapper class to convert objects database to mvc model and vice-versa
    /// </summary>
    /// <typeparam name="Source"></typeparam>
    /// <typeparam name="Destination"></typeparam>
    public class AmigoWalletMapper<Source, Destination>
        where Source: class
        where Destination: class
    {
        //The mapper class
        public AmigoWalletMapper()
        {
            //User -> Models.User
            Mapper.CreateMap<User, Models.User>();
            //Models.User-> User
            Mapper.CreateMap<Models.User, User>();
            //Merchant -> Models.Merchant
            Mapper.CreateMap<Merchant, Models.Merchant>();
            //Models.Merchant -> Merchant
            Mapper.CreateMap<Models.Merchant, Merchant>();
            //UserCard -> Models.usercard
            Mapper.CreateMap<UserCard, Models.UserCard>();
            //Models.Usercard -> UserCard
            Mapper.CreateMap<Models.UserCard, UserCard>();
            //UserTransaction -> Models.UserTransaction
            Mapper.CreateMap<UserTransaction, Models.UserTransaction>();
            //Models.UserTransaction -> UserTransaction
            Mapper.CreateMap<Models.UserTransaction, UserTransaction>();
            //MerchantServiceType -> Models.MerchantServiceType
            Mapper.CreateMap<MerchantServiceType, Models.MerchantServiceType>();
            //Models.MerchantServiceType -> MerchantServiceType
            Mapper.CreateMap<Models.MerchantServiceType, MerchantServiceType>();
        }
        /// <summary>
        /// Used to translate between entity and model, and vice-versa
        /// </summary>
        /// <param name="obj">Object to be translated</param>
        /// <returns></returns>
        public Destination Translate(Source obj)
        {
            return Mapper.Map<Source, Destination>(obj);
        }
    }
}