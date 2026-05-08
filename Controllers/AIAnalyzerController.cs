using Microsoft.AspNetCore.Mvc;

namespace StudentGradeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIAnalyzerController : ControllerBase
    {
        // POST: api/aianalyzer/analyze
        // Analyzes a student grade and returns AI recommendation
        [HttpPost("analyze")]
        public IActionResult AnalyzeGrade([FromBody] GradeAnalysisRequest request)
        {
            if (string.IsNullOrEmpty(request.StudentName))
                return BadRequest(new { message = "Student name is required" });

            var analysis = GenerateAnalysis(request);
            return Ok(analysis);
        }

        // GET: api/aianalyzer/recommendations
        // Returns grade improvement recommendations
        [HttpGet("recommendations")]
        public IActionResult GetRecommendations([FromQuery] double grade)
        {
            var recommendation = GetGradeRecommendation(grade);
            return Ok(new
            {
                grade,
                recommendation,
                analyzedAt = DateTime.UtcNow
            });
        }

        // GET: api/aianalyzer/performance-report
        // Generates a full performance report
        [HttpGet("performance-report")]
        public IActionResult GetPerformanceReport()
        {
            var report = new
            {
                reportGeneratedAt = DateTime.UtcNow,
                summary = "AI-Powered Grade Analysis Report",
                gradeDistribution = new[]
                {
                    new { range = "90-100", label = "Excellent", count = 3 },
                    new { range = "80-89", label = "Good", count = 5 },
                    new { range = "70-79", label = "Average", count = 4 },
                    new { range = "Below 70", label = "Needs Improvement", count = 2 }
                },
                aiInsights = new[]
                {
                    "Students scoring below 75 need additional tutoring",
                    "Top performers should be enrolled in advanced courses",
                    "Average score improvement of 12% detected this semester"
                }
            };
            return Ok(report);
        }

        private GradeAnalysisResponse GenerateAnalysis(GradeAnalysisRequest request)
        {
            var recommendation = GetGradeRecommendation(request.Grade);
            var performanceLevel = GetPerformanceLevel(request.Grade);
            var nextSteps = GetNextSteps(request.Grade);

            return new GradeAnalysisResponse
            {
                StudentName = request.StudentName,
                Grade = request.Grade,
                Course = request.Course,
                PerformanceLevel = performanceLevel,
                Recommendation = recommendation,
                NextSteps = nextSteps,
                AnalyzedAt = DateTime.UtcNow
            };
        }

        private string GetGradeRecommendation(double grade)
        {
            return grade switch
            {
                >= 90 => "Outstanding performance! Recommend advanced coursework and leadership opportunities.",
                >= 80 => "Strong performance. Consider peer tutoring program to reinforce learning.",
                >= 70 => "Satisfactory performance. Additional practice exercises recommended.",
                >= 60 => "Below average. Immediate intervention and tutoring sessions advised.",
                _ => "Critical intervention needed. Schedule meeting with academic advisor immediately."
            };
        }

        private string GetPerformanceLevel(double grade)
        {
            return grade switch
            {
                >= 90 => "Excellent",
                >= 80 => "Good",
                >= 70 => "Average",
                >= 60 => "Below Average",
                _ => "Critical"
            };
        }

        private string[] GetNextSteps(double grade)
        {
            if (grade >= 90)
                return new[]
                {
                    "Enroll in honors program",
                    "Apply for academic scholarships",
                    "Consider teaching assistant role"
                };
            if (grade >= 80)
                return new[]
                {
                    "Join study groups",
                    "Attend optional workshops",
                    "Review advanced materials"
                };
            if (grade >= 70)
                return new[]
                {
                    "Schedule weekly tutor sessions",
                    "Complete extra practice problems",
                    "Attend office hours"
                };
            return new[]
            {
                "Meet with academic advisor immediately",
                "Daily tutoring sessions required",
                "Review all foundational concepts"
            };
        }
    }

    public class GradeAnalysisRequest
    {
        public string StudentName { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public double Grade { get; set; }
    }

    public class GradeAnalysisResponse
    {
        public string StudentName { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public double Grade { get; set; }
        public string PerformanceLevel { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
        public string[] NextSteps { get; set; } = Array.Empty<string>();
        public DateTime AnalyzedAt { get; set; }
    }
}