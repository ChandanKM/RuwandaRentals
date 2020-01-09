using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IConsumerService
    {

        TransactionStatus AddConsumer(ConsumerBo consumer);
        DataSet AddConsumerMandet(ConsumerMandetBo consumer);
        DataSet ConsumerLogin(ConsumerLoginBo login);
        DataSet FbConsumerLogin(ConsumerLoginBo loginBo);
        DataSet ConsumerDetails(ConsumerDetailsBo Cons);
        DataSet ConsumerForgotpwd(ConsumerForgotpwdBo Cons);
        DataSet PropertyList(ListingBo list);
        DataSet PropertyList_Sort(ListingBo list);
        DataSet PropertyListDetails(ListingDetailsBo list);
        DataSet RoomList(ListingDetailsRoomBo list);
        List<object> GetCity();

        List<object> GetLocations();
        TransactionStatus UpdateConsumer(ConsumerBo consumer);
        TransactionStatus UpdateConsumerPswd(ConsumerBo consumer);
        DataSet PreBooking(PrebookingBo PreBo);
        DataSet PreBookingUpdate(PrebookingBo PreBo);
        DataSet GetTransaction(string Invce_Num);
        DataSet GetAllTransaction(PrebookingBo PreBo);


        #region WebAppServices
        DataSet GetProfileDetails(string Cons_Id);
        TransactionStatus ChangePassword(ConsumerChangePasswordBo changepasswordBo);
        DataSet GetOverviewBookedDealsById(string Cons_Id);
        DataSet GetBookedTransactionById(string Cons_Id);
        TransactionStatus CheckSubscribeEmailLatter(ConsumerSubscribeBo subscribeBo);
        TransactionStatus SubscribeEmailLatter(ConsumerSubscribeBo subscribeBo);
        TransactionStatus UnSubscribeEmailLatter(ConsumerSubscribeBo subscribeBo);
        TransactionStatus UpdateConsumerProfile(ConsumerFormBo consumer);
        DataSet PropertyComplete_Details(ListingDetailsBo listBo);
        DataSet BookingHotel_Details(BookNowDetailsBo booknowBo);
        DataSet GetBookingInvoice(string Invce_Num, int Cons_Id);
        DataSet CheckBookingStatus(string Invce_Num, int Cons_Id);
        DataSet CheckCorporateUser(string Cons_Id);
        DataSet GetLocationByCity(string name);

        DataSet GetActiveFacilities();
        DataSet GetHiddenGems();
        DataSet GetRecommendedHotels();
        DataSet GetBestOffers();
        DataSet GetRoomPolicyById(int Prop_Id, int Room_Id);
        DataSet GetRoomDetailsByID(int Prop_Id, int Room_Id);
        TransactionStatus AddFeedBack(FeedBackBo feedbackBo);
        //Feedback down
        TransactionStatus AddFeedBack_Feed(FeedBackBo feedbackBo);
        List<object> GetAutoCompleteLocation(string terms);
        List<object> GetAutoCompleteLocationSearch(string terms);

        List<object> GetStates();
        List<object> GetPincodes();
        #endregion
    }


}
