using MediBook.ReviewService.DTOs;

namespace MediBook.ReviewService.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponse> AddReview(AddReviewRequest request);
        Task<List<ReviewResponse>> GetByProvider(string providerId);
        Task<List<ReviewResponse>> GetByPatient(string patientId);
        Task<ReviewResponse?> GetByAppointment(string appointmentId);
        Task<ReviewResponse?> UpdateReview(string reviewId, UpdateReviewRequest request);
        Task<bool> DeleteReview(string reviewId);
        Task<double> GetAvgRating(string providerId);
        Task<int> GetReviewCount(string providerId);
        Task<List<ReviewResponse>> GetAllReviews();
    }
}
