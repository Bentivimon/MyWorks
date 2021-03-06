﻿using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Speciality request model.
    /// </summary>
    public class SpecialityRequest
    {
        /// <summary>
        /// Gets/Sets code.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets/Sets name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets additional factor
        /// </summary>
        [Required]
        public string Faculty { get; set; }

        /// <summary>
        /// Gets/Sets count of state places.
        /// </summary>
        [Required]
        public string SubjectScores { get; set; }
    }
}
