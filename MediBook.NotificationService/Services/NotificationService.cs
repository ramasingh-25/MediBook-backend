using MediBook.NotificationService.DTOs;
using MediBook.NotificationService.Entities;
using MediBook.NotificationService.Interfaces;

namespace MediBook.NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<NotificationResponse> Send(SendNotificationRequest request)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid().ToString(),
                RecipientId = request.RecipientId,
                Type = request.Type,
                Title = request.Title,
                Message = request.Message,
                Channel = request.Channel,
                RelatedId = request.RelatedId,
                RelatedType = request.RelatedType,
                IsRead = false,
                SentAt = DateTime.UtcNow
            };

            // Send email if channel is EMAIL
            if (request.Channel == "EMAIL")
            {
                await SendEmail(request.RecipientId, request.Title, request.Message);
            }

            // Send SMS if channel is SMS
            if (request.Channel == "SMS")
            {
                await SendSms(request.RecipientId, request.Message);
            }

            var createdNotification = await _repository.CreateNotification(notification);
            return MapToResponse(createdNotification);
        }

        public async Task<List<NotificationResponse>> SendBulk(SendBulkNotificationRequest request)
        {
            var responses = new List<NotificationResponse>();

            foreach (var recipientId in request.RecipientIds)
            {
                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    RecipientId = recipientId,
                    Type = request.Type,
                    Title = request.Title,
                    Message = request.Message,
                    Channel = request.Channel,
                    RelatedId = request.RelatedId,
                    RelatedType = request.RelatedType,
                    IsRead = false,
                    SentAt = DateTime.UtcNow
                };

                // Send email if channel is EMAIL
                if (request.Channel == "EMAIL")
                {
                    await SendEmail(recipientId, request.Title, request.Message);
                }

                // Send SMS if channel is SMS
                if (request.Channel == "SMS")
                {
                    await SendSms(recipientId, request.Message);
                }

                var createdNotification = await _repository.CreateNotification(notification);
                responses.Add(MapToResponse(createdNotification));
            }

            return responses;
        }

        public async Task<NotificationResponse?> MarkAsRead(string notificationId)
        {
            var notification = await _repository.FindByNotificationId(notificationId);
            if (notification == null) return null;

            notification.IsRead = true;
            var updatedNotification = await _repository.UpdateNotification(notification);
            return MapToResponse(updatedNotification);
        }

        public async Task<bool> MarkAllRead(string recipientId)
        {
            var notifications = await _repository.FindByRecipientIdAndIsRead(recipientId, false);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                await _repository.UpdateNotification(notification);
            }
            return true;
        }

        public async Task<List<NotificationResponse>> GetByRecipient(string recipientId)
        {
            var notifications = await _repository.FindByRecipientId(recipientId);
            return notifications.Select(MapToResponse).ToList();
        }

        public async Task<int> GetUnreadCount(string recipientId)
        {
            return await _repository.CountByRecipientIdAndIsRead(recipientId, false);
        }

        public async Task<bool> DeleteNotification(string notificationId)
        {
            return await _repository.DeleteNotification(notificationId);
        }

        public async Task<List<NotificationResponse>> GetAllNotifications()
        {
            var notifications = await _repository.GetAllNotifications();
            return notifications.Select(MapToResponse).ToList();
        }

        private async Task SendEmail(string recipientId, string title, string message)
        {
            // Placeholder for email sending logic using MailKit/MimeKit
            // In a real implementation, this would:
            // 1. Fetch user email by recipientId from Auth Service
            // 2. Use MailKit to send the email
            // For now, just log the action
            await Task.CompletedTask;
        }

        private async Task SendSms(string recipientId, string message)
        {
            // Placeholder for SMS sending logic using Twilio SDK
            // In a real implementation, this would:
            // 1. Fetch user phone by recipientId from Auth Service
            // 2. Use Twilio SDK to send the SMS
            // For now, just log the action
            await Task.CompletedTask;
        }

        private NotificationResponse MapToResponse(Notification notification)
        {
            return new NotificationResponse
            {
                NotificationId = notification.NotificationId,
                RecipientId = notification.RecipientId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                Channel = notification.Channel,
                RelatedId = notification.RelatedId,
                RelatedType = notification.RelatedType,
                IsRead = notification.IsRead,
                SentAt = notification.SentAt
            };
        }
    }
}
