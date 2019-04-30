using Microsoft.Extensions.Configuration;

namespace GraduateWork.Server.Api.Options
{
    /// <summary>
    /// Options for swagger.
    /// </summary>
    public sealed class SwaggerOptions
    {
        /// <summary>
        /// Method for read configuration from file.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> instance.</param>
        public static SwaggerOptions Read(IConfiguration configuration)
        {
            return configuration.GetSection("Swagger")?.Get<SwaggerOptions>();
        }

        /// <summary>
        /// Gets/Sets enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets/Sets version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets/Sets xml comments file name.
        /// </summary>
        public string XmlCommentsFileName { get; set; }
    }
}
