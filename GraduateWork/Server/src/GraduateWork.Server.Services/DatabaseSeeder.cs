using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Data.Entities;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services
{
    /// <summary>
    /// Database seeder.
    /// </summary>
    public class DatabaseSeeder
    {
        private DatabaseContext _context;
        private ICryptoProvider _cryptoProvider;
        
        /// <summary>
        /// Method for seed default data to database.
        /// </summary>
        public async Task SeedAsync(IServiceProvider services)
        {
            _context = services.GetRequiredService<DatabaseContext>();
            _cryptoProvider = services.GetRequiredService<ICryptoProvider>();

            await SeedUsersAsync().ConfigureAwait(false);
            await SeedUniversitiesAsync().ConfigureAwait(false);
            await SeedEntrantsAsync().ConfigureAwait(false);
        }

        private async Task SeedUniversitiesAsync()
        {
            if (await _context.Universities.AnyAsync().ConfigureAwait(false))
                return;

            var universities = new List<UniversityEntity>
            {
                new UniversityEntity
                {
                    Region = new RegionEntity
                    {
                        Name = "Тернопільска обл."
                    },
                    FullName = "Тернопільський Національний Економічний Університет",
                    LevelOfAccreditation = "Вищий навчальний заклад України IV-го рівня акредитації",
                    UniversitySpecialities = new List<UniversitySpecialityEntity>
                    {
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Комп’ютерна інженерія",
                                Code = "123",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Системний аналіз",
                                Code = "124",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Кібербезпека",
                                Code = "125",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        }
                    },
                    Ownership = "Державна",
                    Chief = "Крисоватий Андрій Ігорович",
                    Subordination = "Мінестерство освіти і науки, молоді та спорту України",
                    PostIndex = "46020",
                    Address = "вул. Львівська 11",
                    Phone = "(0352) 47 50 63",
                    Email = "info@tneu.edu.ua",
                    Site = "www.tneu.edu.ua"
                },
                new UniversityEntity
                {
                    Region = new RegionEntity
                    {
                        Name = "Львівська обл."
                    },
                    FullName = "Національний університет Львівська політехніка",
                    LevelOfAccreditation = "Вищий навчальний заклад України IV-го рівня акредитації",
                    UniversitySpecialities = new List<UniversitySpecialityEntity>
                    {
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Комп’ютерна інженерія",
                                Code = "123",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Системний аналіз",
                                Code = "124",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Кібербезпека",
                                Code = "125",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        }
                    },
                    Ownership = "Державна",
                    Chief = "Бобало Юрій Ярославович",
                    Subordination = "Мінестерство освіти і науки, молоді та спорту України",
                    PostIndex = "79013",
                    Address = "вул. Степана Бандери 12",
                    Phone = "(032) 258-22-19",
                    Email = "web@lpnu.ua",
                    Site = "lp.edu.ua/"

                },
                new UniversityEntity
                {
                    Region = new RegionEntity
                    {
                        Name = "Київська обл."
                    },
                    FullName =
                        "Національний технічний університет України «Київський політехнічний інститут імені Ігоря Сікорського»",
                    LevelOfAccreditation = "Вищий навчальний заклад України IV-го рівня акредитації",
                    UniversitySpecialities = new List<UniversitySpecialityEntity>
                    {
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Комп’ютерна інженерія",
                                Code = "123",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Системний аналіз",
                                Code = "124",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Кібербезпека",
                                Code = "125",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій"
                            }
                        }
                    },
                    Ownership = "Державна",
                    Chief = "Згуровський Михайло Захарович",
                    Subordination = "Мінестерство освіти і науки, молоді та спорту України",
                    PostIndex = "03056",
                    Address = "проспект Перемоги, 37",
                    Phone = "+380 44 236 7989",
                    Email = "mail@kpi.ua",
                    Site = "kpi.ua/"
                }
            };

            await _context.Universities.AddRangeAsync(universities).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task SeedEntrantsAsync()
        {
            if (await _context.Entrants.AnyAsync().ConfigureAwait(false))
                return;
            var entrants = new List<EntrantEntity>
            {
                new EntrantEntity
                {
                    FirstName = "Nazar",
                    LastName = "Melnyk",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.1f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 170f,
                        SecondMark = 134f,
                        ThirdMark = 199f,
                        FourthMark = 185f,
                        FirstSubject = "Українська мова та література",
                        SecondSubject = "Англійська мова",
                        ThirdSubject = "Математика",
                        FourthSubject = "Фізика"
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 190,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities)
                                .FirstOrDefaultAsync(x =>
                                    x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First().SpecialtyId
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 190,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First().SpecialtyId
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).FirstOrDefaultAsync(x =>
                                    x.FullName ==
                                    "Національний технічний університет України «Київський політехнічний інститут імені Ігоря Сікорського»")
                                .ConfigureAwait(false)).UniversitySpecialities.First().SpecialtyId
                        }
                    }
                },
                new EntrantEntity
                {
                    FirstName = "Max",
                    LastName = "Lukashyk",
                    Birthday = new DateTimeOffset(1998, 5, 21, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "12394850202332",
                        AverageMark = 9.8f,
                        FullNameOfTheEducationalInstitution = "Камінь-Каширська ЗОШ 1-3 ступеня №1"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "12394123929182",
                        FirstMark = 154f,
                        SecondMark = 140f,
                        ThirdMark = 184f,
                        FourthMark = 0f,
                        FirstSubject = "Українська мова та література",
                        SecondSubject = "Фізика",
                        ThirdSubject = "Математика",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities)
                                .FirstOrDefaultAsync(x =>
                                    x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.Last().SpecialtyId
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.Last().SpecialtyId
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).FirstOrDefaultAsync(x =>
                                    x.FullName ==
                                    "Національний технічний університет України «Київський політехнічний інститут імені Ігоря Сікорського»")
                                .ConfigureAwait(false)).UniversitySpecialities.Last().SpecialtyId
                        }
                    }
                }
            };

            await _context.Entrants.AddRangeAsync(entrants).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task SeedUsersAsync()
        {
            if(await _context.Users.AnyAsync().ConfigureAwait(false))
                return;

            var users = new List<UserEntity>
            {
                new UserEntity
                {
                    FirstName = "Nazar",
                    LastName = "Melnyk",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    Email = "88nazar88@gmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380660367890"
                },
                new UserEntity
                {
                    FirstName = "Anastasi",
                    LastName = "Sokolovska",
                    Birthday = new DateTimeOffset(1997, 9, 4, 0, 0, 0, new TimeSpan()),
                    Email = "sokolovskagmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380672385429"
                },
                new UserEntity
                {
                    FirstName = "Max",
                    LastName = "Lukashyk",
                    Birthday = new DateTimeOffset(1998, 5, 21, 0, 0, 0, new TimeSpan()),
                    Email = "maxkvf@gmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380664578432"
                }
            };

            await _context.Users.AddRangeAsync(users).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
