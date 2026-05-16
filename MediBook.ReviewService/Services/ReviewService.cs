using MediBook.ReviewService.DTOs;
using MediBook.ReviewService.Entities;
using MediBook.ReviewService.Interfaces;

namespace MediBook.ReviewService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;

        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReviewResponse> AddReview(AddReviewRequest request)
        {
            // Check if review already exists for this appointment
            var existingReview = await _repository.ExistsByAppointmentId(request.AppointmentId);
            if (existingReview)
            {
                throw new InvalidOperationException("A review already exists for this appointment.");
            }

            // Validate rating
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var review = new Review
            {
                ReviewId = Guid.NewGuid().ToString(),
                AppointmentId = request.AppointmentId,
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow,
                IsVerified = true,
                IsAnonymous = request.IsAnonymous
            };

            var createdReview = await _repository.CreateReview(review);
            return MapToResponse(createdReview);
        }

        public async Task<List<ReviewResponse>> GetByProvider(string providerId)
        {
            var reviews = await _repository.FindByProviderId(providerId);
            return reviews.Select(MapToResponse).ToList();
        }

        public async Task<List<ReviewResponse>> GetByPatient(string patientId)
        {
            var reviews = await _repository.FindByPatientId(patientId);
            return reviews.Select(MapToResponse).ToList();
        }

        public async Task<ReviewResponse?> GetByAppointment(string appointmentId)
        {
            var review = await _repository.FindByAppointmentId(appointmentId);
            return review == null ? null : MapToResponse(review);
        }

        public async Task<ReviewResponse?> UpdateReview(string reviewId, UpdateReviewRequest request)
        {
            var review = await _repository.FindByReviewId(reviewId);
            if (review == null) return null;

            // Validate rating
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.IsAnonymous = request.IsAnonymous;

            var updatedReview = await _repository.UpdateReview(review);
            return MapToResponse(updatedReview);
        }

        public async Task<bool> DeleteReview(string reviewId)
        {
            return await _repository.DeleteReview(reviewId);
        }

        public async Task<double> GetAvgRating(string providerId)
        {
            return await _repository.AvgRatingByProviderId(providerId);
        }

        public async Task<int> GetReviewCount(string providerId)
        {
            return await _repository.CountByProviderId(providerId);
        }

        public async Task<List<ReviewResponse>> GetAllReviews()
        {
            var reviews = await _repository.GetAllReviews();
            return reviews.Select(MapToResponse).ToList();
        }

        private ReviewResponse MapToResponse(Review review)
        {
            return new ReviewResponse
            {
                ReviewId = review.ReviewId,
                AppointmentId = review.AppointmentId,
                PatientId = review.PatientId,
                ProviderId = review.ProviderId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
                IsVerified = review.IsVerified,
                IsAnonymous = review.IsAnonymous
            };
        }
    }
}
