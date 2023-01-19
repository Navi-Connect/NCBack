using System.Net.Http.Headers;
using CorePush.Google;
using Microsoft.Extensions.Options;
using NCBack.Models;
using NCBack.NotificationModels;

namespace NCBack.Services;


public interface INotificationService
{
    Task<User> SendNotification(NotificationModel notificationModel);
}

public class NotificationService : INotificationService
{
    private readonly FcmNotificationSetting _fcmNotificationSetting;
        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<User> SendNotification(NotificationModel notificationModel)
        {
            User response = new User();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = Convert.ToString(notificationModel.DeviceId);

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GoogleNotification.DataPayload dataPayload = new GoogleNotification.DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.Success = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
}