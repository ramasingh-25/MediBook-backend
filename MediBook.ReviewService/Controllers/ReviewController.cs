using Microsoft.AspNetCore.Mvc;
using MediBook.ReviewService.DTOs;
using MediBook.ReviewService.Interfaces;

namespace MediBook.ReviewService.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<ActionResult<ReviewResponse>> AddReview([FromBody] AddReviewRequest request)
        {
            try
            {
                var response = await _reviewService.AddReview(request);
                return CreatedAtAction(nameof(GetByAppointment), new { appointmentId = response.AppointmentId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<List<ReviewResponse>>> GetByProvider(string providerId)
        {
            var reviews = await _reviewService.GetByProvider(providerId);
            return Ok(reviews);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<ReviewResponse>>> GetByPatient(string patientId)
        {
            var reviews = await _reviewService.GetByPatient(patientId);
            return Ok(reviews);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<ReviewResponse>> GetByAppointment(string appointmentId)
        {
            try
            {
                var review = await _reviewService.GetByAppointment(appointmentId);
                if (review == null)
                {
                    return NotFound(new { message = "Review not found." });
                }
                return Ok(review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{reviewId}")]
        public async Task<ActionResult<ReviewResponse>> UpdateReview(string reviewId, [FromBody] UpdateReviewRequest request)
        {
            var review = await _reviewService.UpdateReview(reviewId, request);
            if (review == null)
            {
                return NotFound(new { message = "Review not found." });
            }
            return Ok(review);
        }

        [HttpDelete("{reviewId}")]
        public async Task<ActionResult<bool>> DeleteReview(string reviewId)
        {
            var result = await _reviewService.DeleteReview(reviewId);
            if (!result)
            {
                return NotFound(new { message = "Review not found." });
            }
            return Ok(true);
        }

        [HttpGet("provider/{providerId}/avgRating")]
        [HttpGet("provider/{providerId}/rating")]
        public async Task<ActionResult<double>> GetAvgRating(string providerId)
        {
            var avgRating = await _reviewService.GetAvgRating(providerId);
            return Ok(new { averageRating = avgRating });
        }

        [HttpGet("provider/{providerId}/summary")]
        public async Task<ActionResult> GetReviewSummary(string providerId)
        {
            var count = await _reviewService.GetReviewCount(providerId);
            var avgRating = await _reviewService.GetAvgRating(providerId);
            return Ok(new { 
                totalReviews = count, 
                averageRating = avgRating,
                recommendationRate = avgRating >= 4 ? 95 : 80 
            });
        }

        [HttpGet("provider/{providerId}/count")]
        public async Task<ActionResult<int>> GetReviewCount(string providerId)
        {
            var count = await _reviewService.GetReviewCount(providerId);
            return Ok(count);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ReviewResponse>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }
    }
}
