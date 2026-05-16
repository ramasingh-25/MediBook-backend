using MediBook.ReviewService.Data;
using MediBook.ReviewService.Entities;
using MediBook.ReviewService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.ReviewService.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewDbContext _context;

        public ReviewRepository(ReviewDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> FindByReviewId(string reviewId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<List<Review>> FindByProviderId(string providerId)
        {
            return await _context.Reviews
                .Where(r => r.ProviderId == providerId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<List<Review>> FindByPatientId(string patientId)
        {
            return await _context.Reviews
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review?> FindByAppointmentId(string appointmentId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
        }

        public async Task<double> AvgRatingByProviderId(string providerId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProviderId == providerId && r.IsVerified)
                .ToListAsync();

            if (!reviews.Any())
            {
                return 0.0;
            }

            return reviews.Average(r => (double)r.Rating);
        }

        public async Task<List<Review>> FindByRating(int rating)
        {
            return await _context.Reviews
                .Where(r => r.Rating == rating)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<int> CountByProviderId(string providerId)
        {
            return await _context.Reviews
                .Where(r => r.ProviderId == providerId)
                .CountAsync();
        }

        public async Task<bool> ExistsByAppointmentId(string appointmentId)
        {
            return await _context.Reviews.AnyAsync(r => r.AppointmentId == appointmentId);
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _context.Reviews
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReview(string reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
