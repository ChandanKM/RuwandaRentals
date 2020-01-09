using App.BusinessObject;
using App.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UIServices
{
    public interface ICorporateService
    {
        //TransactionStatus AddCorporate(CorporateBo corporate);
        //DataSet AddConsumerMandet(ConsumerMandetBo consumer);
        DataSet CorporateLogin(CorporateLoginBo login);
        DataSet FbCorporateLogin(CorporateLoginBo loginBo);
        //DataSet ConsumerDetails(ConsumerDetailsBo Cons);
        //DataSet ConsumerForgotpwd(ConsumerForgotpwdBo Cons);
        //DataSet PropertyList(ListingBo list);
        //DataSet PropertyList_Sort(ListingBo list);
        //DataSet PropertyListDetails(ListingDetailsBo list);
        //DataSet RoomList(ListingDetailsRoomBo list);
        //List<object> GetCity();

        //List<object> GetLocations();
        //TransactionStatus UpdateConsumer(ConsumerBo consumer);
        //TransactionStatus UpdateConsumerPswd(ConsumerBo consumer);
        //DataSet PreBooking(PrebookingBo PreBo);
        //DataSet PreBookingUpdate(PrebookingBo PreBo);
        //DataSet GetTransaction(string Invce_Num);
        //DataSet GetAllTransaction(PrebookingBo PreBo);


        //#region WebAppServices
        DataSet GetProfileDetails(string Cons_Id);
        //TransactionStatus ChangePassword(ConsumerChangePasswordBo changepasswordBo);
        //DataSet GetOverviewBookedDealsById(string Cons_Id);
        //DataSet GetBookedTransactionById(string Cons_Id);
        //TransactionStatus SubscribeEmailLatter(ConsumerSubscribeBo subscribeBo);
        //TransactionStatus UnSubscribeEmailLatter(ConsumerSubscribeBo subscribeBo);
        //TransactionStatus UpdateConsumerProfile(ConsumerFormBo consumer);
        //DataSet PropertyComplete_Details(ListingDetailsBo listBo);
        //DataSet BookingHotel_Details(BookNowDetailsBo booknowBo);
        //DataSet GetBookingInvoice(string Invce_Num, int Cons_Id);
        //DataSet GetLocationByCity(string name);

        //DataSet GetActiveFacilities();
        //DataSet GetHiddenGems();
        //DataSet GetRecommendedHotels();
        //DataSet GetBestOffers();
        //DataSet GetRoomPolicyById(int Prop_Id, int Room_Id);
        //DataSet GetRoomDetailsByID(int Prop_Id, int Room_Id);
        //TransactionStatus AddFeedBack(FeedBackBo feedbackBo);
        ////Feedback down
        //TransactionStatus AddFeedBack_Feed(FeedBackBo feedbackBo);
        //List<object> GetAutoCompleteLocation(string terms);
        //List<object> GetAutoCompleteLocationSearch(string terms);

        List<object> GetStates();
        List<object> GetPincodes();
        //#endregion

        List<object> GetAllCorporateUser(string EmailId);
        List<string> GetAllCorporateCompanies();
        List<object> GetAllCorporateUserByCompany(string company);
        bool UpdateCorporateUserToAdmin(string CorpEmail, string CorpCompany);
    }
}
