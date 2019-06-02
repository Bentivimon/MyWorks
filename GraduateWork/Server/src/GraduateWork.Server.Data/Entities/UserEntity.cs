using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represents user entity.
    /// </summary>
    [Table("users")]
    public class UserEntity
    {
        #region Properties

        /// <summary>
        /// Gets/Sets uniq user identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets user password.
        /// </summary>
        [Column("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets/Sets user email.
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets/Sets user first name
        /// </summary>
        [Column("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/Sets user last name.
        /// </summary>
        [Column("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets/Sets user phone number.
        /// </summary>
        [Column("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets/Sets user birthday date.
        /// </summary>
        [Column("birthday")]
        public DateTimeOffset Birthday { get; set; }

        /// <summary>
        /// Gets/Sets entrant id.
        /// </summary>
        [Column("entrant_id")]
        public Guid EntrantId { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets user associated with a entrant
        /// </summary>
        public EntrantEntity Entrant { get; set; }

        #endregion


        #region Converters

        /// <summary>
        /// Method for update properties of <see cref="UserEntity"/> by <see cref="UserDto"/>.
        /// </summary>
        /// <param name="model">Model with new data.</param>
        public void Update(UserDto model)
        {
            Birthday = model.Birthday;
            FirstName = model.FirstName;
            Phone = model.MobileNumber;
            LastName = model.LastName;
        }

        /// <summary>
        /// Method for convert <see cref="UserEntity"/> to <see cref="UserDto"/>
        /// </summary>
        public UserDto ToDto() => new UserDto()
        {
            Birthday = Birthday.UtcDateTime,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            MobileNumber = Phone
        };

        /// <summary>
        /// Method for convert <see cref="RegistrationModel"/> to <see cref="RegistrationModel"/>.
        /// </summary>
        /// <param name="model">Registration model</param>
        /// <param name="encodePassword">encoded password</param>
        public static UserEntity CreateEntity(RegistrationModel model, string encodePassword) => new UserEntity
        {
            Birthday = model.Birthday,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.MobileNumber,
            Password = encodePassword
        };


        #endregion

    }
}
