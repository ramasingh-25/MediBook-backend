using MediBook.ReviewService.Entities;

namespace MediBook.ReviewService.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> FindByReviewId(string reviewId);
        Task<List<Review>> FindByProviderId(string providerId);
        Task<List<Review>> FindByPatientId(string patientId);
        Task<Review?> FindByAppointmentId(string appointmentId);
        Task<double> AvgRatingByProviderId(string providerId);
        Task<List<Review>> FindByRating(int rating);
        Task<int> CountByProviderId(string providerId);
        Task<bool> ExistsByAppointmentId(string appointmentId);
        Task<List<Review>> GetAllReviews();
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(Review review);
        Task<bool> DeleteReview(string reviewId);
    }
}
