using System.Collections.Generic;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent region. 
    /// </summary>
    public class RegionEntity
    {
        /// <summary>
        /// Gets/Sets region id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets/Sets region name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Foreign ket for universities.
        /// </summary>
        public List<UniversityEntity> Universities { get; set; }

        public RegionDto ToDto() => new RegionDto
        {
            Id = Id,
            Name = Name
        };
    }
}
